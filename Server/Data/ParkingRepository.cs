using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly IEnumerable<Parking> _parkings;
        public ParkingRepository()
        {
            this._parkings = CsvReader.GetParkingsFromFile();
        }
        public IEnumerable<Parking> GetAllParkings()
        {
            return this._parkings.ToList();
        }

    }
}