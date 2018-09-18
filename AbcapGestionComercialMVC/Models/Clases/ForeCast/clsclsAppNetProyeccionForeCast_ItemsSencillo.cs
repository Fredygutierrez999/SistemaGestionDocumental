using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    public class clsclsAppNetProyeccionForeCast_ItemsSencillo
    {
        public long IDAppNetProyeccionForeCast { get; set; }
        public string Canal { get; set; }
        public int IDCliente { get; set; }
        public int IDProductos { get; set; }
        public int IDPresentacion { get; set; }
        public int IDTemporadaBase { get; set; }
        public bool Seleccionado { get; set; }
        public bool Modificado { get; set; }
    }
}