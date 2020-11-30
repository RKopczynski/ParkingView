using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafEla.ParkingView.Server.Data;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingRepository _repository;
        public ParkingController(IParkingRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllParkings();
                if (!results.Any())
                {
                    return NotFound("List of parkings is empty");
                }
                
                return Ok(results);
            }
            catch (ArgumentNullException)
            {
                return NotFound("None parking found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failed");
            }
        }
    }
}
