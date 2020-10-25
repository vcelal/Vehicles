using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.Models;
using Vehicles.Models.ObjectModels;

namespace Vehicles.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeightCategoriesController : ControllerBase
    {
        private DatabaseContext _db;
        public WeightCategoriesController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets a list of weight categories
        /// </summary>
        /// <returns>Weight Categories Object</returns>
        [HttpGet]
        public ActionResult List()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var weightCategories = _db.WeightCategories.ToList();
            List<WeightCategoryModel> castedWeightCategories = new List<WeightCategoryModel>();

           
            if (weightCategories.Count > 0)
            {
                foreach (var w in weightCategories)
                {
                    castedWeightCategories.Add((WeightCategoryModel)w);
                }
                return Ok(castedWeightCategories.OrderBy(x => x.MinWeight));
            }
            return Ok(castedWeightCategories);
        }

        public async Task<ActionResult> Add(WeightCategories newWeightCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            if(newWeightCategory.MinWeight >= newWeightCategory.MaxWeight){
                return BadRequest("Minimum weight cannot be bigger than or equal to Maximum weight.");
            }
            var allCategories = _db.WeightCategories.ToList();
            var weightCategory = new WeightCategories();

            if(allCategories.Count == 0)
            {
                weightCategory = new WeightCategories()
                {
                    Name = newWeightCategory.Name,
                    MinWeight = newWeightCategory.MinWeight,
                    MaxWeight = newWeightCategory.MaxWeight,
                    IconId = newWeightCategory.IconId
                };
                _db.WeightCategories.Add(weightCategory);
            }
            else
            {
                var greatestValue = allCategories.Max(a => a.MaxWeight);
                var lowestValue = allCategories.Min(a => a.MinWeight);

                bool firstRow = newWeightCategory.MinWeight < lowestValue ? true : false;

                if(firstRow == true && newWeightCategory.MaxWeight != lowestValue)
                {
                    return BadRequest("Overlaps and gaps between categories are not allowed.");
                }
                else
                {
                    if (firstRow == false && newWeightCategory.MinWeight != greatestValue)
                    {
                        return BadRequest("Overlaps and gaps between categories are not allowed.");
                    }
                }

                weightCategory = new WeightCategories()
                {
                    Name = newWeightCategory.Name,
                    MinWeight = newWeightCategory.MinWeight,
                    MaxWeight = newWeightCategory.MaxWeight,
                    IconId = newWeightCategory.IconId
                };
                _db.WeightCategories.Add(weightCategory);
            }

            await _db.SaveChangesAsync();
            return Ok((WeightCategoryModel)weightCategory);
        }

        /// <summary>
        /// Edits a specific weight category
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(WeightCategories updatedWeight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            if(updatedWeight.MinWeight >= updatedWeight.MaxWeight)
            {
                return BadRequest("Minimum weight cannot be greater than or equal to maximum weight.");
            }

            var allCategories = _db.WeightCategories.ToList();

            if (allCategories.Count == 0)
            {
                return BadRequest("No weight categories were found");
            }

            var oldWeightCategory = allCategories.FirstOrDefault(o => o.Id == updatedWeight.Id);
            var otherCategories = allCategories.Where(o => o.Id != updatedWeight.Id).ToList();



            List<WeightCategories> combinedList = new List<WeightCategories>();

            combinedList.AddRange(otherCategories);
            combinedList.Add(updatedWeight);
            var checkList = combinedList.OrderBy(x => x.MinWeight).ToList();
            if (oldWeightCategory.MinWeight != updatedWeight.MinWeight || oldWeightCategory.MaxWeight != updatedWeight.MaxWeight)
            {
                for (var index = 0; index < checkList.Count; index++)
                {
                    if (checkList.Count - 1 != index && checkList[index].MaxWeight != checkList[index + 1].MinWeight)
                    {
                        return BadRequest("No gaps are allowed between different categories.");
                    }
                }
            }

            oldWeightCategory.Name = updatedWeight.Name;
            oldWeightCategory.IconId = updatedWeight.IconId;
            oldWeightCategory.MinWeight = updatedWeight.MinWeight;
            oldWeightCategory.MaxWeight = updatedWeight.MaxWeight;

            _db.WeightCategories.Update(oldWeightCategory);

            await UpdateVehicles();
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Removes a specific weight category. Important: removing a weight category may remove related vehicle information.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var requestedCategory = _db.WeightCategories.FirstOrDefault(r => r.Id == categoryId);

            if(requestedCategory == null)
            {
                return BadRequest("This item cannot be found");
            }

            var allCategories = _db.WeightCategories.ToList();
            var greatestValue = allCategories.Max(a => a.MaxWeight);
            var lowestValue = allCategories.Min(a => a.MaxWeight);

            // If this is the greatest weight or the lowest weight, there is no need to modify other categories because this will not create a gap between the ranges.
            // Remove it from the database and remove all related vehicles as well. 
            if (greatestValue == requestedCategory.MaxWeight || lowestValue == requestedCategory.MaxWeight)
            {
                _db.Remove(requestedCategory);
            }
            // If removing an item that is not the lowest or the highest weight, it will produce a gap between the other categories.
            // Assuming, we should not delete other categories, instead we will edit them by dropping the minimum weight of the next item.
            else
            {
                var nextCategory = _db.WeightCategories.FirstOrDefault(n => n.MinWeight == requestedCategory.MaxWeight);

                if(nextCategory == null)
                {
                    return BadRequest("Category cannot be found");
                }

                nextCategory.MinWeight = requestedCategory.MinWeight;
                
                _db.WeightCategories.Update(nextCategory);

                //var vehicleDetails = _db.VehicleDetails.Where(v => v.CategoryId == requestedCategory.Id).ToList();
                //if (vehicleDetails.Count > 0)
                //{
                //    _db.RemoveRange(vehicleDetails);
                //}
                _db.Remove(requestedCategory);
            }
            await UpdateVehicles();
            await _db.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// Updates the cetegory of the vehicles after adding or modifying a weight category
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        private async Task<ActionResult> UpdateVehicles()
        {
            var newWeightCategories = _db.WeightCategories.ToList();
            var vehicles = _db.VehicleDetails.ToList();
            var greatestValue = newWeightCategories.Max(a => a.MaxWeight);
            var lowestValue = newWeightCategories.Min(a => a.MaxWeight);

            if (newWeightCategories.Count > 0) {
                foreach (var c in newWeightCategories)
                {
                    if (vehicles.Count > 0)
                    {
                        foreach(var v in vehicles)
                        {
                            
                            if (c.MinWeight < v.VehicleWeight && c.MaxWeight >= v.VehicleWeight)
                            {
                                v.CategoryId = c.Id;
                                _db.VehicleDetails.Update(v);
                            }
                            else if(v.VehicleWeight < lowestValue || v.VehicleWeight > greatestValue)
                            {
                                _db.VehicleDetails.Remove(v);
                            }
                        }
                    }
                }
            }
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
