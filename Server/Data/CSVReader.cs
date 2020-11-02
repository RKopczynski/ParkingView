using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace RafEla.ParkingView.Server.Data
{
    public class CSVReader
    {
        public void readFile()
        {
            List<(DateTime, int,string)> data= new List<(DateTime, int,string)>();
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
                            data.Add((DateTime.Parse(fields[0]), Int16.Parse(fields[1]), fields[4]));
                    }
                }
            }
            data = data.OrderByDescending(o => o.Item1).ToList();
        }
    }
}