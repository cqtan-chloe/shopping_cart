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
        private readonly Session session;
        private readonly string sessionId;

        public ProductController(Team5_Db db)
        {
            this.db = db;
            sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId != null) session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
        }

        public IActionResult ListProducts(string searchString)
        {
            if (sessionId != null)
                ViewData["Username"] = db.Users.FirstOrDefault(x => x.UserId == session.UserId).Username;
            else
                ViewData["Username"] = "Guest";

            ViewData["Products"] = db.Products.Where(s => (s.Name.Contains(searchString) || s.Description.Contains(searchString)) || searchString == null).ToList();
            ViewData["sessionId"] = sessionId;
            return View("~/Views/Gallery.cshtml");
        }
    }
}
