using System;

namespace Vehicles.Models.ObjectModels
{
    public class WeightCategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double MinWeight { get; set; }

        public double MaxWeight { get; set; }

        public int IconId { get; set; }

        public static explicit operator WeightCategoryModel(WeightCategories w)
        {
            return new WeightCategoryModel()
            {
                Id = w.Id,
                Name = w.Name,
                MinWeight = w.MinWeight,
                MaxWeight = w.MaxWeight,
                IconId = w.IconId

            };
        }
    }
}
