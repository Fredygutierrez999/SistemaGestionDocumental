using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsProyeccionVentaDetalleBase : clsBasicoConfiguracion
    {
        #region "Parametros"
        public clsSemana semana { get; set; }
        public string Canal { get; set; }
        public clsCliente cliente { get; set; }
        public string CodigoPedido { get; set; }
        public clsProducto Productos { get; set; }
        public clsPresentaciones Presentaciones { get; set; }
        public clsTemporadaBase TemporadasBase { get; set; }
        public clsPeriodo Periodo { get; set; }
        public decimal CantidadCajas { get; set; }
        #endregion
        #region Constructor
        public clsProyeccionVentaDetalleBase(){}
        #endregion
    }
}