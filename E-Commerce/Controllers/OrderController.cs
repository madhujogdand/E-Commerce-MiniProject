using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
    

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
           
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }

      

       
    }
}
