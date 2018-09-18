using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsColumna : clsBasicoConfiguracion
    {
        public bool estatica { get; set; }
        public int IDTipoCampo { get; set; }
        public int WidthInicial { get; set; }
        public bool Dinamico { get; set; }
        public bool CargaDinamica { get; set; }
        public string CampoNoDinamico { get; set; }
    }
}