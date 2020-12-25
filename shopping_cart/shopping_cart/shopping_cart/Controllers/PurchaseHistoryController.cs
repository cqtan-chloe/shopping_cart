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
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId == null) return RedirectToAction("Login", "Login");

            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
            List<Purchase> pastPurchases = db.PurchaseHistory.Where(x => x.UserId == session.UserId).ToList();

            ViewData["pastPurchases"] = pastPurchases;
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View("PurchaseHistory");
        }
    }
}
