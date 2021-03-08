using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEMaps
{
    public class Map
    {
        public String name { get; set; }
        public String searchCode { get; set; }
        public String zone { get; set; }
        public int tier { get; set; }
        public int watchstones { get; set; }

        public Map(string name, string searchCode, string zone, int tier)
        {
            this.name = name;
            this.searchCode = searchCode;
            this.zone = zone;
            this.tier = tier;
            this.watchstones = 0;
        }


        public override string ToString()
        {
            return name+" ("+tier+") - ["+zone+" ("+watchstones+")]";
        }

    }
}
