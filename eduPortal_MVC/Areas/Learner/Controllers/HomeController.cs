using System.Diagnostics;
using System.Security.Claims;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;

namespace eduPortal.Areas.Learner.Controllers
{
    [Area("Learner")]
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
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
        [HttpGet]
        public IActionResult BrowseCourse( int? categoryId)
        { 
            var categoryList = _unitOfWork.Category.GetAll();
            var courses = _unitOfWork.Course.GetAll(includeproperties: "Category")
                .OfType<VideoCourse>()
                 .Where(u => categoryId == null || u.CategoryId == categoryId)
                 .ToList();
            BrowseCourseVM vmObj = new BrowseCourseVM
            {
                CategoryList = categoryList,
                videoCourse = courses,
                 
            };
            return View(vmObj);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Enrollement(int? categoryId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var learnerUser = await _userManager.FindByIdAsync(UserId);

            IEnumerable<OrderDetails> orderDetailObj = _unitOfWork.OrderDetails
                .GetAll(u => u.OrderHeader.ApplicationUserId == UserId,
                includeproperties: "OrderHeader,OrderHeader.ApplicationUsers, VCourse, VCourse.ApplicationUser, VCourse.Category").ToList();

            int enrolledCoursesCount = orderDetailObj.Count();

            ViewBag.LearnerName = learnerUser?.Name ?? "Unknown Learner";
            ViewBag.EnrolledCoursesCount = enrolledCoursesCount;
            return View(orderDetailObj ?? Enumerable.Empty<OrderDetails>());
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

            // Check if user is enrolled in this course
            bool isEnrolled = false;
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    isEnrolled = _unitOfWork.OrderDetails.GetAll(
                        od => od.OrderHeader.ApplicationUserId == userId && 
                              od.VCourseId == VideoCourseId
                    ).Any();
                }
            }

            ViewBag.InstructorCourseCount = instructorCourseCount;
            ViewBag.IsEnrolled = isEnrolled;
            return View(cart);
        }

      
        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cart.ApplicationUserId = userId;

            Cart cartFromDb = _unitOfWork.Cart.Get(u => u.ApplicationUserId == userId && u.VCourseId == cart.VCourseId);
            if (cartFromDb != null)
            {
                // cart already have something
                TempData["error"] = "You already added this course to cart.";
                return RedirectToAction("BrowseCourse");
            }
            else
            {
                // no cart so need to add one
                _unitOfWork.Cart.Add(cart);
            }
            
            _unitOfWork.Save();
            TempData["success"] = "Course added to cart.";
            return RedirectToAction(nameof(BrowseCourse));
        }

        [HttpGet]
        public IActionResult PlayVideo(int lectureId)
        {
            // Get the lecture with course information
            var lecture = _unitOfWork.Lecture.Get(
                l => l.Id == lectureId,
                includeproperties: "Section,Section.VideoCourse"
            );
            
            if (lecture == null || lecture.Section?.VideoCourse == null)
            {
                return NotFound();
            }
            
            // Check if user is authenticated and enrolled
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    // Check if user has purchased this course
                    var hasAccess = _unitOfWork.OrderDetails.GetAll(
                        od => od.OrderHeader.ApplicationUserId == userId && 
                              od.VCourseId == lecture.Section.VideoCourse.Id
                    ).Any();
                    
                    if (!hasAccess)
                    {
                        return Forbid("You need to enroll in this course to access the video.");
                    }
                }
            }
            else
            {
                // Not authenticated - redirect to login
                return Challenge();
            }
            
            // Check if video URL exists
            if (string.IsNullOrEmpty(lecture.VideoUrl))
            {
                return NotFound("Video not found.");
            }
            
            // Build the full file path
            var filePath = Path.Combine(
                _webHostEnvironment.WebRootPath, 
                lecture.VideoUrl.TrimStart('\\', '/')
            );
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Video file not found on server.");
            }
            
            // Return the video with range support for streaming
            return PhysicalFile(filePath, "video/mp4", enableRangeProcessing: true);
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




