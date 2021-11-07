using System;
using System.Linq;
using System.Drawing;

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
        public string BackgroundColor
        {
            get
            {
                try
                {
                    return AppData.GetRockTypes().FirstOrDefault(p => p.ID == RockTypeID).BackgroundColor;
                }
                catch
                {
                    return "#000000";
                }
            }
        }

        public Color BackColor => Color.FromArgb(int.Parse(BackgroundColor.Replace("#", ""), System.Globalization.NumberStyles.HexNumber));

        /// <summary>
        /// Запомнить!!! определяет, темный цвет или светлый
        /// </summary>
        public string TextColor => (0.229 * Convert.ToInt32(BackColor.R)) + (0.587 * Convert.ToInt32(BackColor.G)) + (0.114 * Convert.ToInt32(BackColor.B)) > 127.5
                    ? "Black"
                    : "White";

        public string RockName
        {
            get
            {
                try
                {
                    return AppData.GetRockTypes().FirstOrDefault(p => p.ID == RockTypeID).Name;
                }
                catch
                {
                    return "Oil/Gas";
                }
            }
        }
        public string StartNotZeroPoint => StartPoint != 0 ? "" : StartPoint.ToString();
    }
}
