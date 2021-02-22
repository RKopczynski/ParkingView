using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using RafEla.ParkingView.Server.Configuration;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class CsvReader
    {        
        private readonly ZtmConfig _config;

        public static List<ParsedLine> _data;

        public CsvReader(IOptions<ZtmConfig> config)
        {
            _config = config.Value;
        }

        public List<Parking> GetParkingsFromFile()
        {
            List<Parking> parkings = new ();

            GetParkings();

            foreach (var v in _data)
            {
                if (parkings.All(p => p.Street != v.Street))
                {
                    parkings.Add(new Parking {
                        Street = v.Street,
                        ParkingPlaces = v.FreeParkingSpaces,
                        ParkingUrl = _getParkingURL(v.Street)
                    });
                }
            }
            return parkings;
        }           


        private void GetParkings()
        {
            var resultsPR = GetParkAndRideParking("P&R");
            var resultsBufor = GetParkAndRideParking("bufor");

            _data = resultsPR.Concat(resultsBufor).ToList();
            _data = _data.OrderByDescending(o => o.Date).ToList();
        }

        private List<ParsedLine> GetParkAndRideParking(string kind)
        {
            string url = "";

            if (kind == "P&R")
               url = _config.Parknride;
            else if (kind == "bufor")
                url = _config.Buffer; 

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();            

            return results.Parse();
        }

        private static string _getParkingURL(string street)
        {
            var destination = street switch
            {
                "Za Bramką" => "za+bramka+61-001+poznan",
                "Rondo Kaponiera" => "parking+rondo+kaponiera+roosevelta+60-829+poznan",
                "Maratońska" => "parking+zdm-maratonska+maratonska+61-553+poznan",
                "Głogowska - przy dw. Zachodnim" => "parking+glogowska+glogowska+17+60-701+poznan",
                "Szymanowskiego" => "parking+szymanowskiego+13+60-688+poznan",
                "Reymonta" => "chociszewskiego+54b+61-001+poznan",
                _ => null,
            };

            return $"https://www.google.com/maps/dir/?api=1&destination={destination}";
        }
    }
}