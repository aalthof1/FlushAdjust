using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApp.Models
{
    public class Ball
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ball Name")]
        public string Name { get; set; }

        [Display(Name = "Ball Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Serial Number")]
        public string Serial { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
