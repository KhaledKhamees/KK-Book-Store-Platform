using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Models.Models;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using MVCProject2.Utility;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(null, "Category").ToList();            
            return View(productList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll(null, null).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = CategoryList;    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0) 
                {
                    // Generate a unique file name and get the file extension
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    // Define the path where the image will be stored
                    string productPath = Path.Combine(wwwRootPath, "Images", "Product");
                    // Ensure the directory exists, create it if necessary
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }
                    // Save the uploaded file
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    // Set the image URL to the correct path
                    product.ImageUrl = "/Images/Product/" + fileName;
                }
                // Add product to the database and save
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                // Return success message and redirect
                TempData["success"] = "Product added successfully.";
                return RedirectToAction(nameof(Index));
            }
            // In case of error, return to the view
            TempData["error"] = "There was an error while creating the product.";
            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            return View("SaveEdit", product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    // Generate a unique file name and get the file extension
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    // Define the path where the image will be stored
                    string productPath = Path.Combine(wwwRootPath, "Images", "Product");
                    // Ensure the directory exists, create it if necessary
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        //delete the old image 
                        var oldImage = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImage)) 
                        { 
                            System.IO.File.Delete(oldImage);
                        }

                    }
                    // Save the uploaded file
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    // Set the image URL to the correct path
                    product.ImageUrl = "/Images/Product/" + fileName;
                }

                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "There were validation errors.";
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        #region APIsCalls
        //public IActionResult GetAll()
        //{
        //    List<Product> objproductList = _db.GetAll().ToList();
        //    return Json(new {data =  objproductList });
        //}
        #endregion
    }
}
