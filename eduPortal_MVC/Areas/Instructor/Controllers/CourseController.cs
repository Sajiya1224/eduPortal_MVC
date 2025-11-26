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

namespace eduPortal.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Course> courseObj = _unitOfWork.Course.GetAll().ToList();
            return View(courseObj);
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
        public IActionResult UpsertVideo(int? id)
        {

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
                //var courseEntity = _unitOfWork.Course.Get(u => u.Id == id, includeProperties: "Sections,Lectures") as VideoCourse;
                var courseEntity = _unitOfWork.Course.Get(u => u.Id == id && u is VideoCourse, "Category,Sections,Sections.Lectures") as VideoCourse;
                
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
                return View(vm);
            }
           
        }
        [HttpPost]
        public IActionResult UpsertVideo(VideoCourseVM obj, IFormFile? imgFile, IFormFile? vidFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (imgFile != null)
                {
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                    string imgPath = Path.Combine(wwwRootPath, @"Courses\Thumbnails");

                    if (!string.IsNullOrEmpty(obj.VideoCourseVm.ThumbnailUrl))
                    {
                        var oldImgPath = Path.Combine(wwwRootPath, (obj.VideoCourseVm.ThumbnailUrl.TrimStart('\\')));

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    using (var imgfileStream = new FileStream(Path.Combine(imgPath, imgFileName), FileMode.Create))
                    {
                        imgFile.CopyTo(imgfileStream);
                    }


                    obj.VideoCourseVm.ThumbnailUrl = @"\Courses\Thumbnails\" + imgFileName;
                }
                //Map DTO to entity 
                var course = new VideoCourse
                {
                    Id = obj.VideoCourseVm.Id,
                    Title = obj.VideoCourseVm.Title,
                    Description = obj.VideoCourseVm.Description,
                    Price = obj.VideoCourseVm.Price,
                    Level = obj.VideoCourseVm.Level,
                    Duration = obj.VideoCourseVm.Duration,
                    CategoryId = obj.VideoCourseVm.CategoryId,
                    ThumbnailUrl = obj.VideoCourseVm.ThumbnailUrl
                };

                if(obj.VideoCourseVm.Id == 0)
                {
                    _unitOfWork.Course.Add(course);
                }
                else
                {
                    _unitOfWork.Course.Update(course);
                }
                // Save course
                
                _unitOfWork.Save();

                // for sections and lectures
                foreach (var sec in obj.VideoCourseVm.Sections)
                {
                    var section = new Section
                    {
                        Id = sec.Id,
                        SectionTitle = sec.SectionTitle,
                        VideoCourseId = course.Id,
                    };

                    // save sections 
                    if (sec.Id == 0)
                    {
                        _unitOfWork.Section.Add(section);
                    }
                    else
                    {
                        _unitOfWork.Section.Update(section);
                    }
                    _unitOfWork.Save();

                    foreach (var lec in sec.Lectures)
                    {
                        if (vidFile != null)
                        {
                            string vidFileName = Guid.NewGuid().ToString() + Path.GetExtension(vidFile.FileName);
                            string vidPath = Path.Combine(wwwRootPath, @"Courses\Videos");

                            using (var vidfileStream = new FileStream(Path.Combine(vidPath, vidFileName), FileMode.Create))
                            {
                                vidFile.CopyTo(vidfileStream);
                            }


                            lec.VideoUrl = @"\Courses\Videos" + vidFileName;
                        }
                        var lecture = new Lecture
                        {
                            Id = lec.Id,
                            LectureTitle = lec.LectureTitle,
                            VideoUrl = lec.VideoUrl,
                            SectionId = section.Id
                        };
                        // save lectures
                        if (lec.Id == 0)
                        {
                            _unitOfWork.Lecture.Add(lecture);
                        }
                        else
                        {
                            _unitOfWork.Lecture.Update(lecture);
                        }

                    }
                    _unitOfWork.Save();
                }
                if (obj.VideoCourseVm.Id == 0)
                {
                    TempData["success"] = "Video Course created successfully!";
                }
                else
                {
                    TempData["success"] = "Video Course updated successfully!";
                }
                
                return RedirectToAction("Index", "Course");
            }

            else 
            {
                obj.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });

                return View(obj);
            }
        }

       




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Course> courseObj = _unitOfWork.Course.GetAll().ToList();
            return Json(new { data = courseObj });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var courseToBeDeleted = _unitOfWork.Course.Get(u => u.Id == id);

            if (courseToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }

            _unitOfWork.Course.Remove(courseToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Course deleted successfully" });
        }


        #endregion
    }
}
