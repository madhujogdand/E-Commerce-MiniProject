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
                //GetCartCount();
                ViewBag.CartCount = cartService.GetCartCount(userId);
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
                    Quantity = 1,
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
        public IActionResult placeorder(ProductCart productcart)
        {
            try
            {
                int userid = GetCurrentUserId();


                // retrieve cart items for the user
                var cartitems = cartService.GetCartItems(userid);

                // calculate total amount
                decimal totalamount = cartitems.Sum(item => item.Price * item.Quantity);

                // create new order
                var order = new Orders
                {
                    UserId = userid,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalamount,
                    OrderItems = cartitems.Select(item => new OrderItems
                    {

                        ProductId = item.ProductId,
                        OrderStatusId = 6,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
                // place the order
                cartService.PlaceOrder(order);

                // clear the cart after order placement
                foreach (var item in cartitems)
                {
                    cartService.RemoveFromCartAfterOrder(userid, item.ProductId);
                }

                return RedirectToAction("ordersuccess", "order");
            }

            catch (Exception ex)
            {
                ViewBag.errormessage = ex.Message;
                return RedirectToAction("index", "cart");
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

        //orderList Admin side
        //public ActionResult ListOfOrders()
        //{
        //    try
        //    {
        //        //int userId = GetCurrentUserId();
        //        var orders = orderService.GetAllOrders();
        //        if (orders == null || !orders.Any())
        //        {
        //            ViewBag.ErrorMessage = "No orders found.";
        //            return View("Error");
        //        }
        //        return View(orders);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //        return View("Error");
        //    }

        //}

        public IActionResult ListOfOrders()
        {
            try
            {
                var orders = orderService.GetAllOrders();
                if (orders == null || !orders.Any())
                {
                    ViewBag.ErrorMessage = "No orders found.";
                    return View("Error");
                }

                // Fetch order status options
                var orderStatuses = orderStatusService.GetAllOrderStatus()
                    .Select(os => new SelectListItem
                    {
                        Value = os.OrderStatusId.ToString(),
                        Text = os.Status
                    }).ToList();

                ViewBag.OrderStatuses = orderStatuses;

                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
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
        public IActionResult UpdateOrderStatus(int orderItemId, int orderStatusId)
        {
            try
            {
                var result = orderService.UpdateOrderStatus(orderItemId, orderStatusId);
                if (result > 0)
                {
                    return RedirectToAction("ListOfOrders", "Cart");
                }
                else
                {
                    ViewBag.ErrorMessage = "Order item not found.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
