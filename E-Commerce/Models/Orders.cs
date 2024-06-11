using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("orders")]
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
