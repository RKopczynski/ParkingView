using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RafEla.ParkingView.Server.Data
{
    public static class ListExtensions
    {
        public static List<ParsedLine> Parse(this String str)
        {
            var data = new List<ParsedLine>();
            var items = str.Split("\n");
            var header = "Czas_Rejestracji";
            String [] delimiters = {";", "\""};
            foreach (var item in items)
            {
                var fields = item.Split(delimiters, StringSplitOptions.None);
                if (fields[0] != header && fields[0] != "")
                    {
                        data.Add(new (DateTime.Parse(fields[0]), int.Parse(fields[1]), fields[4]));
                    }
            }
            return data;
        }

    }
}
