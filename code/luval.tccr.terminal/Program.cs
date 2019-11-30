using luval.tccr.indicadores;
using System;

namespace luval.tccr.terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new BCCRService();
            service.Execute(new RequestParameters());
        }
    }
}
