    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vehicles.Models
{
    public class WeightCategories
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2,
        ErrorMessage = "Weight Category name must be between 2 to 20 characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        public double MinWeight { get; set; }

        [Required]
        public double MaxWeight { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Invalid range.")]
        public int IconId { get; set; }

        public virtual ICollection<VehicleDetails> Vehicles { get; set; }

    }
}
