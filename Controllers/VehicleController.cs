using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using vehicle_stock_management_api.Models;
using vehicle_stock_management_api.Models.Domain;
using vehicle_stock_management_api.Models.DTO;

namespace vehicle_stock_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehicleController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
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
                isActive = true
            };

            await _appDbContext.Vehicle.AddAsync(eventObj);
            await _appDbContext.SaveChangesAsync();


            return Ok(eventObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = _appDbContext.Vehicle.ToList();

            if(vehicles != null && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle => {
                    vehicle.path = GetImagebyVehicle(vehicle.plaka);
                });
            } 

            return Ok(vehicles);

        }


        [HttpGet]
        [Route("getActiveVehicles")]
        public async Task<IActionResult> GetAllActiveVehicles()
        {
            
            var vehicles = _appDbContext.Vehicle
                .Where(p => p.isActive == true)
                .ToList();

            if (vehicles != null && vehicles.Count > 0)
            {
                vehicles.ForEach(vehicle => {
                    vehicle.path = GetImagebyVehicle(vehicle.plaka);
                });
            }

            return Ok(vehicles);

        }

        [HttpPost("{id}")]
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
        public Models.Domain.Vehicle GetVehicleById(string id)
        {
            Guid guidId = new Guid(id);
            var vehicleById = _appDbContext.Vehicle.Find(guidId);
            if (vehicleById == null)
            {
                throw new KeyNotFoundException("Vehicle Not Found");
            }
            return vehicleById;
        }

        [HttpPost()]
        [Route("update")]
        public async Task<ActionResult<Models.Domain.Vehicle>> UpdateVehicle(UpdateVehicleRequestDto request)
        {
           // Models.Domain.Vehicle vh = GetVehicleById(request.id);

            Models.Domain.Vehicle vehicle = await _appDbContext.Vehicle.FirstOrDefaultAsync(d => d.Id == request.id);

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
        public async Task<ActionResult<Models.Domain.Vehicle>> DeleteVehicle(string DeleteId)
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

        

        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {

            bool Results = false;
            try
            {
                var _uploadFiles = Request.Form.Files;

                foreach (IFormFile file in _uploadFiles)
                {
                    string Filename = file.FileName;
                    string FilePath = GetFilePath(Filename); 

                    if(!System.IO.Directory.Exists(FilePath))
                    {
                        System.IO.Directory.CreateDirectory(FilePath);
                    }

                    string imagePath = FilePath + "\\image.png";

                    if (System.IO.Directory.Exists(imagePath)) 
                    {
                        System.IO.Directory.Delete(imagePath);
                    }
                    using(FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        Results = true;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
            return Ok(Results);
        }

        [NonAction]
        private string GetFilePath(string plakaNo)
        {
            return this._webHostEnvironment.WebRootPath + "\\Uploads\\Product\\" + plakaNo;
        }

        [NonAction]
        private string GetImagebyVehicle(string plakaNo)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "http://localhost:5145";
            string filePath = GetFilePath(plakaNo);
            string ImagePath = filePath + "\\image.png";

            if(!System.IO.File.Exists(ImagePath))
            {
                ImageUrl = HostUrl + "/Uploads/common/noimage.png";

            } else
            {
                ImageUrl = HostUrl + "/Uploads/Product/" + plakaNo + "/image.png";
            }
            return ImageUrl;
        }

    }
}
