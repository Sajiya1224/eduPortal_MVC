using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.DTOs;
using eduPortal.Models.ViewModels;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace eduPortalWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }



        public async Task<IActionResult> Dashboard()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var adminUser = await _userManager.FindByIdAsync(userId);

            // Get all users count
            var totalUsers = _unitOfWork.ApplicationUser.GetAll().Count();

            // Get all courses count
            var totalCourses = _unitOfWork.Course.GetAll().Count();

            // Get all enrollments count
            var totalEnrollments = _unitOfWork.OrderDetails.GetAll().Count();

            // Calculate total revenue (sum of all order details prices)
            var totalRevenue = _unitOfWork.OrderDetails.GetAll().Sum(od => od.Price);

                       // Get active students count (students who have enrolled in at least one course)
            var activeStudents = _unitOfWork.OrderDetails
                .GetAll(includeproperties: "OrderHeader")
                .Select(od => od.OrderHeader.ApplicationUserId)
                .Distinct()
                .Count();

            // Get active instructors count (instructors who have created at least one course)
            var activeInstructors = _unitOfWork.Course
                .GetAll()
                .Select(c => c.ApplicationUserId)
                .Distinct()
                .Count();

            // Get published courses count
            var publishedCourses = _unitOfWork.Course
                .GetAll()
                .OfType<VideoCourse>()
                .Count(c => c.IsPublished == true);

            // Create ViewModel
            var adminDashboardVM = new AdminDashboardVM
            {
                TotalUsersCount = totalUsers,
                TotalCoursesCount = totalCourses,
                TotalEnrollmentsCount = totalEnrollments,
                TotalRevenue = totalRevenue,
                ActiveInstructor = activeInstructors,
                ActiveStudents = activeStudents,
                PublishedCourses = publishedCourses
            };

            ViewBag.AdminUser = adminUser;
            ViewBag.AdminName = adminUser?.Name ?? "Unknown Admin";
            return View(adminDashboardVM);
        }



        [HttpGet]
        [Authorize]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            // Search in courses
            var courses = _unitOfWork.Course.GetAll(
                includeproperties: "Category,ApplicationUser"
            )
            .OfType<VideoCourse>()
            .Where(c =>
                c.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Category.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (c.ApplicationUser != null && c.ApplicationUser.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();

            // Search in users (if needed)
            var users = _unitOfWork.ApplicationUser.GetAll()
                .Where(u =>
                    u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Courses = courses;
            ViewBag.Users = users;

            return View();
        }


        public IActionResult InstructorData()
        {
            List<ApplicationUser> userObj = _unitOfWork.ApplicationUser.GetAll().ToList();
            return View(userObj);
        }

        public IActionResult InstructorDetail(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser? userObj = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (userObj == null)
            {
                return NotFound();
            }
            return View(userObj);
        }

        public IActionResult UserData()
        {
            //List<ApplicationUser> userObj = _unitOfWork.ApplicationUser.GetAll().ToList();
            return View();
        }

        public IActionResult CourseDetail()
        {
            List<Course> courseObj = _unitOfWork.Course.GetAll().ToList();
            if (courseObj == null)
            {
                return NotFound();
            }
            return View(courseObj);

        }

        [HttpPost]
        
        public async Task<IActionResult> Approve(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "Invalid instructor ID";
                return NotFound();
 
            }

            var instructor = await _userManager.FindByIdAsync(id);
            if (instructor == null)
            {
                TempData["error"] = "Instructor not found";
                return NotFound();
               
            }

            // Check if user is actually an instructor
            var isInstructor = await _userManager.IsInRoleAsync(instructor, SD.Role_Instructor);
            if (!isInstructor)
            {
                TempData["error"] = "User is not an instructor";
                return NotFound();
             }

            // Approve the instructor
            instructor.IsApproved = true;
            var result = await _userManager.UpdateAsync(instructor);


           

            if (result.Succeeded)
            {
                TempData["Success"] = "Instructor approved successfully";

            }
            else
            {
                TempData["error"] = "Failed to approve instructor";

            }
           
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "Invalid instructor ID";
                return NotFound();
                
            }

            var instructor = await _userManager.FindByIdAsync(id);
            if (instructor == null)
            {
                TempData["error"] = "Instructor not found";
                return NotFound();
                
            }

            // Reject the instructor (set IsApproved to false)
            instructor.IsApproved = false; // false = Rejected
            var result = await _userManager.UpdateAsync(instructor);

            _unitOfWork.ApplicationUser.Remove(instructor);
            _unitOfWork.Save();

            if (result.Succeeded)
            {
                TempData["Success"] = "Instructor rejected";

            }
            else
            {
                TempData["error"] = "Failed to reject instructor";

            }
            return RedirectToAction("Dashboard");
        }






        #region API CALLS

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> SyncUserRoles()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var primary = roles.FirstOrDefault();
                if (!string.IsNullOrEmpty(primary) && user.Role != primary)
                {
                    user.Role = primary;
                    await _userManager.UpdateAsync(user);
                }
            }
            return Ok("Synced");
        }


        [HttpGet]
        public async Task<IActionResult> GetInstructor(int? take = null)
        {
           
            //var instructors = _unitOfWork.ApplicationUser.GetAll(u => u.Role == SD.Role_Instructor).ToList();
            var users = await _userManager.GetUsersInRoleAsync(SD.Role_Instructor);

            var instructors = users.Where(u => u.IsApproved == null)
                .Select(u => new { 
                    name = u.Name, 
                    email = u.Email, 
                    gender = u.Gender,
                    role = u.Role,
                    cv = u.CV, 
                    demoVid = u.DemoVid, 
                    id = u.Id, 
                    isApproved = u.IsApproved 
                });

            if (take.HasValue)
            {
                instructors = instructors.Take(take.Value);
            }

            var data = instructors;
            return Json(new { data });
        }



        [HttpGet]
        public async Task<IActionResult> GetUser(int? take = null)
        {
            IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll();
           
            var userData = users.Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                gender = u.Gender,
                role = u.Role
            });
            
            if (take.HasValue)
            {
                userData = userData.Take(take.Value);
            }

            var data = userData.ToList();
            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> CourseData(int? take = null)
        {
            IEnumerable<VideoCourse> courseEntity = _unitOfWork.Course
                .GetAll(includeproperties: "Category,Sections,Sections.Lectures")
                .OfType<VideoCourse>();
            if (take.HasValue)
            {
                courseEntity = courseEntity.Take(take.Value);
            }

            var data = courseEntity.ToList();
            return Json(new { data });
        }

     

        #endregion
    }
}
