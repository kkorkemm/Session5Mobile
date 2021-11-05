using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Session5Mobile.Models
{
    public class WellLayers
    {
        public int ID { get; set; }
        public int WellID { get; set; }
        public int RockTypeID { get; set; }
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }

        public double Height => (EndPoint - StartPoint) / 10;
        public string Color => AppData.GetRockTypes().FirstOrDefault(p => p.ID == RockTypeID).BackgroundColor;
        public string RockName => AppData.GetRockTypes().FirstOrDefault(p => p.ID == RockTypeID).Name;

        public string StartNotZeroPoint => StartPoint != 0 ? "" : StartPoint.ToString();
    }
}
