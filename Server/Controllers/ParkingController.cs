using System;
using System.Collections;
using System.Collections.Generic;
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
        public IEnumerable<Parking> Get()
        {
            var results = _repository.GetAllParkings();
            return results;
        }
        [HttpGet("{id}")]
        public Parking Get(int id)
        {
            var results = _repository.GetParking(id);
            return results;
        }    
    }
}
