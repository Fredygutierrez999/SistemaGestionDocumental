using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetConsolidacion_Encabezado : clsBasicoConfiguracion
    {

        public int Consecutivo { get; set; }
        public string Descripcion { get; set; }
        public clsAppNetUsuarios IDAppNetUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public int IDProceso { get; set; }
        public int IDAppNetEstados { get; set; }
        public string NombreEstado { get; set; }
        public int IDAbcap_GeCo { get; set; }
        public DateTime FechaExportacion { get; set; }

        public List<clsAppNetConsolidacion_Detalle_ForeCast> lstForeCast { get; set; }

        public clsAppNetConsolidacion_Encabezado() {
            this.lstForeCast = new List<clsAppNetConsolidacion_Detalle_ForeCast>();
        }

    }
}