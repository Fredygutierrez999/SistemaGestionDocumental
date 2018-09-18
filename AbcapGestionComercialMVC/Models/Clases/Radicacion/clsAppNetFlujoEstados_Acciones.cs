using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetFlujoEstados_Acciones : clsBasicoConfiguracion
    {
        public int IDAppNetFlujoEstados_Sigiente { get; set; }
        public int IDAppNetEstado { get; set; }
    }
}