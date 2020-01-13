using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BowlingApp.Data;
using BowlingApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BowlingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminUser)]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _um;
        private readonly IEmailSender _emailSender;

        public UsersController(ApplicationDbContext db, UserManager<IdentityUser> um, IEmailSender emailSender)
        {
            _db = db;
            _um = um;
            _emailSender = emailSender;
        }

        //GET - Index
        public async Task<IActionResult> Index()
        {
            return View(await _db.ApplicationUser.Where(u => u.Id != _um.GetUserId(User)).ToListAsync());
        }
    }
}