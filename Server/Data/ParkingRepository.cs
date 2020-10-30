using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly IEnumerable<Parking> _parkings;
        public ParkingRepository()
        {
            this._parkings = new List<Parking>()
            {
                new Parking {ParkingId = 1, City = "Poznań", Postcode = "61-624", Street = "Soldiers", StreetNo = 23},
                new Parking {ParkingId = 2, City = "lol", Postcode = "61-624", Street = "Rahu", StreetNo = 56}
            };
        }
        public IEnumerable<Parking> GetAllParkings()
        {
            return this._parkings.ToList();
        }

        public Parking GetParking(int id)
        {
            return this._parkings.First(p => p.ParkingId == id);
        }
    }
}