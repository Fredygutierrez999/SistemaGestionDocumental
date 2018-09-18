using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.Clases
{
    public class clsProyeccionVenta : clsBasicoConfiguracion
    {
        #region Parametros de la proyeccion
        /// <summary>
        /// Tipo de proyeccion
        /// </summary>
        public clsTipoProyeccion TipoProyeccion { get; set; }

        /// <summary>
        /// Estado de la proyeccion
        /// </summary>
        public clsEstadoProyeccion EstadoProyeccion { get; set; }

        /// <summary>
        /// Semana Inicial
        /// </summary>
        public clsSemana SemanaDesde { get; set; }

        /// <summary>
        /// Semana Final
        /// </summary>
        public clsSemana SemanaHasta { get; set; }

        /// <summary>
        /// Observaciones o nombre de la proyeccion
        /// </summary>
        public string Observacion { get; set; }

        /// <summary>
        /// Fecha de la proyeccion
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// ID usuario
        /// </summary>
        public string IDAppUsr { get; set; }

        /// <summary>
        /// Proyeccion antigua
        /// </summary>
        public int IDProyeccionVenta { get; set; }

        /// <summary>
        /// Listado de detalle de proyeccion
        /// </summary>
        public List<clsProyeccionVentaDetalleBase> lstDetalleProyeccion;


        /// <summary>
        /// Listado de detalle de proyeccion
        /// </summary>
        public List<clsProyeccionVentaDetalleBaseResumen> lstDetalleProyeccionResumen;

        /// <summary>
        /// Clientees
        /// </summary>
        public List<clsCliente> lstClientes;

        /// <summary>
        /// Productos
        /// </summary>
        public List<clsProducto> lstProductos;

        /// <summary>
        /// Presentaciones
        /// </summary>
        public List<clsPresentaciones> lstPresentaciones;

        /// <summary>
        /// Temporadas
        /// </summary>
        public List<clsTemporadaBase> lstTemporadas;

        /// <summary>
        /// Periodos
        /// </summary>
        public List<clsPeriodo> lstPeriodos;

        /// <summary>
        /// Columnas
        /// </summary>
        public List<clsColumna> lstColumnas;


        public string Canal { get; set; }
        #endregion

        #region Constructor
        public clsProyeccionVenta()
        {
            this.lstDetalleProyeccion = new List<clsProyeccionVentaDetalleBase>();
            this.lstDetalleProyeccionResumen = new List<clsProyeccionVentaDetalleBaseResumen>();
            this.lstClientes = new List<clsCliente>();
            this.lstProductos = new List<clsProducto>();
            this.lstPresentaciones = new List<clsPresentaciones>();
            this.lstTemporadas = new List<clsTemporadaBase>();
            this.lstPeriodos = new List<clsPeriodo>();
            this.lstColumnas = new List<clsColumna>();
        }
        #endregion
    }
}