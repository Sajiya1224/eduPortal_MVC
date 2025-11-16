
using Microsoft.AspNetCore.Mvc;
using eduPortal.Models;
using eduPortal.DataAccess.Repository.IRepository;


namespace eduPortalWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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


        public IActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category? categoryObj = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
