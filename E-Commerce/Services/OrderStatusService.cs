﻿using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepo repo;

        public OrderStatusService(IOrderStatusRepo repo)
        {
            this.repo = repo;
        }
        public int AddOrderStatus(OrderStatus orderStatus)
        {
           return repo.AddOrderStatus(orderStatus);
        }

        public int DeleteOrderStatus(int id)
        {
           return repo.DeleteOrderStatus(id);
        }

        public int EditOrderStatus(OrderStatus orderStatus)
        {
          return repo.EditOrderStatus(orderStatus);
        }

        public IEnumerable<OrderStatus> GetAllOrderStatus()
        {
          return repo.GetAllOrderStatus();
        }

        public OrderStatus GetOrderStatusById(int id)
        {
       return repo.GetOrderStatusById(id);
        }
    }
}
