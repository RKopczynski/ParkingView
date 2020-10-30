using System.Collections.Generic;
using System.Threading.Tasks;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public interface IParkingRepository
    {
        IEnumerable<Parking> GetAllParkings();

        Parking GetParking(int id);
    }
}