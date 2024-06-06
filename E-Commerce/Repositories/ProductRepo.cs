using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext db;

        public ProductRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddProduct(Product product)
        {
            int result = 0;
            db.Products.Add(product);
            result = db.SaveChanges();
            return result;
        }

        public int DeleteProduct(int id)
        {
            int result = 0;
            var model = db.Products.Where(prod => prod.ProductId == id).FirstOrDefault();
            if (model != null)
            {
                db.Products.Remove(model);
                result = db.SaveChanges();
            }
            return result;
        }

        public int EditProduct(Product product)
        {
            int result = 0;
            var model = db.Products.Where(prod => prod.ProductId == product.ProductId).FirstOrDefault();
            if (model != null)
            {
                model.ProductName = product.ProductName;
                model.Price= product.Price;
                model.CategoryId = product.CategoryId;
                model.Stock= product.Stock;
                model.Description = product.Description;
                model.Image= product.Image;
                result = db.SaveChanges();
            }
            return result;
        }

        public Product GetProductById(int id)
        {
            return db.Products.Where(x => x.ProductId == id).SingleOrDefault();
        }

        public IEnumerable<Product> GetProductByName(string name)
        {
            var model = from product in db.Products
                        where product.ProductName.Contains(name)
                        select product;
            return model;
        }

        public IEnumerable<Product> GetProducts()
        {
            var model = (from product in db.Products
                         join cat in db.Categories on product.CategoryId equals cat.CategoryId
                         select new Product
                         {
                            ProductId = product.ProductId,
                            ProductName= product.ProductName,
                            CategoryId=product.CategoryId,
                            Categoryname=cat.CategoryName,
                            Stock=product.Stock,
                            Price=product.Price,
                            Description=product.Description,    
                            Image= product.Image,
                         }).ToList();
            return model;
        }

      
    }
}
