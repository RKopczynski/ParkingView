using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Web.Pages {

    public partial class FetchData
    {
        private WeatherForecast[] forecasts;
        
        [Inject] public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("https://localhost:5001/weatherforecast");
        }
    }
}