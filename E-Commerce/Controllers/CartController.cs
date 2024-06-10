using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
     

        public CartController(ICartService cartService, IUserService userService)
        {
            this.cartService = cartService;
            this.userService = userService;
          
        }

        private int GetCurrentUserId()
        {
           // Retrieve the user ID from the session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                throw new Exception("User is not authenticated");
            }

            return userId.Value;

        }

        // GET: CartController
        public IActionResult Index()
        {
            try
            {
                int userId = GetCurrentUserId();
                var cartItems = cartService.GetCartItems(userId);
             
                return View(cartItems);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

       

        // POST: CartController/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddToCart(int productId,int userId)
        {
            try
            {
                userId = GetCurrentUserId();
                var cart = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                   
                };
                var result = cartService.AddToCart(cart);
                if (result > 0)
                {
                    // Successfully added to cart
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle failure to add to cart
                    ViewBag.ErrorMessage = "Unable to add product to cart. Please try again.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }

        // POST: CartController/RemoveFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCart(int cartId)
        {
            try
            {
                int result = cartService.RemoveFromCart(cartId);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Failed to remove product from cart.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

       
        public ActionResult ConfirmOrder()
        {
         
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmOrder(int cartId)
        {
            try
            {
                var cartItem = cartService.ConfirmOrder(cartId);
                if (cartItem != null)
                {
                    return View("ConfirmOrder", cartItem);
                }

                ViewBag.ErrorMsg = "Failed to confirm order.";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Index"); 
            }
        }
    }
}
