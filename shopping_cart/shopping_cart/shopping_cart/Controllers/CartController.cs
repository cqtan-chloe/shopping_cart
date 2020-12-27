using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using GDipSA51_Team5.Models;
using GDipSA51_Team5.Data;
using System;

namespace GDipSA51_Team5.Controllers
{
    public class CartController : Controller
    {
        private readonly Team5_Db db;

        public CartController(Team5_Db db)
        {
            this.db = db;
        }

        //receive JSON data from Add.js. (When an item is added to the cart from gallery)
        public JsonResult AddItemToCart([FromBody] Addinput product)
        {
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

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

            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();

            int total = 0;
            foreach (CartItem x in cart)
                total += x.Quantity;

            return Json(new
            {
                status = "success",
                total = total
            });
        }

        public IActionResult ListCartItems()//HttpGET on the cart() action
        {
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();
           
            ViewData["cart"] = cart;
            ViewData["sessioinId"] = sessionId;
            ViewData["Username"] = HttpContext.Request.Cookies["Username"] ?? "Guest";
            return View("Cart");
        }

        [HttpPost]
        public void ChangeCartItemQuantity([FromBody] ChangeInput change)//receive JSON object from Cart.js when the number in the cart is changed
        {
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.pId == change.ProductId);

            item.Quantity = int.Parse(change.Value);

            db.Cart.Update(item);
            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult DeleteCartItem([FromBody] Addinput product)
        {
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            CartItem item = db.Cart.FirstOrDefault(x => x.UserId == userId && x.pId == product.ProductId);

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
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            if (sessionId == null) return RedirectToAction("Login", "Session");

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
            return RedirectToAction("ListPurchaseHistory", "PurchaseHistory");
        }


    }
}