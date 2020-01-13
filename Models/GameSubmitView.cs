using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApp.Models
{
    public class GameSubmitView
    {
        public string pinsLeft { get; set; }

        public string gameString { get; set; }

        public int Score { get; set; }

        public string GameNotes { get; set; }

        public string GameName { get; set; }

        public string BallIds { get; set; }

        public string Pattern { get; set; }
    }
}
