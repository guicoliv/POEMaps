using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace POEMaps
{
    public class PostResult
    {
        public Map m;
        public JToken item_ids { get; set; }
        public int ids_read;
        public string request_id { get; set; }
        public int nIds { get; set; }

        public PostResult(JToken item_ids, string id, Map m, int nIds)
        {
            this.nIds = nIds;
            this.item_ids = item_ids;
            this.request_id = id;
            this.m = m;
            this.ids_read = 0;
        }
    }
}
