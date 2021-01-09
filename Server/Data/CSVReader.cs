using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using RafEla.ParkingView.Server.Configuration;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public class CsvReader
    {
        record ParsedLine(DateTime Date, int FreeParkingSpaces, string Street);
        private readonly string _pathName;
        private readonly ZtmConfig _config;

        private static List<ParsedLine> _data;

        public CsvReader(IOptions<ZtmConfig> config)
        {
            _config = config.Value;
            _pathName = Directory.GetCurrentDirectory() + "/example_data.csv";
        }

        public List<Parking> GetParkingsFromFile()
        {
            GetParkingsToFile();
            ReadFile();
            List<Parking> parkings = new ();

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

        private void ReadFile()
        {
            _data = new List<ParsedLine>();
            var header = "Czas_Rejestracji";

            if (File.Exists(_pathName))
            {
                using var csvReader = new TextFieldParser(_pathName);
                csvReader.SetDelimiters(new string[] {";"});
                
                // Skip the row with the column names
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvReader.ReadFields();
                    if (fields[0] != header)
                    {
                        _data.Add(new (DateTime.Parse(fields[0]), int.Parse(fields[1]), fields[4]));
                    }
                }
            }

            _data = _data.OrderByDescending(o => o.Date).ToList();
        }

        private void GetParkingsToFile()
        {
            var resultsPR = GetParkAndRideParkings("P&R");
            var resultsBufor = GetParkAndRideParkings("bufor");
    
            File.WriteAllText(_pathName, resultsBufor + resultsPR);
        }

        private string GetParkAndRideParkings(string kind)
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

            return results;
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