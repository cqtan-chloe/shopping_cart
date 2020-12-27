using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }     // if user is not logged in, UserId is set to device name

        [Required]
        public string pId { get; set; }

        public int Quantity { get; set; }

        public virtual Product product { get; set; }

    }

    public class Addinput
    {
        public string ProductId { get; set; }
    }

    public class ChangeInput
    {
        public string ProductId { get; set; }
        public string Value { get; set; }
    }
}


