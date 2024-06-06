using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        IEnumerable <Product> GetProductByName(string name);
        int AddProduct(Product product);
        int EditProduct(Product product);
        int DeleteProduct(int id);
       
    }
}
