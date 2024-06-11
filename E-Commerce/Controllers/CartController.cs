using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        private readonly IOrderStatusService orderStatusService;
        private readonly IProductService productService;


        public CartController(ICartService cartService, IUserService userService, IOrderService orderService, IProductService productService, IOrderStatusService orderStatusService)
        {
            this.cartService = cartService;
            this.userService = userService;
            this.orderService = orderService;
            this.productService = productService;
            this.orderStatusService = orderStatusService;
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
                // Update cart count
                GetCartCount();
                return View(cartItems);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
        public IActionResult GetCartCount()
        {
            try
            {
                int userId = GetCurrentUserId();
                int cartCount = cartService.GetCartCount(userId);
                ViewBag.CartCount = cartCount;
                return View(Index);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                ViewBag.Error = ex.Message;
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

                // Fetch product details
                var product = productService.GetProductById(productId);
                if (product.Stock <= 0)
                {
                    // If stock is not available, show a notification message
                    TempData["NotifyMessage"] = "Stock not available. You will be notified when the product is back in stock.";
                    return RedirectToAction("ProductList", "Product");
                }

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
                    var orderStatuses = orderStatusService.GetAllOrderStatus()
                    .Select(os => new SelectListItem
                    {
                      Value = os.OrderStatusId.ToString(),
                      Text = os.Status
                    }).ToList();

                    ViewBag.OrderStatuses = orderStatuses;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(int productId, int quantity, int orderStatusId)
        {
            try
            {
                int userId = GetCurrentUserId();

                // Retrieve cart items for the user
                var cartItems = cartService.GetCartItems(userId);

                // Calculate total amount
                decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

                // Create new order
                var order = new Orders
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    OrderItems = cartItems.Select(item => new OrderItems
                    {
                        ProductId = item.ProductId,
                        OrderStatusId = orderStatusId,  
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
                // Place the order
                cartService.PlaceOrder(order);

                // Clear the cart after order placement
                foreach (var item in cartItems)
                {
                    cartService.RemoveFromCartAfterOrder(userId, item.ProductId);
                }

                return RedirectToAction("OrderSuccess", "Order");
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index", "Cart");
            }
        }

        public IActionResult OrderList()
        {
            try
            {
                int userId = GetCurrentUserId();
                var orders = orderService.GetOrders(userId);
                if (orders == null)
                {
                    ViewBag.ErrorMessage = "No orders found.";
                    return View("Error");
                }
                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
