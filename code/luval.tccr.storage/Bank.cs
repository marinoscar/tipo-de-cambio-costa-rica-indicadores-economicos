using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class Bank
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public int BuyCode { get; set; }
        public int SaleCode { get; set; }
        public string Type { get; set; }
    }
}
