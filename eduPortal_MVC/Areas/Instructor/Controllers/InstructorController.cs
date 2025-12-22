using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace eduPortalWeb.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class InstructorController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public InstructorController(IUnitOfWork unitOfWork, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Dashboard()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(userId);

            // Get all courses created by this instructor
            var videoCourses = _unitOfWork.Course
                .GetAll(c => c.ApplicationUserId == userId,
                    includeproperties: "ApplicationUser,Category")
                .OfType<VideoCourse>()
                .ToList();

            // Get all enrollments (OrderDetails) for courses created by this instructor
            var orderDetails = _unitOfWork.OrderDetails
                .GetAll(u => u.VCourse.ApplicationUserId == userId,
                    includeproperties: "OrderHeader,OrderHeader.ApplicationUsers,VCourse,VCourse.ApplicationUser")
                .ToList();

            // Create ViewModel
            var instructorDashboardVM = new InstructorDashboardVM
            {
                VideoCourses = videoCourses,
                OrderDetails = orderDetails
            };

            // Pass additional data to ViewBag if needed
            ViewBag.InstructorUser = user;
            ViewBag.user = user; // For backward compatibility
            ViewBag.InstructorName = user?.Name ?? "Unknown Instructor";

            return View(instructorDashboardVM);


            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var user = await _userManager.FindByIdAsync(userId);

            //var instructorDashboardVM = new InstructorDashboardVM()
            //{
            //    VideoCourses = _unitOfWork.Course
            //        .GetAll(c => c.ApplicationUserId == userId, includeproperties: "ApplicationUser,Category")
            //        .OfType<VideoCourse>()
            //        .ToList(),
            //    OrderDetails = _unitOfWork.OrderDetails
            //      .GetAll(u => u.VCourse.ApplicationUserId == userId,
            //      includeproperties: "OrderHeader,OrderHeader.ApplicationUsers,VCourse,VCourse.ApplicationUser")
            //      .ToList(),

            //};

            //// Count courses created by instructor
            //int instructorCourseCount = instructorDashboardVM.VideoCourses.Count();
            //ViewBag.InstructorCourseCount = instructorCourseCount;
            //ViewBag.InstructorUser = user; // Pass instructor user info

            //return View(instructorDashboardVM);
        }
    }
}
