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
        private readonly Session session;
        private readonly string sessionId;
        private readonly int userId;

        public PurchaseHistoryController(Team5_Db db)
        {
            this.db = db;
            sessionId = HttpContext.Request.Cookies["sessionId"];
            session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
            // if (session == null) { userId = session.UserId.ToString(); } else { userId = Environment.MachineName; }
            if (session != null) { userId = session.UserId; }
        }

        [Route("PurchaseHistory")]
        public IActionResult ListPurchaseHistory()
        {
            if (session == null) return RedirectToAction("Login", "Login");

            List<Purchase> pastPurchases = db.PurchaseHistory.Where(x => x.UserId == userId).ToList();

            ViewData["pastPurchases"] = pastPurchases;
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View();
        }
    }
}
