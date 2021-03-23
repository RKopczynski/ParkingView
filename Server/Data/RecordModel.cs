using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 
 namespace RafEla.ParkingView.Server.Data
{
    public record ParsedLine(DateTime Date, int FreeParkingSpaces, string Street);
}
