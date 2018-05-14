using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public class Transaction : ICloneable
    {
        public DateTime DateTime { get; private set; }
        public string Id { get; private set; }
        public decimal Debited { get; private set; }

        public Transaction(string id, decimal debited)
        {
            this.DateTime = DateTime.Now;
            this.Id = id;
            this.Debited = debited;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
