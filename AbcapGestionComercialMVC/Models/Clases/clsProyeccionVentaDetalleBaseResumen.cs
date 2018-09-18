using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.Clases
{
    public class clsProyeccionVentaDetalleBaseResumen : clsBasicoConfiguracion
    {
        #region "Parametros"
        public int IdUniversal { get; set; }
        public string Canal { get; set; }
        public int IDcliente { get; set; }
        public string CodigoPedido { get; set; }
        public int IDProductos { get; set; }
        public int IDPresentaciones { get; set; }
        public int IDTemporadasBase { get; set; }
        public int IDPeriodo { get; set; }
        public decimal CantidadCajas { get; set; }
        public decimal Tallos { get; set; }

        [JsonIgnore]
        public List<clsSemana> Semanas { get; set; }
        public decimal[] MatSemanas { get; set; }
        #endregion
        #region Constructor
        public clsProyeccionVentaDetalleBaseResumen() {
            this.Semanas = new List<clsSemana>();
        }
        #endregion
    }
}