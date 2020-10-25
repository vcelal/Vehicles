using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vehicles.Models
{
    public class Manufacturers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2,
        ErrorMessage = "Manufacturer name must be between 2 to 20 characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public virtual ICollection<VehicleDetails> Vehicles { get; set; }


    }
}
