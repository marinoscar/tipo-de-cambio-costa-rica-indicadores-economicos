using Luval.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace luval.tccr.storage
{
    public class Extractor
    {
        public event EventHandler<string> StatusMessage;
        private ExchangeRateRepository _exchangeRateRepository;
        private BankRepository _bankRepo;

        public Extractor()
        {
            _exchangeRateRepository = new ExchangeRateRepository();
            _bankRepo = new BankRepository();
        }

        protected virtual void OnStatusMessage(string status)
        {
            StatusMessage?.Invoke(this, status);
        }


        public IEnumerable<ExchangeRate> DoExtract()
        {
            return DoExtract(new DateTime(), DateTime.Today);
        }

        public IEnumerable<ExchangeRate> DoExtract(DateTime startDate, DateTime endDate)
        {
            var banks = _bankRepo.GetActiveBanks();
            return DoExtract(startDate, endDate, banks);
        }

        protected IEnumerable<ExchangeRate> DoExtract(DateTime startDate, DateTime endDate, List<Bank> banks)
        {
            var exchangeRates = new List<ExchangeRate>();
            var isEmptyDate = startDate.Year < DateTime.Today.AddYears(-3).Year;
            foreach (var bank in banks)
            {
                if (isEmptyDate)
                    startDate = _exchangeRateRepository.GetLastExtractDateByBankId(bank.Id);
                //In case the startDate is incorrect we fix the problem
                if (startDate > endDate) startDate = endDate;
                OnStatusMessage(string.Format("Bank: {0} SD: {1} ED: {2} # {3} of {4}", bank.Name, startDate.ToShortDateString(), endDate.ToShortDateString(), banks.IndexOf(bank) + 1, banks.Count));
                var newRates = DoExtract(bank, startDate, endDate);
                exchangeRates.AddRange(newRates);
            }
            return exchangeRates;
        }

        public int UpsertRates(IEnumerable<ExchangeRate> exchangeRates)
        {
            OnStatusMessage(string.Format("Storing {0} rates", exchangeRates.Count()));
            return _exchangeRateRepository.UpsertRates(exchangeRates);
        }

        
        public int DoUpsert()
        {
            var rates = DoExtract();
            return UpsertRates(rates);
        }

        public int BatchInsert(DateTime startDate, DateTime endDate, int monthIncrement)
        {
            var banks = _bankRepo.GetBanks();
            return BatchInsert(banks, startDate, endDate, monthIncrement);
        }

        public int BatchInsert(IEnumerable<Bank> banks, DateTime startDate, DateTime endDate, int monthIncrement)
        {
            var count = 0;
            while (startDate <= endDate)
            {
                OnStatusMessage(string.Format("\nEXTRACTING FROM {0} TO {1}\n", startDate.ToShortDateString(), startDate.AddMonths(monthIncrement).ToShortDateString()));
                var startTs = DateTime.UtcNow;
                var rates = DoExtract(startDate, startDate.AddMonths(monthIncrement), banks.ToList());
                count += _exchangeRateRepository.UpsertRates(rates);
                startDate = startDate.AddMonths(monthIncrement);
                OnStatusMessage(string.Format("{0} rates inserted in {1}", rates.Count(), DateTime.UtcNow.Subtract(startTs)));
            }
            return count;
        }


        public IEnumerable<ExchangeRate> DoExtract(Bank bank, DateTime endDate)
        {
            var startDate = _exchangeRateRepository.GetLastExtractDateByBankId(bank.Id);
            return DoExtract(bank, startDate, endDate);
        }

        public IEnumerable<ExchangeRate> DoExtract(Bank bank)
        {
            return DoExtract(bank, DateTime.Today);
        }

        public IEnumerable<ExchangeRate> DoExtract(Bank bank, DateTime startDate, DateTime endDate)
        {
            var service = new ExchangeRateService();
            var exchangeRates = new List<ExchangeRate>();
            var buyRates = service.GetIndexResult(bank.BuyCode, startDate.Date, endDate.Date)
                .OrderBy(i => i.Date).ToList();
            var saleRates = service.GetIndexResult(bank.SaleCode, startDate.Date, endDate.Date)
                .OrderBy(i => i.Date).ToList();
            foreach (var buyRate in buyRates)
            {
                var saleRate = saleRates.FirstOrDefault(i => i.Date.Date == buyRate.Date.Date);
                var sale = saleRate == null ? 0d : saleRate.Value;
                exchangeRates.Add(new ExchangeRate(bank, buyRate.Date.Date, buyRate.Value, sale));
            }
            OnStatusMessage(string.Format("Bank: {0}, {1} rates processed", bank.Name, buyRates.Count));
            return exchangeRates;
        }
    }
}
