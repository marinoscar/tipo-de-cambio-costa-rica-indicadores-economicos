using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class BankRepository : RepositoryBase
    {
        public List<Bank> GetBanks()
        {
            return Database.ExecuteToEntityList<Bank>("SELECT Id, Name, ISNULL(BuyCode, 0) As BuyCode, ISNULL(SaleCode, 0) As SaleCode, Type, IsActive, Url From Bank");
        }

        public List<Bank> GetActiveBanks()
        {
            return Database.ExecuteToEntityList<Bank>("SELECT Id, Name, ISNULL(BuyCode, 0) As BuyCode, ISNULL(SaleCode, 0) As SaleCode, Type, IsActive, Url FROM Bank WHERE IsActive = 1");
        }
    }
}
