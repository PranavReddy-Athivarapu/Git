using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePharmacyAppAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        public string Status { get; set; } = "Placed"; // "Placed", "Cancelled", "Completed"

        public bool IsFirstOrder { get; set; } = true;

        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
