using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsAppNetItemImportacion
    {
        public string Canal { get; set; }
        public int IDCliente { get; set; }
        public int IDProductos { get; set; }
        public int IDPresentaciones { get; set; }
        public int IDTemporadasBase { get; set; }
        public int IDSemana { get; set; }
        public string NombreSemana { get; set; }
        public decimal CantidadCajas { get; set; }

        public clsAppNetItemImportacion() { }

    }
}