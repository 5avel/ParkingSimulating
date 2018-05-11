using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public static class Settings
    {
        public static TimeSpan Timeout { get; private set; }
        public static TimeSpan LogTimeout { get; private set; }
        public static Dictionary<CarType, decimal> ParkingPrice { get; private set; }
        public static int ParkingSpace { get; private set; }
        public static decimal Fine { get; private set; }

        public static string LogPath { get; private set; }

        static Settings()
        {
            Timeout = new TimeSpan(0, 0, 3);
            LogTimeout = new TimeSpan(0, 1, 0);

            ParkingPrice = new Dictionary<CarType, decimal>
            {
                { CarType.Truck,      5m},
                { CarType.Passenger,  3m},
                { CarType.Bus,        2m},
                { CarType.Motorcycle, 1m}
            };

            ParkingSpace = 10;

            Fine = 1.2m;

            LogPath = AppDomain.CurrentDomain.BaseDirectory + "Transactions.log";
        }
    }
}
