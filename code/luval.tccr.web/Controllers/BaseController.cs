using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace luval.tccr.web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewData["Application"] = "Tipo de Cambio CR";
        }
    }
}
