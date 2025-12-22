using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace eduPortalWeb.Areas.Learner.Controllers
{
    [Area("Learner")]
    [Authorize]
    public class CartController : Controller
    {
        public IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM cartVM {  get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            double TotalPrice = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new()
            {
               cartList = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == UserId, includeproperties: "VCourse, VCourse.ApplicationUser"),
               orderHeader = new()
            };
            foreach (var item in cartVM.cartList)
            {
                TotalPrice = TotalPrice + item.VCourse.Price;
            }
            cartVM.orderHeader.OrderTotal = TotalPrice;
            return View(cartVM);
             
        }

        [HttpGet]
        public IActionResult Summary()
        {
            double TotalPrice = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new()
            {
                cartList = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == UserId, includeproperties: "VCourse, VCourse.ApplicationUser"),
                orderHeader = new()
            };

            cartVM.orderHeader.ApplicationUsers = _unitOfWork.ApplicationUser.Get(u => u.Id == UserId);

            foreach (var item in cartVM.cartList)
            {
                TotalPrice = TotalPrice + item.VCourse.Price;
            }
            cartVM.orderHeader.OrderTotal = TotalPrice;
            return View(cartVM);

        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var cartItems = _unitOfWork.Cart.GetAll(
                    u => u.ApplicationUserId == UserId,
                    includeproperties: "VCourse,VCourse.ApplicationUser").ToList();

            if (!cartItems.Any())
                return RedirectToAction(nameof(Index));

            var orderHeader = new OrderHeader
            {
                ApplicationUserId = UserId,
                OrderTotal = cartItems.Sum(i => i.VCourse.Price)
            };

            _unitOfWork.OrderHeader.Add(orderHeader);
            _unitOfWork.Save();

            foreach (var item in cartItems)
            {
                _unitOfWork.OrderDetails.Add(new OrderDetails
                {
                    VCourseId = item.VCourseId,
                    OrderHeaderId = orderHeader.Id,
                    Price = item.VCourse.Price // align with total
                });
                
            }
            _unitOfWork.Save();
           
            var ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == UserId);
            if (ApplicationUser != null)
            {
                var domain = "https://localhost:7214/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"learner/cart/orderConfirmation?id={orderHeader.Id}",
                    CancelUrl = domain + "learner/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in cartItems)
                {
                    var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(item.VCourse.Price * 100),
                            Currency = "usd",
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = item.VCourse.Title
                            }
                        },
                        Quantity = 1
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStripePaymentIdByCart(orderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = orderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

            // Retrieve PaymentIntentId from Stripe after payment is completed
            if (orderHeader != null && !string.IsNullOrEmpty(orderHeader.SessionId) && string.IsNullOrEmpty(orderHeader.PaymentIntendId))
            {
                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower()=="paid")
                {
                    // Update PaymentIntentId now that payment is completed
                    if (!string.IsNullOrEmpty(session.PaymentIntentId))
                    {
                        _unitOfWork.OrderHeader.UpdateStripePaymentIdByCart(id, session.Id, session.PaymentIntentId);
                        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                        _unitOfWork.Save();
                    }
                }

            }

            List<Cart> cartObj = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.Cart.RemoveRange(cartObj);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Delete(int cartId)
        {
            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == cartId);
            _unitOfWork.Cart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }


}
