using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }


        [Required]
        public int Stock { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string Image { get; set; }
        [NotMapped]
        public string? Categoryname { get; set; }
    }
}
