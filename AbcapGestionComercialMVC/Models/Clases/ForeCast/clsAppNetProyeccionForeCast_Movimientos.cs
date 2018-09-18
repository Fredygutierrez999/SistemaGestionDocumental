using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetProyeccionForeCast_Movimientos : clsBasicoConfiguracion
    {
        public int IDMovimiento { get; set; }
        public int IDUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IDTipo { get; set; }
        public string NombreTipo { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; }

        public clsAppNetProyeccionForeCast_Movimientos()
        {

        }

    }
}