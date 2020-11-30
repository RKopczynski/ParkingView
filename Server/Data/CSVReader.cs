using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using RafEla.ParkingView.Shared;

namespace RafEla.ParkingView.Server.Data
{
    public static class CsvReader
    {
        private static List<(DateTime, int, string)> _data;
        public static void ReadFile()
        {
            _data = new List<(DateTime, int,string)>();
            var filename = Directory.GetCurrentDirectory() + "/example_data.csv";
            var header = "Czas_Rejestracji";

            if (File.Exists(filename))
            {
                using (TextFieldParser csvReader = new TextFieldParser(filename))
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
    
        public static List<Parking> GetParkingsFromFile()
        {
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
            if (street == "Bramką")
                return "https://goo.gl/maps/GbWzZDQ5LTK2arYe8";
            if (street == "Rondo Kaponiera")
                return "https://goo.gl/maps/zAck7sgJv8ssudMk9";
            if (street == "Maratońska")
                return "https://goo.gl/maps/vBEMsygYjFJLSbSx8";
            if (street == "Głogowska - przy dw. Zachodnim")
                return "https://goo.gl/maps/fenHUjJaGwnYAeSb8";
            if (street == "Reymonta")
                return "test";

            return null;
        }
        

    }
}