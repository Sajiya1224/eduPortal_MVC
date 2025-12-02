//using eduPortal.DataAccess.Repository.IRepository;
//using eduPortal.Models.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace eduPortalWeb.Areas.Learner.Controllers
//{
//    [Area("Learner")]
//    [Authorize]
//    public class CartController : Controller
//    {
//        public IUnitOfWork _unitOfWork;
     
//        public CartController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public IActionResult Index()
//        {
//            var claimsIdentity = (ClaimsIdentity)User.Identity;
//            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            

//            return View();

//        }
//    }
//}
