using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductService service;
        private readonly ICategoryService categoryService;
        private readonly ICartService cartService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;

        public ProductController(IProductService service, ICategoryService categoryService, ICartService cartService, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.service = service;
            this.categoryService = categoryService;
            this.cartService = cartService;
            this.env = env;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var model = service.GetProducts();
            return View(model);
        }
     
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            //var product = service.GetProductById(id);
            //return View(product);

            var product = service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var model = categoryService.GetCategories();
            ViewBag.Categories = model;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product,IFormFile file)
        {
            try
            {
                using (var fs = new FileStream(env.WebRootPath + "\\Images\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                product.Image = "~/Images/" + file.FileName;

                // Add the product to the database
                int result = service.AddProduct(product);
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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = service.GetProductById(id);
            var categories=categoryService.GetCategories();
            ViewBag.Categories =categories ;
            HttpContext.Session.SetString("oldImageUrl", product.Image);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product,IFormFile file)
        {
            try
            {
                string oldimageurl = HttpContext.Session.GetString("oldImageUrl");
                if (file != null)
                {
                    using (var fs = new FileStream(env.WebRootPath + "\\Images\\" + file.FileName, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }
                    product.Image = "~/Images/" + file.FileName;

                    string[] str = oldimageurl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = env.WebRootPath + "\\Images\\" + str1;
                    System.IO.File.Delete(path);
                }
                else
                {
                    product.Image = oldimageurl;
                }

                int result = service.EditProduct(product);
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

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = service.GetProductById(id);
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                int result = service.DeleteProduct(id);
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

        public ActionResult ProductList(string searchTerm)
        {
            //var model = service.GetProducts();
            //return View(model);

            var model = string.IsNullOrEmpty(searchTerm) ? service.GetProducts() : service.GetProductByName(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(model);
        }



    
    }

}
