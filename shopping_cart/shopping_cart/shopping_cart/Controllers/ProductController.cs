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

        public ProductController(Team5_Db db)
        {
            this.db = db;
        }

        public IActionResult ListProducts(string searchString)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);

            if (sessionId != null)
                ViewData["Username"] = db.Users.FirstOrDefault(x => x.UserId == session.UserId).Username;
            else
                ViewData["Username"] = "Guest";

            ViewData["Products"] = db.Products.Where(s => (s.Name.Contains(searchString) || s.Description.Contains(searchString)) || searchString == null).ToList();
            ViewData["sessionId"] = sessionId;
            return View("Gallery");
        }
    }
}
