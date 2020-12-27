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
        private readonly string userId;
        private readonly string sessionId;

        public PurchaseHistoryController(Team5_Db db)
        {
            this.db = db;
            try { userId = HttpContext.Request.Cookies["userId"]; } catch (NullReferenceException) { userId = Environment.MachineName; }
            try { sessionId = HttpContext.Request.Cookies["sessionId"]; } catch (NullReferenceException) { sessionId = null; }
        }

        public IActionResult ListPurchaseHistory()
        {
            if (sessionId == null) return RedirectToAction("Login", "Login");

            List<Purchase> pastPurchases = db.PurchaseHistory.Where(x => x.UserId == int.Parse(userId)).ToList();

            ViewData["pastPurchases"] = pastPurchases;
            ViewData["sessionId"] = sessionId;

            return View("PurchaseHistory");
        }
    }
}
