using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class Profile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        // Navigation
        public virtual User User { get; set; }
    }
}
