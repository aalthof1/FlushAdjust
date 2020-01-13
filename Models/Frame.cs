using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApp.Models
{
    public class Frame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PinsLeft { get; set; }

        [Required]
        public string frameString { get; set; }

        [Required]
        public string UserId { get; set; }

        public int BallId { get; set; }

    }
}
