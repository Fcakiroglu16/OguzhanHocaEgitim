using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Products
{
    public class TaxCalculate
    {
        public decimal CalculateTax(decimal price, decimal taxRate)
        {
            return price + (price * taxRate);
        }
    }
}