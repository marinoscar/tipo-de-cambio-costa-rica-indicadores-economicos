using Luval.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    }
}
