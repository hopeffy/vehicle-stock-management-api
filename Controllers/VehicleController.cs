using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicle_stock_management_api.Models;
using vehicle_stock_management_api.Models.DTO;

namespace vehicle_stock_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public VehicleController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(CreateVehicleRequestDto request)
        {
            var eventObj = new Models.Domain.Vehicle
            {
                 plaka = request.plaka,
                modelYear = request.modelYear,
                muayeneTarihi = request.muayeneTarihi,
                path = request.path,
                isActive = request.isActive
            };

            await _appDbContext.Vehicle.AddAsync(eventObj);
            await _appDbContext.SaveChangesAsync();


            return Ok(eventObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = _appDbContext.Vehicle.ToList();

            return Ok(vehicles);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Domain.Vehicle>> getVehicle(String id)
        {
            Models.Domain.Vehicle vehicleById;
            if (id == null)
            {
                throw new KeyNotFoundException("Vehicle not found.");
            } else
            {
                Guid guidId = new Guid(id);
                vehicleById = await _appDbContext.Vehicle.FindAsync(guidId);

            }

            return Ok(vehicleById);
        }

        [NonAction]
        public Models.Domain.Vehicle GetVehicleById(Guid id)
        {
            var vehicleById = _appDbContext.Vehicle.Find(id);
            if (vehicleById == null)
            {
                throw new KeyNotFoundException("Vehicle Not Found");
            }
            return vehicleById;
        }

        [HttpPost("update/{UpdateId}")]
        public async Task<ActionResult<Models.Domain.Vehicle>> UpdateVehicle([FromBody] UpdateVehicleRequestDto request, Guid UpdateId)
        {
            if (UpdateId == null)
            {
                throw new KeyNotFoundException("Vehicle Not Found");
            }

            Models.Domain.Vehicle vehicle = await _appDbContext.Vehicle.FirstOrDefaultAsync(d => d.Id == UpdateId);

            if (vehicle.Id != null && vehicle.isActive == true)
            {
                vehicle.plaka = request.plaka;
                vehicle.modelYear = request.modelYear;
                vehicle.muayeneTarihi = request.muayeneTarihi;
                vehicle.path = request.path;
                vehicle.isActive = request.isActive;

            }
            await _appDbContext.SaveChangesAsync();

            return Ok(vehicle);
        }

        [HttpPost("delete/{DeleteId}")]
        public async Task<ActionResult<Models.Domain.Vehicle>> DeleteVehicle(Guid DeleteId)
        {
            if(DeleteId == null)
            {
                throw new KeyNotFoundException("Vehicle Not Found");
            }
            Models.Domain.Vehicle deleted = GetVehicleById(DeleteId);

            deleted.isActive = false;

            _appDbContext.SaveChanges();

            return Ok(deleted);
        }
    }
}
