using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eduPortalWeb.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class InstructorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
