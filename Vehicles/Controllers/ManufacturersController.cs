using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Models;
using Vehicles.Models.ObjectModels;


namespace Vehicles.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private DatabaseContext _db;
        public ManufacturersController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets a list of manufacturers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var manufacturers = _db.Manufacturers.ToList();
            List<ManufacturerModel> castedManufacturers = new List<ManufacturerModel>();

            if (manufacturers.Count == 0)
            {
                return BadRequest("No manufacturer was found");
            }
            else 
            {
                foreach(var m in manufacturers)
                {
                    castedManufacturers.Add((ManufacturerModel)m);
                }
            }
           
                        
            return Ok(castedManufacturers);
        }
    }
}
