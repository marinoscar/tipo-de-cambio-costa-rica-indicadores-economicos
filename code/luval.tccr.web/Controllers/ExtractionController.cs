using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using luval.tccr.config;
using luval.tccr.storage;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace luval.tccr.web.Controllers
{
    public class ExtractionController : Controller
    {

        [HttpPost]
        public JsonResult DoExecute()
        {
            var authorizedToken = ConfigManager.Setting["Marin_Token"];
            var hasToken = this.Request.Headers.ContainsKey("marin-auth-token");
            if (!hasToken) throw new ArgumentException("Missing header");
            var tokenValue = Convert.ToString(this.Request.Headers["marin-auth-token"]);
            if(!authorizedToken.Equals(tokenValue)) throw new ArgumentException("Invalid token provided");
            var extractor = new Extractor();
            extractor.StatusMessage += Extractor_StatusMessage;
            int records = 0;

            try
            {
                records = extractor.DoUpsert();
            }
            catch (Exception ex)
            {

                var newEx = new InvalidOperationException("Failed to complete the operation", ex);
                throw newEx;
            }
            return Json(string.Format("Completed. {0} records affected",records));
        }

        private void Extractor_StatusMessage(object sender, string e)
        {
            System.Diagnostics.Trace.TraceWarning(e);
        }
    }
}