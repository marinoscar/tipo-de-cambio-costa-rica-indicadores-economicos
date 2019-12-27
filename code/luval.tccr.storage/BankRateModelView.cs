using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.storage
{
    public class BankRateModelView
    {
        public string DateControl { get; set; }
        public List<BankRate> Rates { get; set; }
    }
}
