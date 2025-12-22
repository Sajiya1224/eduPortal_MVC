using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eduPortalWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EnrollementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public EnrollementController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(userId);
            var isInstructor = await _userManager.IsInRoleAsync(user, SD.Role_Instructor);

            IEnumerable<OrderDetails> orderDetailObj;

            if (isInstructor)
            {
                // Filter: Only enrollments for courses created by this instructor
                orderDetailObj = _unitOfWork.OrderDetails
                    .GetAll(u => u.VCourse.ApplicationUserId == userId,
                    includeproperties: "OrderHeader,OrderHeader.ApplicationUsers,VCourse,VCourse.ApplicationUser")
                    .ToList();
            }
            else if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {
                // Get all enrollments
                orderDetailObj = _unitOfWork.OrderDetails
                    .GetAll(includeproperties: "OrderHeader,OrderHeader.ApplicationUsers,VCourse,VCourse.ApplicationUser")
                    .ToList();
            }
            else
            {
                // Handle other roles (e.g., Learner) or return empty
                orderDetailObj = new List<OrderDetails>();
            }

            return View(orderDetailObj);
        }











        #region API CALLS

        [HttpGet]

        public IActionResult EnrollementDetail(int? take = null)
        {

            IEnumerable<OrderDetails> orderDetailObj;

            if (User.IsInRole(SD.Role_Admin))
            {
                orderDetailObj = _unitOfWork.OrderDetails
                    .GetAll(includeproperties: "OrderHeader,OrderHeader.ApplicationUsers, VCourse, VCourse.ApplicationUser");
            }
            else if (User.IsInRole(SD.Role_Instructor))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderDetailObj = _unitOfWork.OrderDetails
                    .GetAll(u => u.VCourse.ApplicationUserId == UserId,
                    includeproperties: "OrderHeader,OrderHeader.ApplicationUsers, VCourse, VCourse.ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderDetailObj = _unitOfWork.OrderDetails
                    .GetAll(u => u.OrderHeader.ApplicationUserId == UserId,
                    includeproperties: "OrderHeader,OrderHeader.ApplicationUsers, VCourse, VCourse.ApplicationUser");
            }
           
            if (take.HasValue)
            {
                orderDetailObj = orderDetailObj.Take(take.Value);
            }

            var data = orderDetailObj.ToList();
            return Json(new { data });
        }
        #endregion
    }
}
