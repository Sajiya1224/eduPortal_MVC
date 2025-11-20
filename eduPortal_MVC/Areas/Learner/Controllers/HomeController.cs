using System.Diagnostics;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            List<Category> categoryObj = _unitOfWork.Category.GetAll().ToList();
            return View(categoryObj);
        }

        public IActionResult BrowseCourse()
        {
            List<Category> categoryObj = _unitOfWork.Category.GetAll().ToList();
            return View(categoryObj);
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
