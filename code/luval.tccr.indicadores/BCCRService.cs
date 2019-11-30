using System;
using System.Data;

namespace luval.tccr.indicadores
{
    /// <summary>
    /// Provides a class to make calls to the Costa Rica Central bank and retrieve economic indexes
    /// </summary>
    public class BCCRService
    {
        public DataSet Execute(RequestParameters parameters)
        {
            var client = new IndicadoresEconomicos.wsindicadoreseconomicosSoapClient(IndicadoresEconomicos.wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap);
            var result = client.ObtenerIndicadoresEconomicos("316", "21/11/2019", "29/11/2019", "Oscar Marin", "S", "oscar@marin.cr", "NCIR1R2RRM");
            return new DataSet();
        }
    }
}
