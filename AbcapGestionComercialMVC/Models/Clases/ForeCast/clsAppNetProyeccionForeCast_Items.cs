using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetProyeccionForeCast_Items : clsBasicoConfiguracion
    {
        public clsCanal Canal { get; set; }
        public clsCliente Cliente { get; set; }
        public clsProducto Productos { get; set; }
        public clsPresentaciones Presentacion { get; set; }
        public clsTemporadaBase IDTemporadaBase { get; set; }
        public DateTime FechaAgregacion { get; set; }

        public clsAppNetProyeccionForeCast_Items() {

        }
    }
}