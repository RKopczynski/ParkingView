using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RafEla.ParkingView.Shared;
using System.Timers;

namespace RafEla.ParkingView.Web.Pages {

    public partial class Index
    {
        [Inject] public HttpClient Http { get; set; }
        
        private Parking[] parkings;
        private Timer aTimer;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender){
                aTimer = new Timer();
                aTimer.Elapsed += new ElapsedEventHandler(TimerTick);
                aTimer.Interval = 10000;
                aTimer.Enabled = true;
                aTimer.Start();
                await GetParkings();
            }            
        }       

        public void TimerTick(object sender, EventArgs e) => _ = GetParkings();

        private async Task GetParkings()
        {
            parkings = await Http.GetFromJsonAsync<Parking[]>("https://localhost:5001/api/Parking");
            StateHasChanged();
        }
    }
}