using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacyAppAPI.Models
{
    public class AlternativeMedicine
    {
        [Key]
        public int AlternativeId { get; set; }

        [ForeignKey("OriginalMedicine")]
        public int OriginalMedicineId { get; set; }

        [ForeignKey("SubstituteMedicine")]
        public int SubstituteMedicineId { get; set; }

        // Navigation
        public virtual Medicine OriginalMedicine { get; set; }
        public virtual Medicine SubstituteMedicine { get; set; }
    }
}
