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

        private object parkingBalanceSyncRoot = new object();
        private decimal parkingBalance;
        public decimal ParkingBalance
        {
            get
            {
                lock (parkingBalanceSyncRoot)
                {
                    return parkingBalance;
                }
            }
            private set
            {
                lock (parkingBalanceSyncRoot)
                {
                    parkingBalance = value;
                }
            }
        }


        private Timer calcTimer;
        private Timer logTimer;

        private Parking()
        {
            this.calcTimer = new Timer(new TimerCallback(PayCalc), null, Settings.Timeout, Settings.Timeout);
            this.logTimer = new Timer(new TimerCallback(WriteLogAndCleanTransactions), null, Settings.LogTimeout, Settings.LogTimeout);

        }

        public IList<T> CloneList<T>(IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
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

        /// <summary>
        /// Removing car from parking
        /// </summary>
        /// <param name="carLicensePlate">License Plate or Id</param>
        /// <returns> 1 - car successfully deleted; 0 - car not deleted; -1 - carLicensePlate IsNullOrWhiteSpace; -2 - ar not found; -3 - The Car has a negative balance</returns>
        public int DelCar(string carLicensePlate)
        {
            if (String.IsNullOrWhiteSpace(carLicensePlate)) return -1;

            Car delCar = cars.FirstOrDefault<Car>(x => x.LicensePlate == carLicensePlate);
            if (delCar == null) return -2;

            if (delCar.Balance < 0) return -3;

            if(cars.Remove(delCar))
            {
                return 1;
            }
            else
            {
                return 0;
            }
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

        public int CountOccupiedParkingPlaces()
        {
            lock (carsSyncRoot)
            {
                return this.cars.Count;
            }
        }

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

        public decimal GetIncomeLastMinute()
        {
            decimal sum = 0;

            lock (transactionsSyncRoot)
            {
                sum = transactions.Sum(t => t.Debited);
            }

            return sum;
        }

        public List<Car> GetAllCars()
        {
            lock (carsSyncRoot)
            {
                return CloneList<Car>(this.cars).ToList<Car>();
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            lock (transactionsSyncRoot)
            {
                return CloneList<Transaction>(this.transactions).ToList<Transaction>();
            }
        }

    }
}
