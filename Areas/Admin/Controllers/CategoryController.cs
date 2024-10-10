using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject2.Data;
using MVCProject2.Models;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using MVCProject2.Utility;

namespace MVCProject2.Areas.Admin.Controllers
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
            List<Category> CategoriesList = _unitOfWork.Category.GetAll().ToList();
            return View("Index", CategoriesList);
        }
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult SaveNew(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Sussessfully";
                return RedirectToAction("Index");
            }
            return View("Create", category);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Category not found";
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                TempData["error"] = "Category not found";
                return NotFound();
            }

            return View("SaveEdit", category);
        }


        [HttpPost]
        public IActionResult SaveEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Category.Update(category);
                    _unitOfWork.Save();
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Error updating category: " + ex.Message;
                    return View("SaveEdit", category);
                }
            }
            TempData["error"] = "Please correct the validation errors.";
            return View("SaveEdit", category);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Sussessfully";
            return RedirectToAction("Index");
        }

    }
}
