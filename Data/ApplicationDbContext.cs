using System;
using System.Collections.Generic;
using System.Text;
using BowlingApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BowlingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Frame> Frame { get; set; }

        public DbSet<Game> Game { get; set; }

        public DbSet<Ball> Ball { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<BugReport> BugReport { get; set; }
    }
}
