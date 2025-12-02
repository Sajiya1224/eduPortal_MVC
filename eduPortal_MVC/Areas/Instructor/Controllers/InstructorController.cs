using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eduPortalWeb.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class InstructorController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public InstructorController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Dashboard()
        {
            var users = _db.ApplicationUsers.ToList();


                var Videocourses = _unitOfWork.Course.GetAll()
                               .OfType<VideoCourse>()
                               .ToList();

            InstructorDashboardVM DashboardObj = new()
            {
                RoleList = new List<SelectListItem>
                {
                    new SelectListItem { Text = SD.Role_Admin, Value = SD.Role_Admin },
                    new SelectListItem { Text = SD.Role_Instructor, Value = SD.Role_Instructor },
                    new SelectListItem { Text = SD.Role_Learner, Value = SD.Role_Learner }
                },
                Users = users,
                VideoCourses = Videocourses
            };

            return View(DashboardObj);
        }
    }
}
