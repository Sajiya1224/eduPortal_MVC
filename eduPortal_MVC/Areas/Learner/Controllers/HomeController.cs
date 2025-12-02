using System.Diagnostics;
using System.Security.Claims;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace eduPortal.Areas.Learner.Controllers
{
    [Area("Learner")]
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categoryList = _unitOfWork.Category.GetAll();
            var courses = _unitOfWork.Course.GetAll(includeproperties:"Category").OfType<VideoCourse>().ToList();
            BrowseCourseVM vmObj = new BrowseCourseVM
            {
                CategoryList = categoryList,
                videoCourse = courses
            };
            return View(vmObj);
        }

        public IActionResult BrowseCourse( int courseId)
        { 
            var categoryList = _unitOfWork.Category.GetAll();
            var courses = _unitOfWork.Course.GetAll(includeproperties: "Category").OfType<VideoCourse>().ToList();
            BrowseCourseVM vmObj = new BrowseCourseVM
            {
                CategoryList = categoryList,
                videoCourse = courses,
                 
            };
            return View(vmObj);
        }
        [HttpGet]
        public IActionResult Details(int VideoCourseId)
        {
            var vCourse = _unitOfWork.Course.Get(
                u => u.Id == VideoCourseId && u is VideoCourse,
                includeproperties: "ApplicationUser,Sections,Sections.Lectures,Category"
            ) as VideoCourse;

            if (vCourse == null)
            {
                return NotFound();
            }

            var instructorId = vCourse.ApplicationUserId;
            var AUser = _unitOfWork.ApplicationUser.Get(c => c.Id == instructorId);

            int instructorCourseCount = 0;

            if (!string.IsNullOrEmpty(instructorId))
            {
                var allCourses = _unitOfWork.Course.GetAll(includeproperties: "ApplicationUser,Sections,Sections.Lectures,Category");

                if (allCourses != null)
                {
                    instructorCourseCount = allCourses
                        .Count(c => c.ApplicationUserId == instructorId && c is VideoCourse);
                }
            }

            Cart cart = new()
            {
                VCourse = vCourse,
                VCourseId = VideoCourseId,
                ApplicationUserId = instructorId,
                ApplicationUsers = AUser
            };

            ViewBag.InstructorCourseCount = instructorCourseCount;
            return View(cart);
        }
      
        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cart.ApplicationUserId = userId;

            _unitOfWork.Cart.Add(cart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }











        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
