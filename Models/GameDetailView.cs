using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApp.Models
{
    public class GameDetailView
    {
        public Game Game { get; set; }

        public Frame[] Frames { get; set; } 
    }
}
