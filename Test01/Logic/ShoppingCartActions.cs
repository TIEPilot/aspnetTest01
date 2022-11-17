using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test01.Models;

namespace Test01.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCardId { get; set; }
        private ProductContext _db = new ProductContext();
        public const string CartSessionKey = "CartId";
        public void AddToCart(int id)
        {
            ShoppingCardId = GetCartId();
            var cartItem = _db.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCardId 
                && c.ProductId == id);

            if(cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    CartId = ShoppingCardId,
                    Product = _db.Products.SingleOrDefault(p => p.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                _db.ShoppingCartItems.Add(cartItem);
            }else
            {
                cartItem.Quantity++;
            }
            _db.SaveChanges();
        }


        public void Dispose()
        {
            if(_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        private string GetCartId()
        {
            if(HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCardId = GetCartId();
            return _db.ShoppingCartItems.Where(
                c => c.CartId == ShoppingCardId).ToList();
        }
        
    }
}