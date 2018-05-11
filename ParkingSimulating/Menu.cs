﻿using ParkingSimulating.BLL;
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
            Console.WriteLine("2. Del car by id.");
            Console.WriteLine("3. Replenish balance of the car.");
            Console.WriteLine("4. Display transaction history for the last minute.");
            Console.WriteLine("5. Show total income from parking.");
            Console.WriteLine("6. Show the amount of earnings in the last minute.");
            Console.WriteLine("7. Free places count.");
            Console.WriteLine("8. Show all cars in the parking.");

            switch (Console.ReadLine())
            {
                case "1":
                    AddCar();
                    break;
                case "2":
                    DelCarById();
                    break;
                case "3":

                    break;
                case "4":

                    break;
                case "5":

                    break;
                case "6":

                    break;
                case "7":
                    ShowFreePlacesCount();
                    break;
                case "8":
                    
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
            CarType carType = СhoiceCarType();
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

        private static CarType СhoiceCarType()
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
                    return СhoiceCarType();
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

        private static void DelCarById(string msg = null)
        {
            Console.Clear();
            if (msg != null) Console.WriteLine(msg);
            Console.WriteLine("To return to the main menu, enter \"#\" and press \"Enter\".");
            Console.WriteLine("Enter car id:");
            string carId = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(carId))
            {
                DelCarById("The number of the car can not be empty.Please try again.");
            }
            else
            {
                if (carId == "#") MainMenu();
                if(Parking.Instance.DelCar(carId))
                {
                    MainMenu(String.Format("The machine whis number \"{0}\" was successfully deleted.", carId));
                }
                else
                {
                    DelCarById(String.Format("The machine with the number {0} is not found. Please try again.", carId));
                }
            }
        }
    }
}
