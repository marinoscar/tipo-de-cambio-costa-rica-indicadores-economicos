using luval.tccr.config;
using luval.tccr.indicadores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace luval.tccr.storage
{
    public class ExchangeRateService
    {
        public List<ServiceResult> GetIndexResult(int index, DateTime start, DateTime end)
        {
            var res = new List<ServiceResult>();
            var service = new BCCRService();
            var options = new RequestParameters()
            {
                Start = start,
                End = end,
                Index = index,
                ShowSubLevels = false,
                Email = ConfigManager.Setting["BCCR_Service_Email"],
                Name = ConfigManager.Setting["BCCR_Service_Name"],
                Token = ConfigManager.Setting["BCCR_Service_Token"]
            };
            var ds = service.Execute(options);
            if (ds == null || ds.Tables.Count <= 0) return res;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                res.Add(new ServiceResult()
                {
                    Date = Convert.ToDateTime(row["DES_FECHA"]),
                    Value = ToDouble(row["NUM_VALOR"])
                });
            }
            return res;
        }

        private double? ToDouble(object obj)
        {
            if (obj == null || DBNull.Value.Equals(obj)) return null;
            return Convert.ToDouble(obj);
        }
    }
}
