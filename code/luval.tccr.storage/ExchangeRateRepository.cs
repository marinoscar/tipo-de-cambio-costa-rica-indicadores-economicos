using Luval.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;

namespace luval.tccr.storage
{
    public class ExchangeRateRepository : RepositoryBase
    {
        public DateTime GetLastExtractDateByBankId(int bankId)
        {
            var res = Database.ExecuteScalar("SELECT MAX(Date) FROM ExchangeRate Where BankId = {0}".FormatSql(bankId));
            return res.IsNullOrDbNull() ? DateTime.Today.AddYears(-3) : Convert.ToDateTime(res);
        }

        public int UpsertRates(IEnumerable<ExchangeRate> rates)
        {
            var count = 0;
            foreach (var rate in rates)
            {
                count += Database.ExecuteNonQuery(rate.ToSql());
            }
            return count;
        }

        public int InsertRates(IEnumerable<ExchangeRate> exchangeRates)
        {
            var entityAdapter = new SqlEntityAdapter(Database, typeof(ExchangeRate));
            var records = new List<DataRecordAction>();
            foreach (var rate in exchangeRates)
            {
                records.Add(new DataRecordAction() { Record = DictionaryDataRecord.FromEntity(rate), Action = DataAction.Insert });
            }
            entityAdapter.ExecuteInTransaction(records);
            return records.Count;
        }

        public BankRate GetCentralBankRate(DateTime date)
        {
            return GetActiveBanksRatesByDate(date, 99).FirstOrDefault();
        }

        public List<BankRate> GetActiveBanksRatesByDate(DateTime date)
        {
            return GetActiveBanksRatesByDate(date, 0);
        }

        public List<BankRate> GetActiveBanksRatesByDate(DateTime date, int bankId)
        {
            date = ToWorkDate(date);
            var bankSql = bankId > 0 ? string.Format(" And Bank.Id = {0}", bankId) : "";
            var sql = @"
SELECT 
	 Bank.Id As BankId
	,Bank.Name As BankName
	,ER.SaleRate
	,ER.BuyRate
	,(SELECT SaleRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {1} ) As PrevDaySaleRate
	,(SELECT BuyRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {1} ) As PrevDayBuyRate
	,(SELECT SaleRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {2} ) As PrevWeekSaleRate
	,(SELECT BuyRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {2} ) As PrevWeekBuyRate
	,(SELECT SaleRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {3} ) As PrevMonthSaleRate
	,(SELECT BuyRate FROM ExchangeRate WHERE BankId = Bank.Id And Date = {3} ) As PrevMonthBuyRate
	,ER.Date
FROM
	Bank
	INNER JOIN ExchangeRate ER ON Bank.Id = ER.BankId
WHERE
	Bank.IsActive = 1 And ER.Date = {0}

".FormatSql(date, ToWorkDate(date.AddDays(-1)), ToWorkDate(date.AddDays(-7)), ToWorkDate(date.AddMonths(-1)), bankSql);

            var rates = Database.ExecuteToEntityList<BankRate>(sql + bankSql).ToList();
            rates.ForEach(i => i.ApplyFormats());
            return rates;

        }

    }
}
