using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;
using GDipSA51_Team5.Models;
using System;

namespace GDipSA51_Team5.Controllers
{
    public class ProductController : Controller
    {
        private readonly Team5_Db db;
        private readonly string userId;
        private readonly string sessionId;

        public ProductController(Team5_Db db)
        {
            this.db = db;
            try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }
        }

        public IActionResult ListProducts(string searchString)
        {
            ViewData["Products"] = db.Products.Where(s => (s.Name.Contains(searchString) || s.Description.Contains(searchString)) || searchString == null).ToList();

            ViewData["Username"] = userId == Environment.MachineName ? "Guest" : db.Users.FirstOrDefault(x => x.UserId == int.Parse(userId)).Username;
            ViewData["sessionId"] = sessionId;
            return View("Gallery");
        }
    }
}
