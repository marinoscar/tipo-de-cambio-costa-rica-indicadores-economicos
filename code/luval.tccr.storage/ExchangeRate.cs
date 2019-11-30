using Luval.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace luval.tccr.storage
{
    public class ExchangeRate
    {
        public int BankId { get; set; }
        public int DayOfYear { get; set; }
        public int Year { get; set; }
        public DateTime UtcLastUpdateOn { get; set; }
        public DateTime Date { get; set; }
        public double? BuyRate { get; set; }
        public double? SaleRate { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public string DayName { get; set; }

        public ExchangeRate()
        {
            UtcLastUpdateOn = DateTime.UtcNow;
        }

        public ExchangeRate(Bank bank, DateTime date, double buy, double sale) : this(bank.Id, date, buy, sale)
        {
        }

        public ExchangeRate(int bankId, DateTime date, double buy, double sale)
        {
            var culture = CultureInfo.GetCultureInfo("es-ES");
            UtcLastUpdateOn = DateTime.UtcNow;
            BankId = bankId;
            DayOfYear = date.DayOfYear;
            Date = date.Date;
            Year = date.Year;
            BuyRate = buy <= 0 ? default(double?) : buy;
            SaleRate = sale <= 0 ? default(double?) : sale;
            Month = date.Month;
            MonthName = date.ToString("MMMM", culture);
            DayName = date.ToString("dddd", culture);
        }

        public string ToSql()
        {
            var sql = new StringWriter();
            sql.WriteLine("MERGE ExchangeRate AS Target");
            sql.WriteLine("USING (SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}) As Source (BankId, DayOfYear, Year, Date, Month, MonthName, DayName, UtcLastUpdateOn, BuyRate, SaleRate)", BankId.ToSql(), DayOfYear.ToSql(), Year.ToSql(), Date.ToSql(), Month.ToSql(), MonthName.ToSql(), DayName.ToSql(), UtcLastUpdateOn.ToSql(), BuyRate.ToSql(), SaleRate.ToSql());
            sql.WriteLine("ON Target.BankId = {0} And Target.DayOfYear = {1} And Target.Year = {2}", BankId.ToSql(), DayOfYear.ToSql(), Year.ToSql());
            sql.WriteLine("WHEN MATCHED THEN");
            sql.WriteLine(" UPDATE SET Month = {0}, MonthName = {1}, DayName = {2}, UtcLastUpdateOn = {3}, BuyRate = {4}, SaleRate = {5}", Month.ToSql(), MonthName.ToSql(), DayName.ToSql(), UtcLastUpdateOn.ToSql(), BuyRate.ToSql(), SaleRate.ToSql());
            sql.WriteLine("WHEN NOT MATCHED THEN");
            sql.WriteLine(" INSERT (BankId, DayOfYear, Year, Date, Month, MonthName, DayName, UtcLastUpdateOn, BuyRate, SaleRate)");
            sql.WriteLine(" VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9});", BankId.ToSql(), DayOfYear.ToSql(), Year.ToSql(), Date.ToSql(), Month.ToSql(), MonthName.ToSql(), DayName.ToSql(), UtcLastUpdateOn.ToSql(), BuyRate.ToSql(), SaleRate.ToSql());
            return sql.ToString();
        }

    }
}
