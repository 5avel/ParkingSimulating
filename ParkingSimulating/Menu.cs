using ParkingSimulating.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating
{
    public class Menu
    {
        public static void MainMenu(string msg = null)
        {
            Console.Clear();
            if (msg != null) Console.WriteLine(msg);
            Console.WriteLine("=====MainMenu=====");
            Console.WriteLine("1. Add car.");
            Console.WriteLine("2. Del car.");
            Console.WriteLine("3. ");
            Console.WriteLine("4. ");
            Console.WriteLine("5. Free places count.");

            switch (Console.ReadLine())
            {
                case "1":
                    AddCar();
                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "4":

                    break;
                case "5":
                    ShowFreePlacesCount();
                    break;
                default:
                    MainMenu();
                    break;

            }
        }

        private static void ShowFreePlacesCount()
        {
            Console.Clear();
            Console.WriteLine("{0} - places free.", Parking.Instance.CountFreeParkingPlaces());
            Console.WriteLine("Any kay to MainMenu");
            Console.ReadKey();
            MainMenu();
        }

        #region AddCar

        private static void AddCar(string msg = null)
        {
            if (msg != null) Console.WriteLine(msg);
            string carId = EnterCarId();
            CarType carType = CheseCarType();
            decimal diposit = EnterDiposit();

            if(Parking.Instance.AddCar(new Car(carId, carType, diposit)))
            {
                MainMenu(String.Format("Car added carId:{0}; carType:{1}; deposit:{2}.", carId, carType, diposit));
            }
            else
            {
                AddCar("error");
            }
        }

        private static string EnterCarId()
        {
            Console.Clear();
            Console.WriteLine("Enter car id (LicensePlate)");
            string carLicensePlate = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(carLicensePlate)) EnterCarId();

            return carLicensePlate;
        }

        private static CarType CheseCarType()
        {
            Console.Clear();
            Console.WriteLine("select car type:");
            Console.WriteLine("1. Passenger");
            Console.WriteLine("2. Truck");
            Console.WriteLine("3. Bus");
            Console.WriteLine("4. Motorcycle");

            switch (Console.ReadLine())
            {
                case "1":
                    return CarType.Passenger;
                case "2":
                    return CarType.Truck;
                case "3":
                    return CarType.Bus;
                case "4":
                    return CarType.Motorcycle;
                default :
                    return CheseCarType();
            }
        }

        private static decimal EnterDiposit(string msg = null)
        {
            Console.Clear();
            Console.WriteLine("Enter Diposit:");
            if (msg != null) Console.WriteLine(msg);
            decimal deposit;
            if(Decimal.TryParse(Console.ReadLine(), out deposit))
            {
                return deposit;
            }
            return EnterDiposit();
        }

        #endregion AddCar
    }
}
