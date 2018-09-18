using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsResumenGenerarForeCast
    {
        public string Canal { get; set; }
        public int Anio { get; set; }
        public decimal CantidadCajas { get; set; }
        public decimal CantidadCajasArch { get; set; }
        public decimal DiferenciaCajasArch { get; set; }
        public decimal CantidadTallos { get; set; }
        public decimal CantidadTallosArch { get; set; }
        public decimal DiferenciaTallosArch { get; set; }
        public decimal CantidadSinCruzar { get; set; }
        public decimal CantidadSinCruzarArch { get; set; }

        public clsResumenGenerarForeCast() { }

    }
}