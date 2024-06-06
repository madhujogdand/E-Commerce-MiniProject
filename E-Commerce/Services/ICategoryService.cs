using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        int AddCategory(Category category);
        int EditCategory(Category category);
        int DeleteCategory(int id);
    }
}
