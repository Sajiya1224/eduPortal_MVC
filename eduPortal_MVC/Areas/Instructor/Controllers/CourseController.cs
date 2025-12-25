using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.DataAccess.dataContext;
using eduPortal.Models.ViewModels;
using eduPortal.Models;
using Microsoft.AspNetCore.Mvc;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Collections.Specialized.BitVector32;
using eduPortal.Models.DTOs;
using Section = eduPortal.Models.Section;
using eduPortal.DataAccess.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eduPortal.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "AdminOrInstructor")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVideoCourseService _videoCourseService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserManager<ApplicationUser> _userManager;
        public CourseController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment,
            IVideoCourseService videoCourseService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _videoCourseService = videoCourseService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult CourseType()
        {

            return View();
        }

        [HttpGet]
        public IActionResult UpsertQuiz()
        {
            QuizCourseVM courseVM = new ()
            {
                 CategoryList= _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 }),
                QuizCourse = new QuizCourse()
            };
            return View(courseVM);
        }
        [HttpPost]
        public IActionResult UpsertQuiz(QuizCourse obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Course.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Quiz Course Added Successfully.";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        
        public async Task<IActionResult> UpsertVideo(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(userId);

            VideoCourseVM vm = new VideoCourseVM
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VideoCourseVm = new VideoCourseDTO{
                    Sections = new List<SectionDTO>()
                    {
                    new SectionDTO
                    {
                        SectionTitle ="",
                        Lectures = new List<LectureDTO>
                        {
                            new LectureDTO{LectureTitle="", VideoUrl=""}
                        }
                    }
                    }
                }
            };

            if(id == null || id == 0)
            {
                // create
                return View(vm);
            }
            else
            {
                // update
                
                var courseEntity = _unitOfWork.Course.Get(u => u.Id == id && u is VideoCourse, 
                    "Category,Sections,Sections.Lectures") as VideoCourse;
                
                if (courseEntity == null)
                {
                    return NotFound();
                }

                vm.VideoCourseVm = new VideoCourseDTO
                {
                    Id = courseEntity.Id,   
                    Title = courseEntity.Title,
                    Description = courseEntity.Description,
                    Duration = courseEntity.Duration,
                    Level = courseEntity.Level,
                    CategoryId = courseEntity.CategoryId,
                    Price = courseEntity.Price,
                    ThumbnailUrl = courseEntity.ThumbnailUrl,
                    AddInfo = courseEntity.AddInfo,
                    IsPublished = courseEntity.IsPublished,
                    Sections = courseEntity.Sections.Select(sec => new SectionDTO
                    {
                        Id = sec.Id,
                        SectionTitle = sec.SectionTitle,
                        Lectures = sec.Lectures.Select(lec => new LectureDTO
                        {
                            Id = lec.Id,
                            LectureTitle = lec.LectureTitle,
                            VideoUrl = lec.VideoUrl
                        }
                        ).ToList()

                    }).ToList()
                };

                ViewBag.User = user.Role;
                return View(vm);
            }
           
        }
        [HttpPost]
        public IActionResult UpsertVideo(VideoCourseVM obj, IFormFile? imgFile, IList<IFormFile> vidFile)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            obj.VideoCourseVm.ApplicationUserId = userId;


            // CLEAN SECTIONS
            obj.VideoCourseVm.Sections = obj.VideoCourseVm.Sections
                .Where(s => !string.IsNullOrWhiteSpace(s.SectionTitle))
                .ToList();

            // CLEAN LECTURES - Add this!
            foreach (var section in obj.VideoCourseVm.Sections)
            {
                if (section.Lectures != null)
                {
                    section.Lectures = section.Lectures
                        .Where(l => !string.IsNullOrWhiteSpace(l.LectureTitle))
                        .ToList();
                }
            }

            ModelState.Clear();
            TryValidateModel(obj.VideoCourseVm);

            if (!ModelState.IsValid)
            {

                obj.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });
                return View(obj);
            }




            // getting the saving course logic from videoCourseServices 
            var savingCourse = _videoCourseService.saveCourse(obj, imgFile, vidFile);



            TempData["success"] = savingCourse.Id == 0 ?
            "Course Created Successfully!" :
            "Course Updated Successfully!";

            return RedirectToAction("Index", "Course");
        }



        #region API CALLS
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int? take = null)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(userId);

            IEnumerable<Course> courseObj;

            if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {
                // Admin: see all courses
                courseObj = _unitOfWork.Course.GetAll();
            }
            else if (await _userManager.IsInRoleAsync(user, SD.Role_Instructor))
            {
                // Instructor: only their own courses
                courseObj = _unitOfWork.Course.GetAll(c => c.ApplicationUserId == userId);
            }
            else
            {
                // Other roles (e.g. Learner) – adjust as you like
                courseObj = Enumerable.Empty<Course>();
            }
          
            if (take.HasValue)
            {
                courseObj = courseObj.Take(take.Value);
            }
            ViewBag.User = user.Role;
            var data = courseObj.ToList();
            return Json(new { data });

        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid course ID" });
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var videoCourseList = _unitOfWork.Course.Get(u => u.Id == id && u is VideoCourse,
                     "Category,Sections,Sections.Lectures") as VideoCourse;

            if (videoCourseList == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }


            if (!string.IsNullOrEmpty(videoCourseList.ThumbnailUrl))
            {
                var oldImgPath = Path.Combine(wwwRootPath, videoCourseList.ThumbnailUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }
            }
            if (videoCourseList.Sections != null && videoCourseList.Sections.Any())
            {
                // Get all sections as a list (materialize the collection)
                var sectionsToDelete = videoCourseList.Sections.ToList();
                foreach (var section in sectionsToDelete)
                {
                    if (section.Lectures != null && section.Lectures.Any())
                    {
                        var lecturesToDelete = section.Lectures.ToList();
                        foreach (var lecture in lecturesToDelete)
                        {
                            if (!string.IsNullOrEmpty(lecture.VideoUrl))
                            {
                                var oldVidPath = Path.Combine(wwwRootPath, (lecture.VideoUrl.TrimStart('\\')));

                                if (System.IO.File.Exists(oldVidPath))
                                {
                                    System.IO.File.Delete(oldVidPath);
                                }
                            }
                            _unitOfWork.Lecture.Remove(lecture);
                        }
                    }
                    _unitOfWork.Section.Remove(section);
                }
            }

            _unitOfWork.Course.Remove(videoCourseList);
            _unitOfWork.Save();

            

            return Json(new { success = true, message = "Course deleted successfully" });
        }


        #endregion
    }
}
