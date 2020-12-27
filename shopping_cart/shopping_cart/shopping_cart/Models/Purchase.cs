using System;
using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class Purchase
    {
        [Required]
        public int SerialNo { get; set; }

        [MaxLength(20)]
        [Required]
        public string ActivationCode { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int ListingId { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }

}
