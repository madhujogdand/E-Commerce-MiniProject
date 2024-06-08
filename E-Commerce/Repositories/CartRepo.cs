﻿using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly ApplicationDbContext db;
        public CartRepo(ApplicationDbContext db)
        {
            this.db = db; 
        }
        public int AddToCart(Cart cart)
        {
            //bool exists = CheckIfExists(cart);
            //if (!exists)
            //{
            //    db.Carts.Add(cart);
            //    int res = db.SaveChanges();
            //    return res;
            //}
            //else
            //{
            //    return 2;
            //}
            bool exists = CheckIfExists(cart);
            if (!exists)
            {
                db.Carts.Add(cart);
                int res = db.SaveChanges();
                return res;
            }
            else
            {
                // If the product already exists in the cart, update its quantity instead
                var existingCartItem = db.Carts.FirstOrDefault(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cart.Quantity;
                    return db.SaveChanges();
                }
                else
                {
                    // Handle case where the existing cart item is null
                    return 0;
                }
            }
        }

        public bool CheckIfExists(Cart cart)
        {
            bool exists = false;
            var data=db.Carts.Where(x => x.UserId== cart.UserId).ToList();
            foreach (var item in data) 
            {
                if (item.ProductId == cart.ProductId)
                {
                    exists = true; 
                    break;
                }
                else 
                {
                  exists= false;
                }

            }
            return exists;

            //return db.Carts.Any(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);
        }

        //public ProductCart ConfirmOrder(int id)
        //{
        //    var result = (from p in db.Products
        //                  where p.ProductId == id
        //                  select new ProductCart
        //                  {
        //                      ProductId = p.ProductId,
        //                      ProductName = p.ProductName,
        //                      Price = p.Price,
        //                      Image = p.Image,
        //                      Quantity = 1,

        //                  }).FirstOrDefault();
        //    return result;  
        //}

        public int GetCartCount(int userid)
        {
           var res=(from c in db.Carts
                    where c.UserId==userid
                    select c).Count();
            return res;
        }

     

        //public int PlaceOrder(Orders order)
        //{
        // db.orders.Add(order);
        //    return db.SaveChanges();
        //}

        public int RemoveFromCart(int id)
        {
            int res = 0;
            var cart=db.Carts.Where(x => x.CartId==id).FirstOrDefault();
            if(cart!=null) 
            {
              db.Carts.Remove(cart);
                res=db.SaveChanges();
            }
            return res;
        }

        public int RemoveFromCartAfterOrder(int userid, int productid)
        {
            //var result=(from c in db.Carts
            //            where c.UserId==userid && c.ProductId==productid
            //            select c).FirstOrDefault();
            //  db.Carts.Remove((Cart)result);
            //  int res=db.SaveChanges();
            //  return res;
            var result = db.Carts.FirstOrDefault(c => c.UserId == userid && c.ProductId == productid);
            if (result != null)
            {
                db.Carts.Remove(result);
                return db.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<ProductCart> GetCartItems(int userid)
        {
            var result = (from c in db.Carts
                          join p in db.Products on c.ProductId equals p.ProductId
                          where c.UserId == userid
                          select new ProductCart
                          {
                              Image = p.Image,
                              ProductName = p.ProductName,
                              Price = p.Price,
                              CartId = c.CartId,
                              UserId = c.UserId,
                              ProductId = p.ProductId,
                              Quantity=c.Quantity
                          }).ToList();
            return result;
        }

        public int UpdateQuantity(int cartId, int quantity)
        {
            var cartItem = db.Carts.FirstOrDefault(c => c.CartId == cartId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                return db.SaveChanges();
            }
            return 0; // Cart item not found
        }
    }
}