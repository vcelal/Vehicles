using System;

namespace Vehicles.Models.ObjectModels
{
    public class VehicleDetailsModel
    {
        public int Id { get; set; }

        public string OwnerName { get; set; }

        public int ManufactureYear { get; set; }

        public string ManufacturerName { get; set; }

        public int ManufacturerId { get; set; }

        public double VehicleWeight { get; set; }

        public ManufacturerModel Manufacturer { get; set; }

        public WeightCategoryModel WeightCategory{ get; set; }

        public static explicit operator VehicleDetailsModel(VehicleDetails v)
        {
            return new VehicleDetailsModel()
            {
                Id = v.Id,
                OwnerName = v.OwnerName,
                ManufactureYear = v.ManufactureYear,
                VehicleWeight = v.VehicleWeight,
                ManufacturerName = v.ManufacturerDetails.Name,
                Manufacturer = (ManufacturerModel)v.ManufacturerDetails,
                WeightCategory = (WeightCategoryModel)v.Category

            };
        }
    }
}
