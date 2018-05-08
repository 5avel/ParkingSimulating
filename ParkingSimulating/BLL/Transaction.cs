using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public class Transaction
    {
        public DateTime DateTime { get; private set; }
        public string CarLicensePlate { get; private set; }
        public decimal Debited { get; private set; }

        public Transaction(string carLicensePlate, decimal debited)
        {
            this.DateTime = DateTime.Now;
            this.CarLicensePlate = carLicensePlate;
            this.Debited = debited;
        }
    }
}
