﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulating.BLL
{
    public class Car
    {
        public string LicensePlate { get; private set; }
        public decimal Balance { get; set; }
        public CarType CarType { get; private set; }

        public Car(string licensePlate, CarType carType, decimal balance = 0)
        {
            this.LicensePlate = LicensePlate;
            this.CarType = carType;
            this.Balance = balance;
        }
    }
}
