using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly IProductService productService;

        public CartController(ICartService cartService, IUserService userService, IProductService productService)
        {
            this.cartService = cartService;
            this.userService = userService;
            this.productService = productService;
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

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            var product = productService.GetProductById(id);
            return View(product);
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
                    //Quantity = quantity
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

        public IActionResult UpdateQuantity(int cartId, int quantity)
        {
            try
            {
                int result = cartService.UpdateQuantity(cartId, quantity);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Failed to update quantity.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult ConfirmOrder(int cartId)
        {
            try
            {
                var orderItem = cartService.ConfirmOrder(cartId);
                if (orderItem != null)
                {
                    // Here you can implement further logic for order processing
                    // For example, creating an Order and OrderItems in the database
                    // and removing the items from the cart

                    // After processing the order, redirect to an order confirmation page or the cart index
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Unable to confirm order. Please try again.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Index");
            }
        }
    }
}
