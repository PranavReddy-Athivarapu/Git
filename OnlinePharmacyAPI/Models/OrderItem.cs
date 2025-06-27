using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePharmacyAppAPI.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Navigation
        public virtual Order Order { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
