using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject2.Models;
using MVCProject2.Models.Models;
using MVCProject2.Reprository.IRepository;
using System.Diagnostics;
using MVCProject2.Utility; 
using System.Security.Claims;

namespace MVCProject2.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(null, null);
            return View(products);
        }
        public IActionResult Details(int id )
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.Product.Get(p => p.Id == id),
                count = 1,
                ProductId = id
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplecationUserId = UserId;
            ShoppingCart ShoCartFromDb = _unitOfWork.ShoppingCart.Get(U=>U.ApplecationUserId== UserId && U.ProductId==shoppingCart.ProductId);
            if (ShoCartFromDb != null)
            {
                ShoCartFromDb.count += shoppingCart.count;
                _unitOfWork.ShoppingCart.Update(ShoCartFromDb); 
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                //HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(U => U.ApplecationUserId == UserId).Count());
                _unitOfWork.Save();
            }
            TempData["success"] = "Cart updated Sussessfully";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
