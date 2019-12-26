using Luval.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class BankRate
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public double? SaleRate { get; set; }
        public double? BuyRate { get; set; }
        public double? PrevDaySaleRate { get; set; }
        public double? PrevDayBuyRate { get; set; }
        public double? PrevWeekSaleRate { get; set; }
        public double? PrevWeekBuyRate { get; set; }
        public double? PrevMonthSaleRate { get; set; }
        public double? PrevMonthBuyRate { get; set; }
        public DateTime Date { get; set; }


    }
}
