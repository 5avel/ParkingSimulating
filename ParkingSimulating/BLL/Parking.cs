using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public sealed class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get => lazy.Value; }

        private List<Car> cars = new List<Car>();
        private List<Transaction> transactions = new List<Transaction>();
        public decimal ParkingBalance { get; private set; }

        

        private Parking()
        {
            
        }

        /// <summary>
        /// Adds a unique car to the parking.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>false if car == null, parking is full or car is not unique</returns>
        public bool AddCar(Car car)
        {
            if (car == null) return false;

            if (cars.Count >= Settings.ParkingSpace) return false;

            if (cars.Count(x => x.LicensePlate == car.LicensePlate) > 0) return false;

            cars.Add(car);
            return true;
        }

        public bool DelCar(string carLicensePlate)
        {
            if (String.IsNullOrWhiteSpace(carLicensePlate)) return false;

            Car delCar = cars.FirstOrDefault<Car>(x => x.LicensePlate == carLicensePlate);
            if (delCar == null) return false;

            return cars.Remove(delCar);
        }


    }
}
