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
    public class VehiclesController : ControllerBase
    {
        private DatabaseContext _db;
        public VehiclesController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds vehicle details to the Vehicles table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddVehicle(VehicleDetails vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var manufacturer = _db.Manufacturers.FirstOrDefault(m => m.Id == vehicle.ManufacturerId);

            if (manufacturer == null)
            {
                return BadRequest("Manufacturer cannot be found");
            }

            var weightCategory = _db.WeightCategories.Where(w => w.MinWeight < vehicle.VehicleWeight && w.MaxWeight >= vehicle.VehicleWeight).FirstOrDefault();

            if(weightCategory == null)
            {
                return BadRequest("Weight category cannot be found. Please create one.");
            }

            var allocatedWeightCategory = weightCategory.Id;

            VehicleDetails vehicleDetails = new VehicleDetails()
            {
                OwnerName = vehicle.OwnerName,
                ManufacturerId = vehicle.ManufacturerId,
                ManufactureYear = vehicle.ManufactureYear,
                VehicleWeight = vehicle.VehicleWeight,
                CategoryId = allocatedWeightCategory
            };

            _db.VehicleDetails.Add(vehicleDetails);
            await _db.SaveChangesAsync();

            return Ok((VehicleDetailsModel)vehicleDetails);
        }

        /// <summary>
        /// Removes a vehicle entry from Vehicles table
        /// </summary>
        /// <param name="vehicleId">Id of the vehicle</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> RemoveVehicle(int vehicleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var vehicle = _db.VehicleDetails.FirstOrDefault(v => v.Id == vehicleId);

            if(vehicle == null)
            {
                return BadRequest("Vehicle details cannot be found");
            }

            _db.VehicleDetails.Remove(vehicle);
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Edits a vehicle entry in Vehicles table
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> EditVehicle(VehicleDetails vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var vehicleDetails = _db.VehicleDetails.Where(vd => vd.Id == vehicle.Id).FirstOrDefault();

                vehicleDetails.OwnerName = vehicle.OwnerName;
                vehicleDetails.VehicleWeight = vehicle.VehicleWeight;
                vehicleDetails.ManufacturerId = vehicle.ManufacturerId;
                vehicleDetails.ManufactureYear = vehicle.ManufactureYear;

                var weightCategory = _db.WeightCategories.Where(w => w.MinWeight < vehicle.VehicleWeight && w.MaxWeight >= vehicle.VehicleWeight).FirstOrDefault();

                if (weightCategory == null)
                {
                    return BadRequest("Weight category cannot be found.");
                }

                var allocatedWeightCategory = weightCategory.Id;
                vehicleDetails.CategoryId = allocatedWeightCategory;

                _db.VehicleDetails.Update(vehicleDetails);
                await _db.SaveChangesAsync();
            }
            catch (System.Exception)
            {

                return BadRequest("Something went wrong");
            }
            

            return Ok();
        }

        /// <summary>
        /// Gets a list of vehicle details
        /// </summary>
        /// <returns>All Vehicle Details</returns>
        [HttpGet]
        public ActionResult VehicleList()
        {
            var vehicles = _db.VehicleDetails.ToList();
            
            List<VehicleDetailsModel> vehicleList = new List<VehicleDetailsModel>();

            if(vehicles.Count > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    var castedVehicle = (VehicleDetailsModel)vehicle;

                    vehicleList.Add(castedVehicle);
                }
            }

            return Ok(vehicleList);
        }

    }
}
