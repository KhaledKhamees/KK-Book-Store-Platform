using Microsoft.AspNetCore.Mvc;
using MVCProject2.Models.Models;
using MVCProject2.Models.Veiw_Models;

namespace MVCProject2.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser").ToList();

            return View(orderHeaders);
        }
        public IActionResult Delete(int id)
        {
            var orderheader = _unitOfWork.OrderHeader.Get(u=>u.Id==id);
            if(orderheader != null)
            {
                _unitOfWork.OrderHeader.Remove(orderheader);
                _unitOfWork.Save();
                TempData["success"] = "Order Removed Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Order cann't deleted";
                return RedirectToAction("Index");
            }

        }
    }
}
