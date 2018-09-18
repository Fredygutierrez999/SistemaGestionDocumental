using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetDocumentos_Filtros
    {

        public long xBID { get; set; }
        public int xBIDTipoDocumento { get; set; }
        public int xBIDEstadoDocumento { get; set; }
        public string xBNumeroDocumento { get; set; }
        public DateTime xBFechaDocumento { get; set; }
        public DateTime xBFechaRecepcion { get; set; }
        public string xBNota { get; set; }
        public DateTime xBFechaCreacion { get; set; }

        public clsAppNetDocumentos_Filtros() {
            this.xBFechaDocumento = DateTime.MinValue;
            this.xBFechaRecepcion = DateTime.MinValue;
            this.xBFechaCreacion = DateTime.MinValue;
        }

    }
}