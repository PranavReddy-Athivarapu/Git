using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public int MinimumStockThreshold { get; set; } = 10;

        // Navigation
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<AlternativeMedicine> Alternatives { get; set; }
        public virtual ICollection<StockReplenishment> Replenishments { get; set; }
    }
}
