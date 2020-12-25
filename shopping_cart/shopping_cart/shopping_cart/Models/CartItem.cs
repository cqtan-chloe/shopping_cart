using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }     // if user is not logged in, UserId is set to device name

        [Required]
        public string ProductId { get; set; }

        public int Quantity { get; set; }
        

        public virtual Product Product { get; set; }

    }

    public class Addinput
    {
        public bool AddIt { get; set; }
        public string ProductId { get; set; }

    }

    /*
    public class CartItems
    {
        public Dictionary<string, CartItem> map { get; set; }

        public CartItems()
        {
            map = new Dictionary<string, CartItem>();
        }
    }*/

    public class ChangeInput
    {
        public string ProductId { get; set; }
        public string Value { get; set; }
    }
}


