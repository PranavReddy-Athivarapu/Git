using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.DTO
{
    public class DiscountDTO
    {
        public int DiscountId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string DiscountCode { get; set; }

        [Required]
        public string DiscountType { get; set; }

        [Required]
        public decimal Value { get; set; }

        public bool IsPercentage { get; set; } = true;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
    }
}