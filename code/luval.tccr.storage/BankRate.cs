using Luval.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public string FormattedSaleRate { get; set; }
        public string FormattedBuyRate { get; set; }
        public string FormattedPrevDaySaleRate { get; set; }
        public string FormattedPrevDayBuyRate { get; set; }
        public string FormattedPrevWeekSaleRate { get; set; }
        public string FormattedPrevWeekBuyRate { get; set; }
        public string FormattedPrevMonthSaleRate { get; set; }
        public string FormattedPrevMonthBuyRate { get; set; }


        public void ApplyFormats()
        {
            ApplyFormats(new CultureInfo("es-CR"));
        }
        public void ApplyFormats(CultureInfo ci)
        {
            FormattedSaleRate = DoFormat(SaleRate, ci);
            FormattedBuyRate = DoFormat(BuyRate, ci);
            FormattedPrevDaySaleRate = DoFormat(PrevDaySaleRate, ci);
            FormattedPrevDayBuyRate = DoFormat(PrevDayBuyRate, ci);
            FormattedPrevWeekSaleRate = DoFormat(PrevWeekSaleRate, ci);
            FormattedPrevWeekBuyRate = DoFormat(PrevWeekBuyRate, ci);
            FormattedPrevMonthSaleRate = DoFormat(PrevMonthSaleRate, ci);
            FormattedPrevMonthBuyRate = DoFormat(PrevMonthBuyRate, ci);
        }

        private string DoFormat(double? dbl, CultureInfo ci)
        {
            return dbl == null ? 0d.ToString("N2", ci) : dbl.Value.ToString("N2", ci);
        }


    }
}
