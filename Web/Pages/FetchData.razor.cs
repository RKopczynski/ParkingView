using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Web.Pages {

    public partial class FetchData
    {
        private Parking[] parkings;
        
        [Inject] public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            parkings = await Http.GetFromJsonAsync<Parking[]>("https://localhost:5001/api/Parking");
        }
    }
}