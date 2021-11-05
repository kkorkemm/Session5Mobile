using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Session5Mobile
{
    using Models;

    /// <summary>
    /// Работа с API
    /// </summary>
    public class AppData
    {
        private static string addressPhone = "http://192.168.0.108:55075/api/";
        private static string addressEmulator = "http://10.0.2.2:55075/api/";

        /// <summary>
        /// Проверка устройства (эмулятор или телефон)
        /// </summary>
        /// <returns> Нужный адрес API </returns>
        public static string CheckDevice()
        {
            string address;
            try
            {
                string responce = new WebClient().DownloadString(addressPhone);
                address = addressPhone;
            }
            catch
            {
                address = addressEmulator;
            }
            return address;
        }

        /// <summary>
        /// GET: api/RockTypes
        /// </summary>
        /// <returns> Список типов rock </returns>
        public static List<RockTypes> GetRockTypes()
        {
            string address = CheckDevice();
            string responce = new WebClient().DownloadString($"{address}RockTypes");
            return JsonConvert.DeserializeObject<List<RockTypes>>(responce);
        }

        /// <summary>
        /// GET: api/WellLayers
        /// </summary>
        /// <returns> Список слоев лунки </returns>
        public static List<WellLayers> GetWellLayers()
        {
            string address = CheckDevice();
            string responce = new WebClient().DownloadString($"{address}WellLayers");
            return JsonConvert.DeserializeObject<List<WellLayers>>(responce);
        }

        /// <summary>
        /// GET: api/Wells
        /// </summary>
        /// <returns> Список лунок </returns>
        public static List<Wells> GetWells()
        {
            string address = CheckDevice();
            string responce = new WebClient().DownloadString($"{address}Wells");
            return JsonConvert.DeserializeObject<List<Wells>>(responce);
        }

        /// <summary>
        /// GET: api/WellTypes
        /// </summary>
        /// <returns> Список типов лунок </returns>
        public static List<WellTypes> GetWellTypes()
        {
            string address = CheckDevice();
            string responce = new WebClient().DownloadString($"{address}WellTypes");
            return JsonConvert.DeserializeObject<List<WellTypes>>(responce);
        }
    }
}
