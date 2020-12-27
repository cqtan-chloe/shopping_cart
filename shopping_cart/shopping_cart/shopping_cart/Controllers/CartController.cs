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
            try { userId = HttpContext.Request.Cookies["userId"]; } catch (NullReferenceException) { userId = Environment.MachineName; }
            try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
        }

        //receive JSON data from Add.js. (When an item is added to the cart from gallery)
        public JsonResult AddItemToCart([FromBody] Addinput product)
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.ProductId == product.ProductId);

            if (item == null)
            {
                item.UserId = userId;
                item.ProductId = product.ProductId;
            }

            item.Quantity += 1;

            db.Add(item);
            db.SaveChanges();

            //return the total as JSON to the Add.js
            return Json(new
            {
                status = "success",
                total = item.Quantity
            });
        }

        public IActionResult ListCartItems()//HttpGET on the cart() action
        {
            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();

            ViewData["cart"] = cart;
            return View("Cart");
        }

        [HttpPost]
        public string ChangeCartItemQuantity([FromBody] ChangeInput change)//receive JSON object from Cart.js when the number in the cart is changed
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.ProductId == change.ProductId);

            item.Quantity = int.Parse(change.Value);

            db.Cart.Update(item);
            db.SaveChanges();

            object data = new
            {
                status = "success"
            };

            return JsonSerializer.Serialize(data);
        }

        [HttpPost]
        public JsonResult DeleteCartItem([FromBody] Addinput product)
        {
            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.ProductId == product.ProductId);

            db.Cart.Remove(item);
            db.SaveChanges();

            return Json(new
            {
                status = "success"
            });
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
                        ProductId = int.Parse(item.ProductId),
                        PurchaseDate = DateTime.Today.Date,
                        ListingId = (item.ProductId + DateTime.Today.Date).GetHashCode()
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