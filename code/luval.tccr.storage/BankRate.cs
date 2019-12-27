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
        public string FormattedPrevDaySaleRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevDaySaleRateGrowthIconClass { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevDayBuyRateGrowthIconClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRate { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekSaleRateGrowthIconClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevWeekBuyRateGrowthIconClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRate { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthSaleRateGrowthIconClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRate { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRateGrowth { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRateGrowthClass { get; set; }
        [NotMapped]
        public string FormattedPrevMonthBuyRateGrowthIconClass { get; set; }

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

            ComputeGrowth(ci, PrevDayBuyRate, BuyRate,
                (val) => { FormattedPrevDayBuyRateGrowth = val; },
                (val) => { FormattedPrevDayBuyRateGrowthClass = val; },
                (val) => { FormattedPrevDayBuyRateGrowthIconClass = val; });
            ComputeGrowth(ci, PrevWeekBuyRate, BuyRate,
                (val) => { FormattedPrevWeekBuyRateGrowth = val; },
                (val) => { FormattedPrevWeekBuyRateGrowthClass = val; },
                (val) => { FormattedPrevWeekBuyRateGrowthIconClass = val; });
            ComputeGrowth(ci, PrevMonthBuyRate, BuyRate,
                (val) => { FormattedPrevMonthBuyRateGrowth = val; },
                (val) => { FormattedPrevMonthBuyRateGrowthClass = val; },
                (val) => { FormattedPrevMonthBuyRateGrowthIconClass = val; });

            ComputeGrowth(ci, PrevDaySaleRate, SaleRate,
                (val) => { FormattedPrevDaySaleRateGrowth = val; },
                (val) => { FormattedPrevDaySaleRateGrowthClass = val; },
                (val) => { FormattedPrevDaySaleRateGrowthIconClass = val; });
            ComputeGrowth(ci, PrevWeekSaleRate, SaleRate,
                (val) => { FormattedPrevWeekSaleRateGrowth = val; },
                (val) => { FormattedPrevWeekSaleRateGrowthClass = val; },
                (val) => { FormattedPrevWeekSaleRateGrowthIconClass = val; });
            ComputeGrowth(ci, PrevMonthSaleRate, SaleRate,
                (val) => { FormattedPrevMonthSaleRateGrowth = val; },
                (val) => { FormattedPrevMonthSaleRateGrowthClass = val; },
                (val) => { FormattedPrevMonthSaleRateGrowthIconClass = val; });

        }

        private void ComputeGrowth(CultureInfo ci, double? prev, double? current, Action<string> toFormat, Action<string> classValue, Action<string> iconClassValue)
        {
            var growth = 0d;
            var diff = 0d;
            if (prev != null && current != null)
            {
                growth = Math.Round((1d - (prev.Value / current.Value)) * 100, 2);
                diff = Math.Round(current.Value - prev.Value, 2);
            }
            toFormat(string.Format("{0}{1} ({2}%)", diff < 0 ? "-" : "+", DoFormat(diff, ci), DoFormat(growth, ci)));
            if (diff == 0)
            {
                classValue("badge-secondary");
                iconClassValue("");
            }
            else if (diff < 0)
            {
                classValue("badge-success");
                iconClassValue("fas fa-chevron-down");
            }
            else
            {
                classValue("badge-danger");
                iconClassValue("fas fa-chevron-up");
            }
        }

        private string DoFormat(double? dbl, CultureInfo ci)
        {
            return dbl == null ? 0d.ToString("N2", ci) : dbl.Value.ToString("N2", ci);
        }


    }
}
