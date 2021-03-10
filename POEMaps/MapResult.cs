using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEMaps
{
    public class MapResult
    {
        public Map m { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string stash { get; set; }
        public int x { get; set; }
        public int y { get; set;}
        public int position { get; set; }

        public MapResult(Map m, string currency, int amount, string stash, int x, int y, int position)
        {
            this.m = m;
            this.currency = currency;
            this.amount = amount;
            this.stash = stash;
            this.x = x;
            this.y = y;
            this.position = position;
        }
    }
}
