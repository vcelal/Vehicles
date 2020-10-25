using System;

namespace Vehicles.Models.ObjectModels
{
    public class ManufacturerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static explicit operator ManufacturerModel(Manufacturers m)
        {
            return new ManufacturerModel()
            {
              Id = m.Id,
              Name = m.Name
            };
        }
    }
}
