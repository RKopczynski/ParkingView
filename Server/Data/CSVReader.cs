using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public static class CsvReader
    {
        private static List<(DateTime, int, string)> _data;
        private static string pathName;
        public static void ReadFile()
        {
            _data = new List<(DateTime, int,string)>();
            var header = "Czas_Rejestracji";

            if (File.Exists(pathName))
            {
                using (TextFieldParser csvReader = new TextFieldParser(pathName))
                {
                    csvReader.SetDelimiters(new string[] {";"});
                    
                    // Skip the row with the column names
                    csvReader.ReadLine();

                    while (!csvReader.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvReader.ReadFields();
                        if (fields[0] != header)
                            _data.Add((DateTime.Parse(fields[0]), Int16.Parse(fields[1]), fields[4]));
                    }
                }
            }
            _data = _data.OrderByDescending(o => o.Item1).ToList();
        }
        public static void GetParkingsToFile()
        {
            pathName = Directory.GetCurrentDirectory() + "/example_data.csv";
            var resultsPR = GetParkAndRideParkings("P&R");
            var resultsBufor = GetParkAndRideParkings("bufor");
    
            File.WriteAllText(pathName, resultsBufor + resultsPR);
        }

        private static string GetParkAndRideParkings(string kind)
        {
            string url = "";
            if (kind == "P&R")
               url  =
                "https://www.ztm.poznan.pl/pl/dla-deweloperow/getBuforParkingFile?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0Mi56dG0ucG96bmFuLnBsIiwiY29kZSI6MSwibG9naW4iOiJtaFRvcm8iLCJ0aW1lc3RhbXAiOjE1MTM5NDQ4MTJ9.ND6_VN06FZxRfgVylJghAoKp4zZv6_yZVBu_1-yahlo";
            else if (kind == "bufor")
                url =
                    "https://www.ztm.poznan.pl/pl/dla-deweloperow/getParkingFile?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0Mi56dG0ucG96bmFuLnBsIiwiY29kZSI6MSwibG9naW4iOiJtaFRvcm8iLCJ0aW1lc3RhbXAiOjE1MTM5NDQ4MTJ9.ND6_VN06FZxRfgVylJghAoKp4zZv6_yZVBu_1-yahlo"; 
           
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }

        public static List<Parking> GetParkingsFromFile()
        {
            GetParkingsToFile();
            ReadFile();
            List<Parking> parkings = new List<Parking>();

            foreach (var v in _data)
            {
                if (parkings.All(p => p.Street != v.Item3))
                {
                    parkings.Add(new Parking()
                        { Street = v.Item3, ParkingPlaces = v.Item2, ParkingUrl = GetParkingURL(v.Item3)});
                }
            }

            return parkings;
        }
        private static string GetParkingURL(string street)
        {
            return street switch
            {
                "Za Bramką" => "https://goo.gl/maps/GbWzZDQ5LTK2arYe8",
                "Rondo Kaponiera" => "https://goo.gl/maps/zAck7sgJv8ssudMk9",
                "Maratońska" => "https://goo.gl/maps/vBEMsygYjFJLSbSx8",
                "Głogowska - przy dw. Zachodnim" => "https://goo.gl/maps/fenHUjJaGwnYAeSb8",
                "Szymanowskiego" => "https://goo.gl/maps/Nrue8UKzPLHqToNm6",
                "Reymonta" => "https://goo.gl/maps/vBeh3AtB3yf6cYWq8",
                _ => null,
            };
        }
        
        
    }
}