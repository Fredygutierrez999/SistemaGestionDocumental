using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AbcapGestionComercialMVC.Models.Clases;
using DataControllerUtility.ProcesamientoXMLaSQL;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsProyeccionVentaBD : clsConfgiruacionBD
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public clsProyeccionVentaBD() { }

        /// <summary>
        /// Consulta proyeccion
        /// </summary>
        public clsProyeccionVenta consultarProyeccion(long xIdProyeccion, string xCanal)
        {
            try
            {
                clsProyeccionVenta objProyeccion = new clsProyeccionVenta();
                this.objControlador.setCommand("consultaProyeccionBase");
                this.objControlador.addNewParameter("@IDProyeccionVenta", xIdProyeccion);
                this.objControlador.addNewParameter("@Canal", xCanal);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                this.objControlador.closeConnection();

                List<clsProyeccionVenta> lstProyecciones = consultaBasicaProyecciones(xIdProyeccion);
                objProyeccion = lstProyecciones[0];

                /*NUMERO DE FILA QUE EMPIEZA A CARGAR SEMANAS*/
                int xColumna = Convert.ToInt32(dtsDatos.Tables[CERO].Rows[CERO]["SemanaInicia"].ToString());

                /*CARGA DETALLE PLAN DE VENTAS*/
                DataTable dtrResumenPlanVentas = dtsDatos.Tables[UNO];
                for (int i = 0; i < dtrResumenPlanVentas.Rows.Count; i++)
                {
                    DataRow dtrFila = dtrResumenPlanVentas.Rows[i];
                    clsProyeccionVentaDetalleBaseResumen objResumen = new clsProyeccionVentaDetalleBaseResumen();
                    objResumen.IdUniversal = Convert.ToInt32(dtrFila["ID"].ToString()) -1;
                    objResumen.ID = Convert.ToInt64(dtrFila["ID"].ToString());
                    objResumen.Canal = dtrFila["Canal"].ToString();
                    objResumen.IDcliente = Convert.ToInt32(dtrFila["IDClientes"].ToString());
                    objResumen.CodigoPedido = dtrFila["CodigoPedido"].ToString();
                    objResumen.IDProductos = Convert.ToInt32(dtrFila["IDProductos"].ToString());
                    objResumen.IDPresentaciones = Convert.ToInt32(dtrFila["IDPresentaciones"].ToString());
                    objResumen.IDTemporadasBase = Convert.ToInt32(dtrFila["IDTemporadasBase"].ToString());
                    objResumen.IDPeriodo = Convert.ToInt32(dtrFila["IDPeriodo"].ToString());
                    objResumen.Tallos = Convert.ToDecimal(dtrFila["Tallos"].ToString());
                    objResumen.MatSemanas = new decimal[(dtrResumenPlanVentas.Columns.Count - 1) - (xColumna)];
                    /*CARGA EL LISTADO DE SEMANAS PARA ESTA COMBINACIÓN*/
                    for (int j = xColumna; j < dtrResumenPlanVentas.Columns.Count - 1; j++)
                    {
                        objResumen.MatSemanas[j - xColumna] = Convert.ToDecimal(dtrResumenPlanVentas.Rows[i][j + 1].ToString());
                        //clsSemana objSemana = new clsSemana();
                        //string xAnioSemana = dtrResumenPlanVentas.Columns[j].ColumnName.ToString();
                        //objSemana.ANO = xAnioSemana.Substring(0, 4);
                        //objSemana.SEMANA = xAnioSemana.Substring(4, 2);
                        //objSemana.CantidadCajas = 
                        //objResumen.Semanas.Add(objSemana);
                    }
                    objProyeccion.lstDetalleProyeccionResumen.Add(objResumen);
                }

                DataTable dtrColumnas = dtsDatos.Tables[SIETE];
                for (int i = 0; i < dtrColumnas.Rows.Count; i++)
                {
                    clsColumna objColumna = new clsColumna();
                    objColumna.ID = Convert.ToInt64(dtrColumnas.Rows[i]["ID"].ToString());
                    objColumna.Nombre = dtrColumnas.Rows[i]["NombreCampo"].ToString();
                    objColumna.IDTipoCampo = Convert.ToInt32(dtrColumnas.Rows[i]["IDTipoCampo"].ToString());
                    objColumna.WidthInicial = Convert.ToInt32(dtrColumnas.Rows[i]["WidthInicial"].ToString());
                    objColumna.Dinamico = Convert.ToBoolean(dtrColumnas.Rows[i]["Dinamico"].ToString());
                    objColumna.CargaDinamica = Convert.ToBoolean(dtrColumnas.Rows[i]["CargaDinamica"].ToString());
                    objColumna.CampoNoDinamico = dtrColumnas.Rows[i]["CampoNoDinamico"].ToString();
                    objProyeccion.lstColumnas.Add(objColumna);
                }

                /*CARGAR COLUMNAS*/
                for (int i = xColumna + 1; i < dtrResumenPlanVentas.Columns.Count; i++)
                {
                    clsColumna objColumna = new clsColumna();
                    objColumna.ID = i;
                    objColumna.Nombre = dtrResumenPlanVentas.Columns[i].ColumnName;
                    objColumna.estatica = (i < xColumna);
                    objColumna.WidthInicial = 80;
                    objColumna.IDTipoCampo = 3;
                    objColumna.CargaDinamica = false;
                    objColumna.CampoNoDinamico = dtrResumenPlanVentas.Columns[i].ColumnName;
                    objProyeccion.lstColumnas.Add(objColumna);
                }

                /*CARGA CLIENTES*/
                DataTable dtrResumenClientes = dtsDatos.Tables[DOS];
                for (int i = 0; i < dtrResumenClientes.Rows.Count; i++)
                {
                    clsCliente objCliente = new clsCliente();
                    objCliente.ID = Convert.ToInt32(dtrResumenClientes.Rows[i]["ID"].ToString());
                    objCliente.Nombre = dtrResumenClientes.Rows[i]["Nombre"].ToString();
                    objCliente.Codigo = dtrResumenClientes.Rows[i]["Codigo"].ToString();
                    objCliente.IDCanales = Convert.ToInt32(dtrResumenClientes.Rows[i]["IDCanales"].ToString());
                    objCliente.IDSubCanales = Convert.ToInt32(dtrResumenClientes.Rows[i]["IDSubCanales"].ToString());
                    objCliente.IDDivisionesClientes = Convert.ToInt32(dtrResumenClientes.Rows[i]["IDDivisionesClientes"].ToString());
                    objCliente.IDCiudades = Convert.ToInt32(dtrResumenClientes.Rows[i]["IDCiudades"].ToString());
                    objProyeccion.lstClientes.Add(objCliente);
                }

                /*PRODUCTOS*/
                DataTable dtrProductos = dtsDatos.Tables[TRES];
                for (int i = 0; i < dtrProductos.Rows.Count; i++)
                {
                    clsProducto objProductoItem = new clsProducto();
                    objProductoItem.ID = Convert.ToInt32(dtrProductos.Rows[i]["ID"].ToString());
                    objProductoItem.Nombre = dtrProductos.Rows[i]["Nombre"].ToString();
                    objProductoItem.Codigo = dtrProductos.Rows[i]["Codigo"].ToString();
                    objProyeccion.lstProductos.Add(objProductoItem);
                }

                /*PRESENTACIONES*/
                DataTable dtrPresentaciones = dtsDatos.Tables[CUATRO];
                for (int i = 0; i < dtrPresentaciones.Rows.Count; i++)
                {
                    clsPresentaciones objProductoItem = new clsPresentaciones();
                    objProductoItem.ID = Convert.ToInt32(dtrPresentaciones.Rows[i]["ID"].ToString());
                    objProductoItem.Nombre = dtrPresentaciones.Rows[i]["Nombre"].ToString();
                    objProductoItem.Codigo = dtrPresentaciones.Rows[i]["Codigo"].ToString();
                    objProyeccion.lstPresentaciones.Add(objProductoItem);
                }

                /*TEMPORADAS*/
                DataTable dtrTemporadas = dtsDatos.Tables[CINCO];
                for (int i = 0; i < dtrTemporadas.Rows.Count; i++)
                {
                    clsTemporadaBase objProductoItem = new clsTemporadaBase();
                    objProductoItem.ID = Convert.ToInt32(dtrTemporadas.Rows[i]["ID"].ToString());
                    objProductoItem.Nombre = dtrTemporadas.Rows[i]["Nombre"].ToString();
                    objProductoItem.Codigo = dtrTemporadas.Rows[i]["Codigo"].ToString();
                    objProyeccion.lstTemporadas.Add(objProductoItem);
                }

                /*PERIODOS*/
                DataTable dtrPeriodos = dtsDatos.Tables[SEIX];
                for (int i = 0; i < dtrPeriodos.Rows.Count; i++)
                {
                    clsPeriodo objProductoItem = new clsPeriodo();
                    objProductoItem.ID = Convert.ToInt32(dtrPeriodos.Rows[i]["ID"].ToString());
                    objProductoItem.Nombre = dtrPeriodos.Rows[i]["Nombre"].ToString();
                    objProductoItem.Codigo = dtrPeriodos.Rows[i]["Codigo"].ToString();
                    objProyeccion.lstPeriodos.Add(objProductoItem);
                }
                return objProyeccion;
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
                return null;
            }
        }

        /// <summary>
        /// Metodo encargado de consultar proyecciones
        /// </summary>
        /// <returns></returns>
        public List<clsProyeccionVenta> consultaBasicaProyecciones()
        {
            return this.consultaBasicaProyecciones(-1);
        }

        /// <summary>
        /// Metodo encargado de consultar proyecciones
        /// </summary>
        /// <param name="xIdProyeccion">Código proyeccion</param>
        /// <returns></returns>
        public List<clsProyeccionVenta> consultaBasicaProyecciones(long xIdProyeccion)
        {
            List<clsProyeccionVenta> lstProyeccionVenta = new List<clsProyeccionVenta>();
            this.objControlador.setCommand("consultarProyeccionVenta");
            this.objControlador.addNewParameter("@ID", xIdProyeccion);
            DataTable dttDatos = this.objControlador.execTableResult();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsProyeccionVenta objProyeccion = new clsProyeccionVenta();
                objProyeccion.ID = Convert.ToInt32(dttDatos.Rows[i]["Id"].ToString());
                objProyeccion.Nombre = dttDatos.Rows[i]["Nombre"].ToString();

                objProyeccion.TipoProyeccion = new clsTipoProyeccion();
                objProyeccion.TipoProyeccion.ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetTipoProceso"].ToString());
                objProyeccion.TipoProyeccion.Nombre = dttDatos.Rows[i]["NombreTipoProyeccion"].ToString();

                objProyeccion.EstadoProyeccion = new clsEstadoProyeccion();
                objProyeccion.EstadoProyeccion.ID = Convert.ToInt32(dttDatos.Rows[i]["IDEstado"].ToString());
                objProyeccion.EstadoProyeccion.Nombre = dttDatos.Rows[i]["NombreEstadoProyeccion"].ToString();

                objProyeccion.SemanaDesde = new clsSemana();
                objProyeccion.SemanaDesde.ANO = Convert.ToInt32(dttDatos.Rows[i]["SemanaDesde"].ToString().Substring(0, 4));
                objProyeccion.SemanaDesde.SEMANA = dttDatos.Rows[i]["SemanaDesde"].ToString().Substring(4, 2);

                objProyeccion.SemanaHasta = new clsSemana();
                objProyeccion.SemanaHasta.ANO = Convert.ToInt32(dttDatos.Rows[i]["SemanaHasta"].ToString().Substring(0, 4));
                objProyeccion.SemanaHasta.SEMANA = dttDatos.Rows[i]["SemanaHasta"].ToString().Substring(4, 2);

                objProyeccion.Observacion = dttDatos.Rows[i]["Descripcion"].ToString();
                objProyeccion.Fecha = Convert.ToDateTime(dttDatos.Rows[i]["FechaCreacion"].ToString());

                lstProyeccionVenta.Add(objProyeccion);
            }
            this.objControlador.closeConnection();
            return lstProyeccionVenta;
        }


        /// <summary>
        /// Metodo encargado de consultar listado de canales
        /// </summary>
        /// <returns></returns>
        public List<clsCanal> consultaCanales()
        {
            List<clsCanal> lstCanal = new List<clsCanal>();
            this.objControlador.setCommand("consultaSiglaAgrupacion");
            DataTable dttDatos = this.objControlador.execTableResult();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsCanal objCanal = new clsCanal();
                objCanal.Nombre = dttDatos.Rows[i]["SiglaAgrupacionCliente"].ToString();
                lstCanal.Add(objCanal);
            }
            this.objControlador.closeConnection();
            return lstCanal;
        }

        /// <summary>
        /// Metodo encargado de consultar listado de canales para el usuario
        /// </summary>
        /// <returns></returns>
        public List<clsCanal> consultaCanalesUsuario(clsAppNetUsuarios objUsuario)
        {
            List<clsCanal> lstCanal = new List<clsCanal>();
            this.objControlador.setCommand("consultaCanalesUsuario");
            this.objControlador.addNewParameter("@IDAppNetUsuarios", objUsuario.ID);
            DataTable dttDatos = this.objControlador.execTableResult();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsCanal objCanal = new clsCanal();
                objCanal.Nombre = dttDatos.Rows[i]["Canal"].ToString();
                lstCanal.Add(objCanal);
            }
            this.objControlador.closeConnection();
            return lstCanal;
        }


        /// <summary>
        /// Metodo encargado de consultar listado de canales para el usuario
        /// </summary>
        /// <returns></returns>
        public List<clsCanal> consultaCanalesUsuarioYForeCast(clsAppNetUsuarios objUsuario, int xIDForeCast)
        {
            List<clsCanal> lstCanal = new List<clsCanal>();
            this.objControlador.setCommand("consultaCanalesUsuarioForeCast");
            this.objControlador.addNewParameter("@IDAppNetUsuarios", objUsuario.ID);
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
            DataTable dttDatos = this.objControlador.execTableResult();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsCanal objCanal = new clsCanal();
                objCanal.Nombre = dttDatos.Rows[i]["Canal"].ToString();
                lstCanal.Add(objCanal);
            }
            this.objControlador.closeConnection();
            return lstCanal;
        }

        /// <summary>
        /// metodo encargado de guardar en la base de datos los movimientos realizados en la modificación de la proyección
        /// </summary>
        /// <param name="lstMovimientosBD">Listado movimientos</param>
        /// <param name="xIDModificacion">IDModificacion</param>
        /// <returns></returns>
        public Boolean sincronizacion(int xIdProyeccionVenta, List<clsItemModificacion> lstMovimientosBD, string xIDModificacion)
        {
            this.objControlador.setCommand("sincronizaXMLMovimientos");
            this.objControlador.addNewParameter("@IDProyeccionVenta", xIdProyeccionVenta);
            this.objControlador.addNewParameter("@IDModificacion", xIDModificacion);
            this.objControlador.addNewParameter("@Movimientos", procesamientoObjetoAXML.listadoAXML(lstMovimientosBD, "@Movimientos"));
            this.objControlador.execScalar();
            this.objControlador.closeConnection();
            return true;
        }


        /// <summary>
        /// Metodo encargado de limpiar tablas temporales
        /// </summary>
        /// <param name="xIdProyeccionVenta"></param>
        /// <param name="xIDModificacion"></param>
        /// <returns></returns>
        public Boolean LimpiaSincronizacion(int xIdProyeccionVenta, string xIDModificacion)
        {
            this.objControlador.setCommand("limpiarTablasTemporalesEdicionProyeccion");
            this.objControlador.addNewParameter("@IDProyeccionVenta", xIdProyeccionVenta);
            this.objControlador.addNewParameter("@IDModificacion", xIDModificacion);
            this.objControlador.execNoResults();
            this.objControlador.closeConnection();
            return true;
        }


        /// <summary>
        /// Metodo utilziado para graficar datos por canal
        /// </summary>
        /// <param name="xIdProyeccion"></param>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public clsProyeccionVenta consultaGraficaConsolidadoForeCast(long xIdProyeccion, clsAppNetUsuarios objUsuario)
        {
            try
            {
                clsProyeccionVenta objProyeccion = new clsProyeccionVenta();
                this.objControlador.setCommand("consultaForeCastPorCanalGrafica");
                this.objControlador.addNewParameter("@IDProyeccionVenta", xIdProyeccion);
                this.objControlador.addNewParameter("@IDAppNetUsurio", objUsuario.ID);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                this.objControlador.closeConnection();

                List<clsProyeccionVenta> lstProyecciones = consultaBasicaProyecciones(xIdProyeccion);
                objProyeccion = lstProyecciones[0];

                /*CARGA DETALLE PLAN DE VENTAS*/
                DataTable dtrResumenPlanVentas = dtsDatos.Tables[CERO];
                for (int i = 0; i < dtrResumenPlanVentas.Rows.Count; i++)
                {
                    DataRow dtrFila = dtrResumenPlanVentas.Rows[i];
                    clsProyeccionVentaDetalleBaseResumen objResumen = new clsProyeccionVentaDetalleBaseResumen();
                    objResumen.Canal = dtrFila["Canal"].ToString();
                    objResumen.Nombre = dtrFila["Color"].ToString();
                    objResumen.MatSemanas = new decimal[(dtrResumenPlanVentas.Columns.Count - 1) - (1)];
                    /*CARGA EL LISTADO DE SEMANAS PARA ESTA COMBINACIÓN*/
                    for (int j = 1; j < dtrResumenPlanVentas.Columns.Count - 1; j++)
                    {
                        objResumen.MatSemanas[j - 1] = Convert.ToDecimal(dtrResumenPlanVentas.Rows[i][j + 1].ToString());
                    }
                    objProyeccion.lstDetalleProyeccionResumen.Add(objResumen);
                }


                /*CARGAR COLUMNAS*/
                for (int i = 1; i < dtrResumenPlanVentas.Columns.Count; i++)
                {
                    clsColumna objColumna = new clsColumna();
                    objColumna.ID = i;
                    objColumna.Nombre = dtrResumenPlanVentas.Columns[i].ColumnName;
                    objColumna.estatica = (i < 1);
                    objColumna.WidthInicial = 80;
                    objColumna.IDTipoCampo = 3;
                    objColumna.CargaDinamica = false;
                    objColumna.CampoNoDinamico = dtrResumenPlanVentas.Columns[i].ColumnName;
                    objProyeccion.lstColumnas.Add(objColumna);
                }

                return objProyeccion;
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
                return null;
            }
        }

    }
}