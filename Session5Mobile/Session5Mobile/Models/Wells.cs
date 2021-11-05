using System;
using System.Collections.Generic;
using System.Text;

namespace Session5Mobile.Models
{
    public class Wells
    {
        public int ID { get; set; }
        public int WellTypeID { get; set; }
        public string WellName { get; set; }
        public int GasOilDepth { get; set; }
        public int Capacity { get; set; }
    }
}
