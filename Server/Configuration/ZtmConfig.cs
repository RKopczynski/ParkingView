namespace RafEla.ParkingView.Server.Configuration
{
    public class ZtmConfig
    {
        private string _parknride;
        private string _buffer;

        public string Token { get; set; }
        
        public string BaseUrl { get; set; }

        public string Parknride
        {
            get => $"{BaseUrl}{_parknride}?token={Token}";
            set => _parknride = value;
        }
        
        public string Buffer
        {
            get => $"{BaseUrl}{_buffer}?token={Token}";
            set => _buffer = value;
        }
    }
}