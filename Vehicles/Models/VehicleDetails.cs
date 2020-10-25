using System;
using System.ComponentModel.DataAnnotations;

namespace Vehicles.Models
{
    public class VehicleDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4,
        ErrorMessage = "Name must be between 4 to 30 characters")]
        [DataType(DataType.Text)]
        public string OwnerName { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(1880, 2100, ErrorMessage = "Please enter valid year")]
        public int ManufactureYear { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid weight")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Weight cannot have more than 2 decimal points")]
        public double VehicleWeight { get; set; }

        public virtual Manufacturers ManufacturerDetails { get; set; }

        public virtual WeightCategories Category { get; set; }


    }
}
