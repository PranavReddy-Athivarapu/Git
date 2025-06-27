using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }

        [Required, StringLength(50)]
        public string DiscountCode { get; set; } // e.g., "WELCOME10"

        [Required]
        public string DiscountType { get; set; } // "FirstOrder", "RegularUser"

        [Required]
        public decimal Value { get; set; } // e.g., 10 (%)

        [Required]
        public bool IsPercentage { get; set; } = true;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } // Null = no expiration
    }
}
