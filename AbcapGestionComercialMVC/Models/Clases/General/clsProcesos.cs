using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace AbcapGestionComercialMVC.Models.Clases.General
{
    public class clsProcesos : clsBasicoConfiguracion{
        public Thread objProceso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public clsProcesos() {
            this.FechaCreacion = DateTime.Now;
        }
    }
}