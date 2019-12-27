using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using luval.tccr.web.Models;
using luval.tccr.storage;
using Newtonsoft.Json;

namespace luval.tccr.web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet, OutputCache(Duration = 3600)]
        public JsonResult GetBancoCentral()
        {
            var exchangeRepo = new ExchangeRateRepository();
            return Json(exchangeRepo.GetCentralBankRate(DateTime.Today.Date));

        }

        [HttpGet, OutputCache(Duration = 3600)]
        public JsonResult GetBankData()
        {
            var exchangeRepo = new ExchangeRateRepository();
            var rates = exchangeRepo.GetActiveBanksRatesByDate(DateTime.Today.Date).Where(i => i.BankId != 99).ToList();
            return Json(new BankRateModelView() {
                Rates = rates, DateControl = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            });
        }
    }
}
