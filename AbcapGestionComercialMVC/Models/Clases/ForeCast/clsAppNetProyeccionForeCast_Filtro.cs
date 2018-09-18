using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    [Serializable()]
    public class clsAppNetProyeccionForeCast_Filtro
    {
        public long IDF { get; set; }
        public string NombreF { get; set; }
        public clsTipoDatoPYG BasTipoDatoPyGF { get; set; }
        public clsEstadoProyeccion BasEstadoProyeccionF { get; set; }
        public clsAppNetTipoProceso TipoProcesoF { get; set; }

        public clsAppNetProyeccionForeCast_Filtro() {
            this.BasTipoDatoPyGF = new clsTipoDatoPYG() { ID = -1};
            this.BasEstadoProyeccionF = new clsEstadoProyeccion() { ID = -1 };
            this.BasTipoDatoPyGF = new clsTipoDatoPYG() { ID = -1 };
        }
    }
}