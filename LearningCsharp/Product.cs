using System;
using System.Collections.Generic;
using System.Text;

namespace LearningCsharp
{
    public class Car
    {
        protected int X { get; set; }
    }


    public class SuperCar : Car
    {
        public SuperCar()
        {
            X = 20;
        }
    }


    internal class Product
    {
        public int X { get; set; } // instance variable // property
        public int Y; // instance variable // field

        public static int Z; // static variable // field
        public static int A { get; set; } // static variable // property

        public Product()
        {
            //var car = new Car();
            //car.X = 10;

            //var superCar = new SuperCar();
            //superCar.X = 20;
        }

        public void Work()
        {
        }

        public static void Work1()
        {
        }
    }
}
