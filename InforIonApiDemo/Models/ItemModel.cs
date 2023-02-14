using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InforIonApiDemo.Models
{
    public class Items
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
        public string Item { get; set; }
        public string Description { get; set; }
        public string Revision { get; set; }
        public string Stat { get; set; }
        public string UM { get; set; }
        public string _ItemId { get; set; }
    }

    public class ItemsRoot
    {
        public List<Items> Items { get; set; }
    }
}