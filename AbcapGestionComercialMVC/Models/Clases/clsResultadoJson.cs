using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsResultadoJson
    {
        public bool ResultadoProceso { get; set; }
        public string MensajeProceso { get; set; }
        public Object objResultado { get; set; }
        public string MensajeTecnicoFormulario { get; set; }

        public  clsResultadoJson() {
            this.ResultadoProceso = true;
        }

        /// <summary>
        /// Metodo encargado de cargar detalles del error
        /// </summary>
        /// <param name="ex"></param>
        public void cargarErro(Exception ex)
        {
            this.MensajeProceso = ex.Message;
            this.ResultadoProceso = false;
        }

    }
}