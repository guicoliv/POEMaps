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
        public int nMaps { get; set; }
        public int nChaos { get; set; }

        public MapResult(Map m, int nMaps, int nChaos)
        {
            this.m = m;
            this.nMaps = nMaps;
            this.nChaos = nChaos;
        }
    }
}
