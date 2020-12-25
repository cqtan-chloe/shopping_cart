using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class Product
    {

        public Product()
        {
            PurchaseHistory = new HashSet<Purchase>();
        }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Url { get; set; }

        public string GalleryTypeId { get; set; }

        public virtual ICollection<Purchase> PurchaseHistory { get; set; }

    }
}
