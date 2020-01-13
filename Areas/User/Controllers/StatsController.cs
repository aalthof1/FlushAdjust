using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BowlingApp.Data;
using BowlingApp.Models;
using BowlingApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BowlingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.User + "," + SD.AdminUser)]
    [Area("User")]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public StatsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Game> games = _db.Game.Where(g => g.UserId == _userManager.GetUserId(User)).ToList();
            List<Frame> frames = _db.Frame.Where(f => f.UserId == _userManager.GetUserId(User)).ToList();
            if (games.Count() > 0)
            {
                //get stats basic
                double strikePct = (double)frames.Where(f => f.frameString[0] == 'X').Count() / (double)frames.Count();
                strikePct *= 100;
                string strikePctFormatted = String.Format("{0:0.##}", strikePct);

                double sparePct = (double)frames.Where(f => f.frameString[0] != 'X' && f.frameString[1] == '/').Count() / (double)frames.Where(f => f.frameString[0] != 'X').Count();
                sparePct *= 100;
                string sparePctFormatted = String.Format("{0:0.##}", sparePct);

                double filledPct = (double)frames.Where(f => f.frameString[0] == 'X' || f.frameString[1] == '/').Count() / frames.Count();
                filledPct *= 100;
                string filledPctFormatted = String.Format("{0:0.##}", filledPct);

                int avg = (int)games.Average(g => g.Score);

                int mostFrequentlyLeftSinglePin = int.Parse(
                    frames
                    .Where(f => f.PinsLeft.Length == 1 || f.PinsLeft.Length == 2)
                    .Select(f => f.PinsLeft)
                    .GroupBy(g => g)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault());

                string mostFrequentSpareLeft =
                    frames
                    .Where(f => f.PinsLeft != "")
                    .Select(f => f.PinsLeft)
                    .GroupBy(g => g)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault();

                ViewBag.date1 = games.Select(g => g.Date).Distinct().OrderBy(d => d.Date).FirstOrDefault();
                ViewBag.date2 = games.Select(g => g.Date).Distinct().OrderByDescending(d => d.Date).FirstOrDefault();
                ViewBag.patterns = games.Select(g => g.Name);
                ViewBag.games = games;
                ViewBag.frames = frames;
                ViewBag.balls = _db.Ball.Where(b => b.UserId == _userManager.GetUserId(User)).ToList();
                ViewBag.strikePercent = strikePctFormatted;
                ViewBag.sparePercent = sparePctFormatted;
                ViewBag.filledPercent = filledPctFormatted;
                ViewBag.Average = avg;
                ViewBag.mostFrequent = mostFrequentlyLeftSinglePin;
                ViewBag.mostFrequentSpare = mostFrequentSpareLeft;
                ViewBag.display = true;
            }
            else
            {
                ViewBag.display = false;
            }
            return View();
        }
    }
}