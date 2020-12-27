using System;
using System.Collections.Generic;
using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;

namespace GDipSA51_Team5.Controllers
{
    public class SessionController : Controller
    {
        private readonly Team5_Db db;

        public SessionController(Team5_Db db)
        {
            this.db = db;
        }

        public IActionResult Login()
        {
            ViewData["Username"] = HttpContext.Request.Cookies["Username"] == null ? "Guest" : HttpContext.Request.Cookies["Username"];
            return View("Login");
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                ViewData["errMsg"] = "No such user or incorrect password";
                return View("Login");
            }

            Session session = new Session()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            db.Sessions.Add(session);
            db.SaveChanges();


            // the sequence of steps below matters. 
            AddNewItemsToCart(Environment.MachineName, session.UserId.ToString());
            Response.Cookies.Append("sessionId", session.Id);
            Response.Cookies.Append("userId", session.UserId.ToString());
            Response.Cookies.Append("Username", db.Users.FirstOrDefault(x => x.UserId == session.UserId).Username);

            return RedirectToAction("ListProducts", "Product");
        }

        private void AddNewItemsToCart(string userId, string session_UserId)  // session_UserId should be the real user ID converteid to from int to string
        {
            List<CartItem> cart = db.Cart.Where(x => x.UserId == userId).ToList();  // userId should be the DeviceName

            foreach (CartItem item in cart)
            {
                item.UserId = session_UserId;
                db.Update(item);
                db.SaveChanges();
            }
        }


        public IActionResult Logout()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);

            db.Sessions.Remove(session);
            HttpContext.Response.Cookies.Delete("sessionId");
            HttpContext.Response.Cookies.Delete("userId");
            HttpContext.Response.Cookies.Delete("Username");

            return RedirectToAction("ListProducts", "Product");
        }

    }
} 
