using ParkingSimulating.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating
{
    class Program
    {
        static void Main(string[] args)
        {
            Parking.Instance.AddCar(new Car("1111", CarType.Passenger, 20));
            Parking.Instance.AddCar(new Car("1112", CarType.Passenger, 20));
            Parking.Instance.AddCar(new Car("1113", CarType.Passenger, 20));
            Parking.Instance.AddCar(new Car("1114", CarType.Passenger, 20));

            Console.ReadKey();
        }
    }
}
