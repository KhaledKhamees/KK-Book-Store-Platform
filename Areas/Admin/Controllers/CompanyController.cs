using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject2.Models.Models;
using MVCProject2.Utility;

namespace MVCProject2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Retrieve all companies and pass them to the view
            List<Company> CompaniesList = _unitOfWork.Company.GetAll(null, null).ToList();
            return View(CompaniesList); // View name is inferred from action name
        }

        public IActionResult Create()
        {
            return View(); // View name is inferred from action name
        }

        [HttpPost]
        public IActionResult SaveNew(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Add(company);
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            // If validation fails, return the same view with the model
            return View("Create", company);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Company not found";
                return NotFound(); // Consider returning a view instead
            }

            var company = _unitOfWork.Company.Get(c => c.Id == id);
            if (company == null)
            {
                TempData["error"] = "Company not found";
                return NotFound(); // Consider returning a view instead
            }

            return View("SaveEdit", company);
        }

        [HttpPost]
        public IActionResult SaveEdit(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure company is not null
                    if (company != null)
                    {
                        _unitOfWork.Company.Update(company);
                        _unitOfWork.Save();
                        TempData["success"] = "Company updated successfully";
                        return RedirectToAction("Index");
                    }
                    TempData["error"] = "Company cannot be null.";
                    return View("SaveEdit", company);
                }
                catch (Exception ex)
                {
                    // Log the exception (you could use a logging framework)
                    TempData["error"] = "Error updating company: " + ex.Message;
                    return View("SaveEdit", company);
                }
            }
            TempData["error"] = "Please correct the validation errors.";
            return View("SaveEdit", company);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Consider returning a view instead
            }
            var company = _unitOfWork.Company.Get(c => c.Id == id);
            if (company == null)
            {
                return NotFound(); // Consider returning a view instead
            }
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
