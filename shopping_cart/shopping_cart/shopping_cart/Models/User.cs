using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(15)]
        public string Password { get; set; }
    }
}
