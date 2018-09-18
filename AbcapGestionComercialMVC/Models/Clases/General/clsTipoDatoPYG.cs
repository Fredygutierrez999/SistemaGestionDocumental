using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.General
{
    public class clsTipoDatoPYG : clsBasicoConfiguracion
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }

        public clsTipoDatoPYG() {}

    }
}