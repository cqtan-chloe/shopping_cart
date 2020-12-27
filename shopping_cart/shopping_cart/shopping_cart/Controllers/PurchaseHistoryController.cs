using System.Collections.Generic;
using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;
using System;

namespace GDipSA51_Team5.Controllers
{
    public class PurchaseHistoryController : Controller
    {
        private readonly Team5_Db db;

        public PurchaseHistoryController(Team5_Db db)
        {
            this.db = db;
        }

        public IActionResult ListPurchaseHistory()
        {
            string sessionId; try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
            string userId; if (sessionId != null) { userId = HttpContext.Request.Cookies["userId"]; } else { userId = Environment.MachineName; }

            if (sessionId == null) return RedirectToAction("Login", "Session");

            List<Purchase> pastPurchases = db.PurchaseHistory.Where(x => x.UserId == int.Parse(userId)).ToList();

            ViewData["pastPurchases"] = pastPurchases;
            ViewData["sessionId"] = sessionId;
            ViewData["Username"] = HttpContext.Request.Cookies["Username"] == null ? "Guest" : HttpContext.Request.Cookies["Username"];
            return View("PurchaseHistory");
        }
    }
}
