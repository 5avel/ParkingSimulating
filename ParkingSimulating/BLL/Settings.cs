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
        public static Dictionary<CarType, int> ParkingPrice { get; private set; }
        public static int ParkingSpace { get; private set; }
        public static double Fine { get; private set; }

        static Settings()
        {
            Timeout = new TimeSpan(0, 0, 3000);

            ParkingPrice = new Dictionary<CarType, int>
            {
                { CarType.Truck,      5},
                { CarType.Passenger,  3},
                { CarType.Bus,        2},
                { CarType.Motorcycle, 1}
            };

            ParkingSpace = 10;

            Fine = 1.2;
        }
    }
}
