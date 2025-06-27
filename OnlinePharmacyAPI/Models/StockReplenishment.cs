using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class StockReplenishment
    {
        [Key]
        public int ReplenishmentId { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        public int QuantityAdded { get; set; }

        [Required]
        public DateTime ReplenishmentDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("AdminUser")]
        public int AdminUserId { get; set; }

        // Navigation
        public virtual Medicine Medicine { get; set; }
        public virtual User AdminUser { get; set; }
    }
}
