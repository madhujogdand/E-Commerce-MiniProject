using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    [Table("productcart")]
    public class ProductCart
    {
        [Key]
        public int ProductCartId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Users User { get; set; }
    }
}
