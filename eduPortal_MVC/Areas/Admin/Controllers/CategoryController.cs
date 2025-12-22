
using Microsoft.AspNetCore.Mvc;
using eduPortal.Models;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;


namespace eduPortalWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categoryObj = _unitOfWork.Category.GetAll().ToList();
            return View(categoryObj);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Added Successfully.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryObj = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            return View(categoryObj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Updated Successfully.";
            }

            return RedirectToAction("Index");
        }


    #region API CALLS
    [HttpGet]
        public IActionResult GetAll(int? take = null)
        {
            IEnumerable<Category> categoryObj = _unitOfWork.Category.GetAll();

            if (take.HasValue)
            {
                categoryObj = categoryObj.Take(take.Value);
            }

            var data = categoryObj.ToList();
            return Json(new { data });
           
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var categoryToBeDeleted = _unitOfWork.Category.Get(u => u.Id == id);
           
            if(categoryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Erroe while deleting" });

            }

            _unitOfWork.Category.Remove(categoryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Category deleted successfully" });
        }


        #endregion
    }
}
