using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;
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
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            ViewData["Products"] = db.Products.Where(s => (s.Name.Contains(searchString) || s.Description.Contains(searchString)) || searchString == null).ToList();

            ViewData["Username"] = HttpContext.Request.Cookies["Username"] == null ? "Guest" : HttpContext.Request.Cookies["Username"];
            ViewData["sessionId"] = sessionId;
            return View("Gallery");
        }
    }
}
