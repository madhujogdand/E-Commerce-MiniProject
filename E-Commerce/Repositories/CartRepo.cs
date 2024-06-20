using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == cart.ProductId);

                if (product != null && product.Stock >= cart.Quantity && cart.Quantity > 0)
                {
                    var existingCartItem = db.Carts.FirstOrDefault(x => x.UserId == cart.UserId && x.ProductId == cart.ProductId);
                    if (existingCartItem == null)
                    {
                        db.Carts.Add(cart);
                    }
                    else
                    {
                        existingCartItem.Quantity += cart.Quantity;
                    }
                    return db.SaveChanges();
                }
                else if (product == null)
                {
                    throw new Exception($"Product with ID {cart.ProductId} not found.");
                }
                else if (product.Stock < cart.Quantity)
                {
                    throw new Exception($"Insufficient stock for product '{product.ProductName}'. Available stock: {product.Stock}");
                }
                else if (cart.Quantity <= 0)
                {
                    throw new Exception($"Quantity must be greater than zero.");
                }
                else
                {
                    throw new Exception("Unknown error occurred while adding to cart.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as per your application's error handling strategy
                throw new Exception("Failed to add item to cart.", ex);
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

         
        }

        public ProductCart ConfirmOrder(int id)
        {
            var result = (from c in db.Carts
                          join p in db.Products on c.ProductId equals p.ProductId
                          where c.CartId == id
                          select new ProductCart
                          {
                              ProductId = p.ProductId,
                              ProductName = p.ProductName,
                              Price = p.Price,
                              Image = p.Image,
                              Quantity = c.Quantity,  
                              CartId = c.CartId,
                              UserId = c.UserId
                          }).FirstOrDefault();
            return result;
        }


        public int GetCartCount(int userid)
        {
           var res=(from c in db.Carts
                    where c.UserId==userid
                    select c).Count();
            return res;
        }



        public int PlaceOrder(Orders order)
        {
            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);

                    if (product != null)
                    {
                        // Check if sufficient stock is available
                        if (product.Stock >= item.Quantity)
                        {
                            // Decrease product stock
                            product.Stock -= item.Quantity;
                        }
                        else
                        {
                            // Insufficient stock, return an error status or message
                            return -1; // or you can throw an exception here if needed
                        }
                    }
                    else
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }
                }

                // Add order to Orders table
                db.Orders.Add(order);
                db.SaveChanges();

                return order.OrderId;
            }
            catch (Exception ex)
            {
                // Handle exceptions as per your application's error handling strategy
                throw new Exception("Failed to place order.", ex);
            }
        }

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
                              Quantity=c.Quantity,
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
