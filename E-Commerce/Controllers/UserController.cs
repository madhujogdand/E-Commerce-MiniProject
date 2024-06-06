using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class UserController : Controller
    {
        public readonly IUserService service;
        public UserController(IUserService service)
        {
            this.service = service;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var model = service.GetUsers();
            return View(model);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user=service.GetUserById(id);
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            try
            {
                int result = service.AddUser(user);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var user = service.GetUserById(id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Users user)
        {
            try
            {
                int result = service.EditUser(user);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            var user = service.GetUserById(id);
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                int result = service.DeleteUser(id);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users login)
        {
           

            try
            {
                var user = service.GetUserByEmail(login.Email);

                // Check if the user exists and the password matches
                if (user != null && user.Password == login.Password)
                {
                    // Check the role of the user
                    if (user.RoleId == 1) // Assuming 1 is the role ID for admin
                    {
                        // Store the admin's email in session
                        HttpContext.Session.SetString("UserEmail", login.Email);

                        // Redirect to the product dashboard
                        return RedirectToAction("Create", "Product");
                    }
                    else if (user.RoleId == 2) // Assuming 2 is the role ID for customer
                    {
                        // Store the customer's email in session
                        HttpContext.Session.SetString("UserEmail", login.Email);

                        // Redirect to the product list
                        return RedirectToAction("Index", "Product");
                    }
                }

                // If user or password is incorrect
                ViewBag.ErrorMsg = "Invalid email or password";
                return View(login);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(login);
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
