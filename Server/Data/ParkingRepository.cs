using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class ParkingRepository : IParkingRepository
    {
        private IEnumerable<Parking> _parkings;
        public IEnumerable<Parking> GetAllParkings()
        {
            var result = new List<Parking>()
            {
                new Parking {ParkingId = 1, City = "lilo", Postcode = "61-624", Street = "Soldiers", StreetNo = 23},
                new Parking {ParkingId = 2, City = "lol", Postcode = "61-624", Street = "Rahu", StreetNo = 56}
            };
            this._parkings = result;
            
            return this._parkings.ToList();
        }

        public Parking GetParking(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}