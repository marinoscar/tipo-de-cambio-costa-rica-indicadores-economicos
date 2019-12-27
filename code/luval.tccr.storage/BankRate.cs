using Luval.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace luval.tccr.storage
{
    public class BankRate
    {

        public BankRate()
        {
            Labels = new List<string>();
        }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankUrl { get; set; }
        public double? SaleRate { get; set; }
        public double? BuyRate { get; set; }
        public double? PrevDaySaleRate { get; set; }
        public double? PrevDayBuyRate { get; set; }
        public double? PrevWeekSaleRate { get; set; }
        public double? PrevWeekBuyRate { get; set; }
        public double? PrevMonthSaleRate { get; set; }
        public double? PrevMonthBuyRate { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public string FormattedSaleRate { get; set; }
        [NotMapped]
        public string FormattedBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevDaySaleRate { get; set; }
        [NotMapped]
        public string FormattedPrevDaySaleRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRate { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRate { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRateGrowthClass { get; set; }

        [NotMapped]
        public List<string> Labels { get; set; }
        [NotMapped]
        public List<double> PastBuyRates { get; set; }
        [NotMapped]
        public List<double> PastSaleRates { get; set; }


        public void CalculateValues()
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
            ComputeGrowth(ci, PrevDayBuyRate, SaleRate, FormattedPrevDayBuyRateGrowth, FormattedPrevDayBuyRateGrowthClass);
            ComputeGrowth(ci, PrevWeekBuyRate, SaleRate, FormattedPrevWeekBuyRateGrowth, FormattedPrevWeekBuyRateGrowthClass);
            ComputeGrowth(ci, PrevMonthBuyRate, SaleRate, FormattedPrevMonthBuyRateGrowth, FormattedPrevMonthBuyRateGrowthClass);
        }

        private void ComputeGrowth(CultureInfo ci, double? prev, double? current, string toFormat, string classValue)
        {
            var growth = 0d;
            var diff = 0d;
            if (prev != null && current != null)
            {
                growth = Math.Round((1d - (prev.Value / current.Value)) * 100, 2);
                diff = Math.Round(current.Value - prev.Value, 2);
            }
            toFormat = string.Format("{0}{1} ({2}%)", diff < 0 ? "-" : "+", DoFormat(diff, ci), DoFormat(growth, ci));
            if (diff == 0)
                classValue = "badge-secondary";
            else if (diff < 0)
                classValue = "badge-success";
            else
                classValue = "badge-danger";
        }

        private string DoFormat(double? dbl, CultureInfo ci)
        {
            return dbl == null ? 0d.ToString("N2", ci) : dbl.Value.ToString("N2", ci);
        }


    }
}
