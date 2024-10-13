using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject2.Models.Models;
using MVCProject2.Models.Veiw_Models;
using MVCProject2.Utility;
using Stripe.Checkout;
using System.Security.Claims;

namespace MVCProject2.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; 
            ShoppingCartVM = new()
            {
                shoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplecationUserId==UserId, "Product").ToList(),
                OrderHeader = new OrderHeader()
            };
            foreach(var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = CalculateTotalBasedOnCount(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price*cart.count);
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                // Handle case when UserId is null, e.g., redirect to login or show an error
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM = new()
            {
                shoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplecationUserId == UserId, "Product").ToList(),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplecationUser.Get(u => u.Id == UserId);

            if (ShoppingCartVM.OrderHeader.ApplicationUser == null)
            {
                // Handle the case when the user is not found in the database
                return RedirectToAction("Error", "Home");
            }

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name ?? "Name not available";
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City ?? "City not available";
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostelCode ?? "Postal Code not available";
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber ?? "Phone number not available";

            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = CalculateTotalBasedOnCount(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product?.ListPrice ?? 0) * cart.count; // Ensure product price isn't null
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPost()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM.shoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplecationUserId == UserId, "Product").ToList();
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = UserId;
            ApplecationUser applicationUser = _unitOfWork.ApplecationUser.Get(u=>u.Id == UserId);
					
			foreach (var cart in ShoppingCartVM.shoppingCartsList)
			{
				cart.Price = CalculateTotalBasedOnCount(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.ListPrice * cart.count);
			}
            if (applicationUser.CompanyId.GetValueOrDefault() == 0) { 
                //customer order status 
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending; 
                ShoppingCartVM.OrderHeader.PaymentStatus= SD.PaymentStatusPending;
			}
            else
            {
				//company order status 
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
			}
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach(var cart in ShoppingCartVM.shoppingCartsList)
            {
                OrderDetail orderDetail = new ()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
			if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
                //customer payment logic 
                var DomainURL = "https://localhost:44315/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = DomainURL+ $"Customer/ShoppingCart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = DomainURL+ "Customer/ShoppingCart/Index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };
                foreach (var item in ShoppingCartVM.shoppingCartsList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.50 => 2050 (amount in cents)
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id,session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);



                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			}

			return RedirectToAction(nameof(OrderConfirmation),new {id= ShoppingCartVM.OrderHeader.Id});

		}
        public IActionResult OrderConfirmation(int id)
        {
            // Fix the includeProperties typo
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SesstionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            // Fix the property name here too
            List<ShoppingCart> UserShoppingCart = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplecationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(UserShoppingCart);
            _unitOfWork.Save();

            return View(id);
        }



        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(c=> c.Id==cartId);
            cartFromDb.count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            if(cartFromDb.count <=1 ){
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.count-=1;
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        private double CalculateTotalBasedOnCount(ShoppingCart shoppingCart)
        {
            if (shoppingCart.count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }

        }
    }
}
