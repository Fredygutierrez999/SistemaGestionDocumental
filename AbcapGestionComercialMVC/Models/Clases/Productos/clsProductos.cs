using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.Productos
{
    public class clsProductosSincronizacion : clsBasicoConfiguracion
    {
        public string Revision { get; set; }
        public int IDClasificacion { get; set; }
        public string NombreClasificacion { get; set; }
        public int IDPresentacion { get; set; }
        public string NombrePresentacion { get; set; }
        public int IDTipoProducto { get; set; }
        public string NombreTipoProducto { get; set; }

        public clsProductosSincronizacion() {

        }

    }
}