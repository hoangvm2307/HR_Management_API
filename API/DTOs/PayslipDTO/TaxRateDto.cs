using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TaxRateDto
    {
        public decimal TaxRate1 { get; set; }
        public decimal TaxRate2 { get; set; }
        public decimal TaxRate3 { get; set; }
        public decimal TaxRate4 { get; set; }
        public decimal TaxRate5 { get; set; }
        public decimal TaxRate6 { get; set; }
        public decimal TaxRate7 { get; set; }
        public decimal Total()
        {
            return TaxRate1 + TaxRate2 + TaxRate3 + TaxRate4 + TaxRate5 + TaxRate6 + TaxRate7;
        }
    }
}