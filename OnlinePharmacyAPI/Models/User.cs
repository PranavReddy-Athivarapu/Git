using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "Customer"

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StockReplenishment> Replenishments { get; set; }
    }
}
