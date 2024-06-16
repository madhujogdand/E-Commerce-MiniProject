using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepo repo;

        public CartService(ICartRepo repo)
        {
            this.repo = repo;
        }
        public int AddToCart(Cart cart)
        {
          return repo.AddToCart(cart);
        }

        public bool CheckIfExists(Cart cart)
        {
          return repo.CheckIfExists(cart);
        }

        public ProductCart ConfirmOrder(int id)
        {
         return repo.ConfirmOrder(id);
        }

        public int GetCartCount(int userid)
        {
        return repo.GetCartCount(userid);
        }

        public IEnumerable<ProductCart> GetCartItems(int userid)
        {
        return repo.GetCartItems(userid);
        }

        public int PlaceOrder(Orders order)
        {
          return repo.PlaceOrder(order);
        }

        public int RemoveFromCart(int id)
        {
          return repo.RemoveFromCart(id);
        }

        public int RemoveFromCartAfterOrder(int userid, int productid)
        {
          return repo.RemoveFromCartAfterOrder(userid, productid);    
        }

        public int UpdateQuantity(int cartId, int quantity)
        {
           return repo.UpdateQuantity(cartId, quantity);
        }
    }
}
