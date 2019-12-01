using System;
using luval.tccr.storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace luval.tccr.extract.func
{
    public static class ExchangeRateService
    {
        private static ILogger _localLogger;
        //Production cron -> "0 6-18/2 * * 1-5"

        [FunctionName("ExchangeRateService")]
        public static void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, ILogger log)
        {
            _localLogger = log;
            _localLogger.LogInformation(string.Format("Starting the exececution of the function"));
            var extractor = new Extractor();
            extractor.StatusMessage += Extractor_StatusMessage;

            try
            {
                extractor.DoUpsert();
            }
            catch (Exception ex)
            {
                _localLogger.LogError(ex.ToString());
                throw ex;
            }
        }

        private static void Extractor_StatusMessage(object sender, string e)
        {
            _localLogger.LogInformation(e);
        }
    }
}
