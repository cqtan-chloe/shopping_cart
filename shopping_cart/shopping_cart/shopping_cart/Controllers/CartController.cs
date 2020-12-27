using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using GDipSA51_Team5.Models;
using GDipSA51_Team5.Data;
using System.Text.Json;
using System;
using Castle.Core.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Diagnostics;

namespace GDipSA51_Team5.Controllers
{
    public class CartController : Controller
    {
        private readonly Team5_Db db;
        private readonly string userId;
        private readonly string sessionId;

        public CartController(Team5_Db db)
        {
            this.db = db;
            try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }
        }

        //receive JSON data from Add.js. (When an item is added to the cart from gallery)
        public void AddItemToCart([FromBody] Addinput product)
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.pId == product.ProductId);

            if (item == null)
            {
                item = new CartItem();

                item.UserId = userId;
                item.pId = product.ProductId;
                item.Quantity = 1;
                item.product = db.Products.FirstOrDefault(x => x.ProductId == int.Parse(product.ProductId));
                db.Add(item);
            }
            else
            {
                item.Quantity += 1;
                db.Update(item);
            }

            db.SaveChanges();
        }

        public IActionResult ListCartItems()//HttpGET on the cart() action
        {
            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();
           
            ViewData["cart"] = cart;
            return View("Cart");
        }

        [HttpPost]
        public void ChangeCartItemQuantity([FromBody] ChangeInput change)//receive JSON object from Cart.js when the number in the cart is changed
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.pId == change.ProductId);

            item.Quantity = int.Parse(change.Value);

            db.Cart.Update(item);
            db.SaveChanges();
        }

        [HttpPost]
        public void DeleteCartItem([FromBody] Addinput product)
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.pId == product.ProductId);

            db.Cart.Remove(item);
            db.SaveChanges();
        }

        [HttpPost] 
        public IActionResult Checkout() // checkout is form deletion from Cart and adding to PurchaseHistory
        {
            if (HttpContext.Request.Cookies["sessionId"] == null) return RedirectToAction("Login", "Login");

            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();

            foreach (CartItem item in cart) 
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    Purchase order = new Purchase
                    {
                        ActivationCode = Guid.NewGuid().ToString().Substring(3, 15),
                        UserId = int.Parse(item.UserId),
                        ProductId = int.Parse(item.pId),
                        PurchaseDate = DateTime.Today.Date,
                        ListingId = (item.pId + DateTime.Today.Date).GetHashCode()
                    };

                    db.PurchaseHistory.Add(order);
                }
                
                db.Cart.Remove(item); 
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}