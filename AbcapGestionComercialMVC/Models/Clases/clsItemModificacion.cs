using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsItemModificacion
    {
        public int _orden { get; set; }
        public long _IDUniversal { get; set; }
        public int _ID { get; set; }
        public string _AnioSemana { get; set; }
        public string _Canal { get; set; }
        public string _CodigoPedido { get; set; }
        public int _IDClientes { get; set; }
        public int _IDProductos { get; set; }
        public int _IDPresentaciones { get; set; }
        public int _IDTemporadabase { get; set; }
        public int _IDPeriodo { get; set; }
        public decimal _AnteriorValor { get; set; }
        public decimal _NuevoValor { get; set; }
        public int _SincronizadaBD { get; set; }

        public clsItemModificacion()
        {
        }
    }
}