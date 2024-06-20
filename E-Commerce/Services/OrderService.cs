using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo repo;

        public OrderService(IOrderRepo repo)
        {
            this.repo = repo;
        }

   

        public IEnumerable<Orders> GetOrders(int userId)
        {
            return repo.GetOrders(userId);
        }

        public IEnumerable<Orders> GetAllOrders()
        {
         return repo.GetAllOrders();
        }

        public int UpdateOrderStatus(int orderItemId, int orderStatusId)
        {
          return repo.UpdateOrderStatus(orderItemId, orderStatusId);
        }
    }
}
