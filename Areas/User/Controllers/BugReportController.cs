using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BowlingApp.Data;
using BowlingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BowlingApp.Areas.User.Controllers
{
    [Area("User")]
    public class BugReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BugReport BugReport { get; set; }

        [TempData]
        public string Message { get; set; }

        public BugReportController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BugReport bugReport)
        {
            if(!ModelState.IsValid)
            {
                return View(bugReport);
            }
            _db.BugReport.Add(bugReport);
            await _db.SaveChangesAsync();
            Message = "Your report has been received, we will follow up with you shortly!";
            return RedirectToAction("Index", "Home");
        }
    }
}