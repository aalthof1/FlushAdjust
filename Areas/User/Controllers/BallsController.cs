using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BowlingApp.Data;
using BowlingApp.Models;
using BowlingApp.Service;
using BowlingApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BowlingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.User + "," + SD.AdminUser)]
    [Area("User")]
    public class BallsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        [TempData]
        public string Message { get; set; }

        public Ball Ball { get; set; }

        public BallsController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        //GET - Index
        public IActionResult Index()
        {
            return View(_db.Ball.Where(b => b.UserId == _userManager.GetUserId(User)));
        }

        //GET - Create
        public IActionResult Create()
        {
            return View();
        }

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ball ball)
        {
            if(ModelState.IsValid)
            {
                ball.UserId = _userManager.GetUserId(User);
                _db.Ball.Add(ball);
                await _db.SaveChangesAsync();
                Message = "Ball created succesfully";
                await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault().Email, "Ball Added - " + ball.Name, $"The ball {ball.Name} was added to your ball list.");
                return RedirectToAction("Index");
            }
            return View(ball);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int id)
        {
            Ball ball = await _db.Ball.FindAsync(id);
            _db.Ball.Remove(ball);
            await _db.SaveChangesAsync();
            Message = "Ball Deleted Successfully";
            return RedirectToAction("Index");
        }

        //GET - Details
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Ball ball = await _db.Ball.FindAsync(id);
            if (ball == null || ball.UserId != _userManager.GetUserId(User))
            {
                Message = "You are not Authorized to view that game!";
                return RedirectToAction("Index");
            }
            return View(ball);
        }

    }
}