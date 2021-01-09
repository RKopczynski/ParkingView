using System.Collections.Generic;
using System.Linq;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly IEnumerable<Parking> _parkings;

        public ParkingRepository(CsvReader reader)
        {
            this._parkings = reader.GetParkingsFromFile();
        }

        public IEnumerable<Parking> GetAllParkings()
        {
            return this._parkings.ToList();
        }
    }
}