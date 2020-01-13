using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BowlingApp.Areas.User.Controllers
{
    [Area("User")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserGuide()
        {
            return View();
        }

        public IActionResult Developer()
        {
            return View();
        }
    }
}