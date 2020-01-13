using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApp.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 300)]
        public int Score { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string FrameIds { get; set; }

        [Required]
        public string GameString { get; set; }
        
        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Pattern { get; set; }  

        public string UserId { get; set; }
    }
}
