using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public sealed class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get => lazy.Value; }

        private List<Car> cars = new List<Car>();
        private object carsSyncRoot = new object();

        private List<Transaction> transactions = new List<Transaction>();

        private object transactionsSyncRoot = new object();

        public decimal ParkingBalance { get; private set; }

        private Timer calcTimer;
        private Timer logTimer;

        private Parking()
        {
            this.calcTimer = new Timer(new TimerCallback(PayCalc), null, Settings.Timeout, Settings.Timeout);
            this.logTimer = new Timer(new TimerCallback(WriteLogAndCleanTransactions), null, Settings.LogTimeout, Settings.LogTimeout);

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

            if (delCar.Balance < 0) return false;

            return cars.Remove(delCar);
        }

        private void PayCalc(object o)
        {
            foreach(Car car in cars)
            {
                decimal parkingPrice = Settings.ParkingPrice[car.CarType];
                decimal fine = Settings.Fine;
                decimal curPrice = 0;
                if (car.Balance > 0)
                {
                    if(car.Balance < parkingPrice)
                    {
                        decimal rest = car.Balance;
                        decimal negativeBalance = (parkingPrice - rest) * fine;
                        curPrice = (rest + negativeBalance);
                    }
                    else
                    {
                        curPrice = parkingPrice;
                    }
                }
                else
                {
                    curPrice = (parkingPrice * fine);
                }
                
                car.Balance -= curPrice;
                
                this.ParkingBalance += curPrice;
                // Add transaction
                lock (transactionsSyncRoot)
                {
                    this.transactions.Add(new Transaction(car.LicensePlate, curPrice));
                }
            }
        }

        public bool AddBalanceCar(string licensePlate, decimal money)
        {
            if (String.IsNullOrWhiteSpace(licensePlate)) return false;

            Car car = this.cars.FirstOrDefault(x => x.LicensePlate == licensePlate);
            if (car == null) return false;

            car.Balance += money;
            return true;
        }

        public decimal GetTotalParkingIncome() => this.ParkingBalance;

        public int CountFreeParkingPlaces() => Settings.ParkingSpace - this.cars.Count;

        public int CountOccupiedParkingPlaces() => this.cars.Count;

        public List<Transaction> AllTransaction() => this.transactions;

        private void WriteLogAndCleanTransactions(object o)
        {
            decimal sum = 0;

            lock (transactionsSyncRoot)
            {
                sum = transactions.Sum(t => t.Debited);
            }

            using (StreamWriter sw = new StreamWriter(Settings.LogPath, true))
            {
                sw.WriteLine("{0} - sum = {1:C2}", DateTime.Now, sum);
            }

            lock (transactionsSyncRoot)
            {
                transactions.Clear();
            }

        }

    }
}
