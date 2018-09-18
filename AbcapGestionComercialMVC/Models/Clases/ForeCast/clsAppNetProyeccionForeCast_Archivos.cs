using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetProyeccionForeCast_Archivos : clsBasicoConfiguracion
    {
        public string NombreUsuarioSubida { get; set; }
        public string TipoArchivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaSubida { get; set; }
        public DateTime Fecha { get; set; }
        public string RutaCompleta { get; set; }
        public string NombreEstado { get; set; }
        public bool SeEjecutaProceso { get; set; }
        public bool SeMuestraBtnError { get; set; }
        public long cantidadCajas { get; set; }
        public long cantidadCajasArchivo { get; set; }
        public long cantidadTallos { get; set; }
        public long cantidadTallosArchivo { get; set; }

        public clsAppNetProyeccionForeCast_Archivos() {

        }
    }
}