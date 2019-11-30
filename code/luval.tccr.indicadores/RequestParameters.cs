using System;
using System.Collections.Generic;
using System.Text;

namespace luval.tccr.indicadores
{
    /// <summary>
    /// Provides the parameters required to call the web service <see cref="https://gee.bccr.fi.cr/Indicadores/Suscripciones/WS/wsindicadoreseconomicos.asmx?op=ObtenerIndicadoresEconomicos"/>
    /// </summary>
    public class RequestParameters
    {
        /// <summary>
        /// The economic index
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Start date for the dataset
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// End date for the dataset
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// The name of the person making the request
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Indicates if sublevels are to be shown in the result set
        /// </summary>
        public bool ShowSubLevels { get; set; }
        /// <summary>
        /// Email from the person making the request
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The authorized token for the request
        /// </summary>
        public string Token { get; set; }
    }
}
