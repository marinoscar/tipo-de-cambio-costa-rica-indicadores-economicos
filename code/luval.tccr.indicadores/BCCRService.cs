using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

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
            var result = client.ObtenerIndicadoresEconomicos("316", "10/11/2019", "29/11/2019", "Oscar Marin", "S", "oscar@marin.cr", "NCIR1R2RRM");
            XmlNode[] nodes = ((XmlNode[])result);
            var ds = new DataSet();
            ds.ReadXmlSchema(new StringReader(nodes[0].OuterXml));
            ds.ReadXml(new StringReader(nodes[1].OuterXml));
            return ds;
        }
    }
}
