using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetConsolidacion_Detalle_ForeCast : clsBasicoConfiguracion
    {
        public clsAppNetProyeccionForeCast objAppNetProyeccionForeCast { get; set; }
        public int IDEstado { get; set; }
        public List<string> mensajesValidacion { get; set; }
        public bool Seleccionado { get; set; }

        public clsAppNetConsolidacion_Detalle_ForeCast() {
            this.mensajesValidacion = new List<string>();
        }

    }
}