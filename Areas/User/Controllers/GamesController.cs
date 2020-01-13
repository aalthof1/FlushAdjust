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
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        [TempData]
        public string Message { get; set; }

        public GamesController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public GameSubmitView GameSubmitView { get; set; }

        //GET - Index
        public IActionResult Index()
        {
            var nullPatternGames = _db.Game.Where(g => g.Pattern == null).ToList();
            foreach (var game in nullPatternGames)
            {
                game.Pattern = "House";
                _db.Game.Update(game);
                _db.SaveChanges();
            }
            return View(_db.Game.Where(g => g.UserId == _userManager.GetUserId(User)));
        }

        //GET - Create
        public IActionResult Create()
        {
            if(GetBalls().Count() == 0)
            {
                Message = "Please add a ball before adding a game, found under Bowling -> Balls. If you do not own a bowling ball, just add a ball with name \"house ball\" and all other fields blank.";
                return RedirectToAction("Index");
            }
            ViewBag.Balls = GetBalls();
            return View();
        }

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameSubmitView GameInfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string pinsLeft = GameInfo.pinsLeft;
            string[] pins = pinsLeft.Split(";");
            string gameString = GameInfo.gameString;
            string[] frameIds = new string[10];
            for (int i = 0; i < 20; i += 2)
            {
                Frame f = new Frame();
                f.frameString = "" + gameString[i] + gameString[i + 1];
                if(pinsLeft.Length == 21 && i == 18)
                {
                    f.frameString += gameString[i + 1];
                }
                f.PinsLeft = pins[i / 2];
                if (f.PinsLeft.Length > 2)
                {
                    int[] pinsLeftTemp = (int[])Array.ConvertAll(f.PinsLeft.Split(","), int.Parse);
                    Array.Sort(pinsLeftTemp);
                    f.PinsLeft = String.Join(",", new List<int>(pinsLeftTemp).ConvertAll(x => x.ToString()).ToArray());
                }
                
                f.UserId = _userManager.GetUserId(User);
                f.BallId = Array.ConvertAll(GameInfo.BallIds.Split(","), int.Parse)[i/2];
                _db.Frame.Add(f);
                await _db.SaveChangesAsync();
                frameIds[i / 2] = "" + f.Id;
            }
            string frameIdString = "";
            for(int i = 0; i < frameIds.Length; i++)
            {
                frameIdString += (i == 0) ? frameIds[i] : "," + frameIds[i];
            }

            Game g = new Game();
            var patternTemp = GameInfo.Pattern;
            if (patternTemp == null || patternTemp == "")
            {
                patternTemp = "House";
            }
            g.Pattern = patternTemp;
            g.Score = GameInfo.Score;
            g.FrameIds = frameIdString;
            g.Date = DateTime.Now;
            g.GameString = gameString;
            g.Name = GameInfo.GameName;
            g.Notes = GameInfo.GameNotes;
            g.UserId = _userManager.GetUserId(User);

            _db.Game.Add(g);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        //helper method to populate balls list
        private IEnumerable<Ball> GetBalls()
        {
            return _db.Ball.Where(b => b.UserId == _userManager.GetUserId(User));
        }

        public Game Game { get; set; }

        //GET - Details
        public async Task<IActionResult> Details(int id)
        {
            Game game = await _db.Game.FindAsync(id);
            if(game == null || game.UserId != _userManager.GetUserId(User))
            {
                Message = "You are not Authorized to view that game!";
                return RedirectToAction("Index");
            }
            int[] frameIds = Array.ConvertAll(game.FrameIds.Split(","), int.Parse);
            GameDetailView model = new GameDetailView();
            model.Frames = GetFrames(frameIds);
            model.Game = game;
            string[] ballNamesByFrame = new string[10];
            for (int i = 0; i < model.Frames.Length; i++)
            {
                ballNamesByFrame[i] = _db.Ball.Find(model.Frames[i].BallId).Name;
            }
            ViewBag.BallNamesByFrame = ballNamesByFrame;
            return View(model);
        }

        public Frame[] GetFrames(int[] frameIds)
        {
            return _db.Frame.Where(f => frameIds.Contains(f.Id)).ToArray();
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int id)
        {
            Game game = await _db.Game.FindAsync(id);
            foreach (int frameId in Array.ConvertAll(game.FrameIds.Split(","), int.Parse))
            {
                Frame frameToRemove = await _db.Frame.FindAsync(frameId);
                _db.Frame.Remove(frameToRemove);
                await _db.SaveChangesAsync();
            }
            _db.Game.Remove(game);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }

}
