using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.DataAccess.dataContext;
using eduPortal.Models;
using Microsoft.AspNetCore.Mvc;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;

namespace eduPortal.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Course> courseObj = _unitOfWork.Course.GetAll().ToList();
            return View(courseObj);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Course obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Course.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Course Addes Successfully";
            }
            return RedirectToAction("Index");
        }

    }
}
