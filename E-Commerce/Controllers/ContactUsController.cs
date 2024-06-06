using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ContactUsController : Controller
    {
        public readonly IContactUsService service;
        public ContactUsController(IContactUsService service)
        {
            this.service = service;
        }
        // GET: ContactUsController
        public ActionResult Index()
        {
            var model = service.GetAllMessages();
            return View(model);
        }

        // GET: ContactUsController/Details/5
        public ActionResult Details(int id)
        {
            var message = service.GetMessageById(id);
            return View(message);
        }

        // GET: ContactUsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactUsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactUs message)
        {
            try
            {
               
                int result = service.AddMessage(message);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(ThankYou));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View(message); 
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(message); // Pass the message back in case of exception
            }
        }
        
        

      
        // GET: ContactUsController/Delete/5
        public ActionResult Delete(int id)
        {
            var message = service.GetMessageById(id);
            return View(message);
        }

        // POST: ContactUsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
               
                int result = service.DeleteMessage(id);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(ThankYou));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    var message = service.GetMessageById(id); // Re-fetch the message
                    return View(message); // Pass the message back in case of error
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var message = service.GetMessageById(id); // Re-fetch the message
                return View(message); // Pass the message back in case of exception
            }
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
