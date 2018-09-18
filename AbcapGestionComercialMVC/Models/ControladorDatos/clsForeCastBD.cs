using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using AbcapGestionComercialMVC.Models.Clases.ForeCast;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.App_Start;
using DataControllerUtility.ProcesamientoXMLaSQL;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsForeCastBD : clsConfgiruacionBD
    {
        public clsForeCastBD()
        {
        }

        /// <summary>
        /// Metodo encargado de consultar tipo de datos PyG
        /// </summary>
        /// <returns></returns>
        public List<clsTipoDatoPYG> consultaListadoTipoDatoPyG()
        {
            List<clsTipoDatoPYG> lstTipoDatoPyG = new List<clsTipoDatoPYG>();
            this.objControlador.setCommand("consultaTipoDatoPyG");
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsTipoDatoPYG objtipoPyG = new clsTipoDatoPYG();
                objtipoPyG.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                objtipoPyG.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                objtipoPyG.fechaInicial = Convert.ToDateTime(dttDatos.Rows[i]["FechaInicial"].ToString());
                objtipoPyG.fechaFinal = Convert.ToDateTime(dttDatos.Rows[i]["FechaFinal"].ToString());
                lstTipoDatoPyG.Add(objtipoPyG);
            }

            this.objControlador.closeConnection();
            return lstTipoDatoPyG;
        }

        /// <summary>
        /// Metodo encargado de consultar tipo de datos PyG
        /// </summary>
        /// <returns></returns>
        public List<clsTipoDatoPYG> consultaListadoSemanaInicial()
        {
            List<clsTipoDatoPYG> lstTipoDatoPyG = new List<clsTipoDatoPYG>();
            this.objControlador.setCommand("consultaTipoDatoPyG");
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsTipoDatoPYG objtipoPyG = new clsTipoDatoPYG();
                objtipoPyG.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                objtipoPyG.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                objtipoPyG.fechaInicial = Convert.ToDateTime(dttDatos.Rows[i]["FechaInicial"].ToString());
                objtipoPyG.fechaFinal = Convert.ToDateTime(dttDatos.Rows[i]["FechaFinal"].ToString());
                lstTipoDatoPyG.Add(objtipoPyG);
            }

            this.objControlador.closeConnection();
            return lstTipoDatoPyG;
        }

        /// <summary>
        /// Metodo encargadao de consultar llistado de ForeCast que se basan en los filtros
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast> consultaForeCast(clsAppNetProyeccionForeCast_Filtro objFiltro, clsAppNetUsuarios objUsuario)
        {
            List<clsAppNetProyeccionForeCast> lstForeCast = new List<clsAppNetProyeccionForeCast>();
            this.objControlador.setCommand("AppNetProyeccionForeCast_Consulta");
            this.objControlador.addNewParameter("@ID", objFiltro.IDF);
            this.objControlador.addNewParameter("@Nombre", clsGeneralBD.validaCadenaNUllAMenosUno(objFiltro.NombreF));
            this.objControlador.addNewParameter("@IDBas_TipoDatoPyG", objFiltro.BasTipoDatoPyGF.ID);
            this.objControlador.addNewParameter("@IDEstado", objFiltro.BasEstadoProyeccionF.ID);
            this.objControlador.addNewParameter("@IDUsuaio", objUsuario.ID);
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                DataRow dttFila = dttDatos.Rows[i];
                clsAppNetProyeccionForeCast objForeCast = new clsAppNetProyeccionForeCast();
                objForeCast.ID = Convert.ToInt64(dttFila["ID"].ToString());
                objForeCast.Nombre = dttFila["Nombre"].ToString();
                objForeCast.Descripcion = dttFila["Descripcion"].ToString();
                objForeCast.Bas_TipoDatoPYG = new clsTipoDatoPYG() { ID = Convert.ToInt64(dttFila["IDBas_TipoDatoPYG"].ToString()), Nombre = dttFila["Nombre_TipoDatoPyG"].ToString() };
                objForeCast.AnioSemanaInicial = new clsSemana() { ID = Convert.ToInt64(dttFila["AnioSemanaInicial"].ToString()), Nombre = dttFila["NombreAnioSemanaInicial"].ToString(), ANO = Convert.ToInt32(dttFila["AnioInicial"].ToString()) };
                objForeCast.AnioSemanaFinal = new clsSemana() { ID = Convert.ToInt64(dttFila["AnioSemanaFinal"].ToString()), Nombre = dttFila["NombreAnioSemanaFinal"].ToString(), ANO = Convert.ToInt32(dttFila["AnioFinal"].ToString()) };
                objForeCast.FechaCreacion = Convert.ToDateTime(dttFila["FechaCreacion"].ToString());
                objForeCast.FechaModificacion = Convert.ToDateTime(dttFila["FechaModificacion"].ToString());
                objForeCast.UsuarioCreacion = new clsAppNetUsuarios() { ID = Convert.ToInt64(dttFila["UsuarioCreacion"].ToString()), UserName = dttFila["UserNameCreacion"].ToString() };
                objForeCast.Usuariomodificacion = new clsAppNetUsuarios() { ID = Convert.ToInt64(dttFila["Usuariomodificacion"].ToString()), UserName = dttFila["UserNameModificacion"].ToString() };
                objForeCast.EstadoObj = new clsEstadoProyeccion() { ID = Convert.ToInt64(dttFila["IDEstado"].ToString()), Nombre = dttFila["NombreEstado"].ToString() };
                objForeCast.Consecutivo = Convert.ToInt32(dttFila["Consecutivo"].ToString());
                objForeCast.TipoProceso = new clsAppNetTipoProceso() { ID = Convert.ToInt64(dttFila["IDAppNetTipoProceso"].ToString()) };
                objForeCast.CargarAnteriorForeCast = Convert.ToBoolean(dttFila["CargarAnteriorForeCast"].ToString());
                objForeCast.IDForeCastAnterior = Convert.ToInt64(dttFila["IDForeCastAnterior"].ToString());
                objForeCast.DatosConsolidados = Convert.ToBoolean(dttFila["DatosConsolidados"].ToString());
                lstForeCast.Add(objForeCast);
            }

            this.objControlador.closeConnection();
            return lstForeCast;
        }

        /// <summary>
        /// Metodo encargao de asignar valores por defecto
        /// </summary>
        /// <param name="objAppnet"></param>
        public void asignarValoresForeCastDefecto(clsAppNetProyeccionForeCast objAppnet)
        {
            this.objControlador.setCommand("ValoresPorDefectoCreacion");
            DataTable dttDatos = this.objControlador.execTableResult();
            this.objControlador.closeConnection();
            if (dttDatos.Rows.Count == 1)
            {
                objAppnet.Bas_TipoDatoPYG = new clsTipoDatoPYG() { ID = Convert.ToInt32(dttDatos.Rows[0]["ID"].ToString()) };
                objAppnet.AnioSemanaInicial = new clsSemana() { ID = Convert.ToInt32(dttDatos.Rows[0]["SemanaInicial"].ToString()), ANO = Convert.ToInt32(dttDatos.Rows[0]["AnoInicial"].ToString()) };
                objAppnet.AnioSemanaFinal = new clsSemana() { ID = Convert.ToInt32(dttDatos.Rows[0]["SemanaFinal"].ToString()), ANO = Convert.ToInt32(dttDatos.Rows[0]["AnoFinal"].ToString()) };
            }
            else
            {
                this.objControlador.setCommand("ValoresPorDefectoAnio");
                this.objControlador.addNewParameter("@Ano", DateTime.Now.Year);
                DataTable dttDatosAnio = this.objControlador.execTableResult();
                if (dttDatosAnio.Rows.Count == 1)
                {
                    objAppnet.AnioSemanaInicial = new clsSemana() { ID = Convert.ToInt64(dttDatosAnio.Rows[0]["SemanaInicial"].ToString()), ANO = DateTime.Now.Year };
                    objAppnet.AnioSemanaFinal = new clsSemana() { ID = Convert.ToInt64(dttDatosAnio.Rows[0]["SemanaFinal"].ToString()), ANO = DateTime.Now.Year };
                }
                else
                {
                    objAppnet.AnioSemanaInicial = new clsSemana() { ANO = DateTime.Now.Year };
                    objAppnet.AnioSemanaFinal = new clsSemana() { ANO = DateTime.Now.Year };
                }
                this.objControlador.closeConnection();
            }
        }

        /// <summary>
        /// Metodo encargado de generar nomnbre automatico de ForeCast
        /// </summary>
        /// <param name="xIDBasTipoDatoPyG"></param>
        /// <param name="xSemanaInicial"></param>
        /// <param name="xSemanaFinal"></param>
        /// <param name="xUsuario"></param>
        /// <returns></returns>
        public string generaNombreAutomaticoFcts(int xIDBasTipoDatoPyG, int xSemanaInicial, int xSemanaFinal, long xUsuario)
        {
            this.objControlador.setCommand("GenerarNomobreForeCastAutomatico");
            this.objControlador.addNewParameter("@IDBasTipoDatoPyG", xIDBasTipoDatoPyG);
            this.objControlador.addNewParameter("@SemanaInicial", xSemanaInicial);
            this.objControlador.addNewParameter("@SemanaFinal", xSemanaFinal);
            this.objControlador.addNewParameter("@Usuario", xUsuario);
            string xstrSalida = (string)this.objControlador.execScalar();
            this.objControlador.closeConnection();
            return xstrSalida;
        }

        /// <summary>
        /// Metodo encargado de validar objeto de ForeCast Antes de guardar en la base de datos
        /// </summary>
        /// <param name="objForeCast"></param>
        /// <returns></returns>
        private List<string> validacionObjetoForeCast(clsAppNetProyeccionForeCast objForeCast)
        {
            List<string> cadenaValidacion = new List<string>();
            if (string.IsNullOrEmpty(objForeCast.Nombre))
            {
                cadenaValidacion.Add("Debe asignarle un nombre al ForeCast.");
            }
            if (objForeCast.Bas_TipoDatoPYG.ID <= 0)
            {
                cadenaValidacion.Add("Debe seleccionar un tipo de dato para el ForeCast.");
            }
            if (objForeCast.AnioSemanaInicial.ID <= 0)
            {
                cadenaValidacion.Add("Debe seleccionar una semana inicial valida.");
            }
            if (objForeCast.AnioSemanaFinal.ID <= 0)
            {
                cadenaValidacion.Add("Debe seleccionar una semana final valida.");
            }
            if (objForeCast.CargarAnteriorForeCast)
            {
                if (objForeCast.IDForeCastAnterior <= 0)
                {
                    cadenaValidacion.Add("Debe seleccionar un ForeCast de la lista.");
                }
            }
            return cadenaValidacion;
        }

        /// <summary>
        /// Metodo encargado de consultar ForeCast
        /// </summary>
        /// <param name="objForeCast"></param>
        /// <returns></returns>
        public clsResultadoJson guardarForeCast(clsAppNetProyeccionForeCast objForeCast)
        {
            clsResultadoJson _objresultado = new clsResultadoJson();
            List<string> lstMensajes = validacionObjetoForeCast(objForeCast);
            if (lstMensajes.Count == 0)
            {
                try
                {
                    this.objControlador.setCommand("InsertaModificaForeCast");
                    this.objControlador.addNewParameter("@ID", objForeCast.ID);
                    this.objControlador.addNewParameter("@Nombre", objForeCast.Nombre);
                    this.objControlador.addNewParameter("@Descripcion", clsGeneralBD.validaCadenaNUllAVacio(objForeCast.Descripcion));
                    this.objControlador.addNewParameter("@IDBas_TipoDatoPYG", objForeCast.Bas_TipoDatoPYG.ID);
                    this.objControlador.addNewParameter("@AnioSemanaInicial", objForeCast.AnioSemanaInicial.ID);
                    this.objControlador.addNewParameter("@AnioSemanaFinal", objForeCast.AnioSemanaFinal.ID);
                    this.objControlador.addNewParameter("@IDEstado", (objForeCast.EstadoObj == null ? new clsEstadoProyeccion() { ID = 1 } : objForeCast.EstadoObj).ID);
                    this.objControlador.addNewParameter("@Usuario", objForeCast.Usuariomodificacion.ID);
                    this.objControlador.addNewParameter("@IDTipoProceso", objForeCast.TipoProceso.ID);
                    int _Resultado = (int)this.objControlador.execScalar();
                    this.objControlador.closeConnection();
                    if (_Resultado != 0)
                    {
                        if (objForeCast.ID == 0)
                        {
                            _objresultado.MensajeProceso = "Proyección creada exitosamente con ID: " + _Resultado.ToString();
                            _objresultado.objResultado = _Resultado;
                        }
                        else
                        {
                            _objresultado.MensajeProceso = "Proyección modificada exitosamente con ID: " + _Resultado.ToString();
                            _objresultado.objResultado = _Resultado;
                        }
                    }
                    else
                    {
                        _objresultado.ResultadoProceso = false;
                        _objresultado.MensajeProceso = "Error inesperado";
                    }
                }
                catch (Exception ex)
                {
                    _objresultado.ResultadoProceso = false;
                    _objresultado.MensajeProceso = ex.Message;
                }
            }
            else
            {
                _objresultado.ResultadoProceso = false;
                _objresultado.objResultado = lstMensajes;
            }
            return _objresultado;
        }

        /// <summary>
        /// Metodo encargado de consultar listados basicos para a cargar de productos y clientes
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xOjbUsuario"></param>
        /// <returns></returns>
        public object ProductosyClientesProyectarNuevos(int xIDForeCast, string xCanal, int xCliente, string xProducto, clsAppNetUsuarios xOjbUsuario)
        {
            this.objControlador.setCommand("NuevosProductosProyeccionClientesProductosProyectar");
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
            this.objControlador.addNewParameter("@IDAppNetUsuario", xOjbUsuario.ID);
            this.objControlador.addNewParameter("@Canal", xCanal);
            this.objControlador.addNewParameter("@Cliente", xCliente);
            this.objControlador.addNewParameter("@Productos", xProducto);
            
            DataSet dtsDatosForeCast = this.objControlador.execDataSetResult();
            this.objControlador.closeConnection();

            DataTable dttCanales = dtsDatosForeCast.Tables[0];
            DataTable dttClientes = dtsDatosForeCast.Tables[1];
            DataTable dttProductos = dtsDatosForeCast.Tables[2];
            DataTable dttPresentaciones = dtsDatosForeCast.Tables[3];
            DataTable dttTemporadasBase = dtsDatosForeCast.Tables[4];
            DataTable dttDatosResumen = dtsDatosForeCast.Tables[5];

            List<object> lstCanales = new List<object>();
            List<object> lstClientes = new List<object>();
            List<object> lstProductos = new List<object>();
            List<object> lstPresentaciones = new List<object>();
            List<object> lstTemporadasBase = new List<object>();
            List<object> lstDatosResumen = new List<object>();

            ///Canales
            for (int i = 0; i < dttCanales.Rows.Count; i++)
            {
                lstCanales.Add(new { @Canal = dttCanales.Rows[i]["Canal"].ToString() });
            }
            ///Clientes
            for (int i = 0; i < dttClientes.Rows.Count; i++)
            {
                lstClientes.Add(new
                {
                    @ID = Convert.ToInt64(dttClientes.Rows[i]["IDCliente"].ToString()),
                    @Nombre = dttClientes.Rows[i]["NombreCliente"].ToString()
                });
            }
            ///Productos
            for (int i = 0; i < dttProductos.Rows.Count; i++)
            {
                lstProductos.Add(new
                {
                    @ID = Convert.ToInt64(dttProductos.Rows[i]["ID"].ToString()),
                    @Codigo = dttProductos.Rows[i]["Codigo"].ToString(),
                    @Nombre = dttProductos.Rows[i]["Nombre"].ToString(),
                    @Revision = dttProductos.Rows[i]["Revision"].ToString()
                });
            }
            ///Presentacion
            for (int i = 0; i < dttPresentaciones.Rows.Count; i++)
            {
                lstPresentaciones.Add(new
                {
                    @ID = Convert.ToInt64(dttPresentaciones.Rows[i]["ID"].ToString()),
                    @Nombre = dttPresentaciones.Rows[i]["Nombre"].ToString(),
                });
            }
            ///Temporada Base
            for (int i = 0; i < dttTemporadasBase.Rows.Count; i++)
            {
                lstTemporadasBase.Add(new
                {
                    @ID = Convert.ToInt64(dttTemporadasBase.Rows[i]["ID"].ToString()),
                    @Nombre = dttTemporadasBase.Rows[i]["Nombre"].ToString(),
                });
            }
            ///Resumen
            for (int i = 0; i < dttDatosResumen.Rows.Count; i++)
            {
                lstDatosResumen.Add(new
                {
                    @IDAppNetProyeccionForeCast = Convert.ToInt32(dttDatosResumen.Rows[i]["IDAppNetProyeccionForeCast"].ToString()),
                    @Canal = dttDatosResumen.Rows[i]["Canal"].ToString(),
                    @IDCliente = Convert.ToInt64(dttDatosResumen.Rows[i]["IDCliente"].ToString()),
                    @IDProductos = Convert.ToInt64(dttDatosResumen.Rows[i]["IDProductos"].ToString()),
                    @IDPresentacion = Convert.ToInt64(dttDatosResumen.Rows[i]["IDPresentacion"].ToString()),
                    @IDTemporadaBase = Convert.ToInt64(dttDatosResumen.Rows[i]["IDTemporadaBase"].ToString()),
                    @Seleccionado = Convert.ToBoolean(dttDatosResumen.Rows[i]["Seleccionado"].ToString()),
                    @Modificado = Convert.ToBoolean(dttDatosResumen.Rows[i]["Modificado"].ToString())
                });
            }

            return new
            {
                Canales = lstCanales,
                Clientes = lstClientes,
                Productos = lstProductos,
                Presentaciones = lstPresentaciones,
                TemporadasBase = lstTemporadasBase,
                DatosResumen = lstDatosResumen
            };
        }


        /// <summary>
        /// Metodo encargado de consultar listados basicos para a cargar de productos y clientes
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xOjbUsuario"></param>
        /// <returns></returns>
        public object ProductosyClientesProyectar(int xIDForeCast, clsAppNetUsuarios xOjbUsuario)
        {
            this.objControlador.setCommand("ConsultaListadoClientesProductosProyectar");
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
            this.objControlador.addNewParameter("@IDAppNetUsuario", xOjbUsuario.ID);
            DataSet dtsDatosForeCast = this.objControlador.execDataSetResult();
            this.objControlador.closeConnection();

            DataTable dttCanales = dtsDatosForeCast.Tables[0];
            DataTable dttClientes = dtsDatosForeCast.Tables[1];
            DataTable dttProductos = dtsDatosForeCast.Tables[2];
            DataTable dttPresentaciones = dtsDatosForeCast.Tables[3];
            DataTable dttTemporadasBase = dtsDatosForeCast.Tables[4];
            DataTable dttDatosResumen = dtsDatosForeCast.Tables[5];

            List<object> lstCanales = new List<object>();
            List<object> lstClientes = new List<object>();
            List<object> lstProductos = new List<object>();
            List<object> lstPresentaciones = new List<object>();
            List<object> lstTemporadasBase = new List<object>();
            List<object> lstDatosResumen = new List<object>();

            ///Canales
            for (int i = 0; i < dttCanales.Rows.Count; i++)
            {
                lstCanales.Add(new { @Canal = dttCanales.Rows[i]["Canal"].ToString() });
            }
            ///Clientes
            for (int i = 0; i < dttClientes.Rows.Count; i++)
            {
                lstClientes.Add(new
                {
                    @ID = Convert.ToInt64(dttClientes.Rows[i]["IDCliente"].ToString()),
                    @Nombre = dttClientes.Rows[i]["NombreCliente"].ToString()
                });
            }
            ///Productos
            for (int i = 0; i < dttProductos.Rows.Count; i++)
            {
                lstProductos.Add(new
                {
                    @ID = Convert.ToInt64(dttProductos.Rows[i]["ID"].ToString()),
                    @Codigo = dttProductos.Rows[i]["Codigo"].ToString(),
                    @Nombre = dttProductos.Rows[i]["Nombre"].ToString(),
                    @Revision = dttProductos.Rows[i]["Revision"].ToString()
                });
            }
            ///Presentacion
            for (int i = 0; i < dttPresentaciones.Rows.Count; i++)
            {
                lstPresentaciones.Add(new
                {
                    @ID = Convert.ToInt64(dttPresentaciones.Rows[i]["ID"].ToString()),
                    @Nombre = dttPresentaciones.Rows[i]["Nombre"].ToString(),
                });
            }
            ///Temporada Base
            for (int i = 0; i < dttTemporadasBase.Rows.Count; i++)
            {
                lstTemporadasBase.Add(new
                {
                    @ID = Convert.ToInt64(dttTemporadasBase.Rows[i]["ID"].ToString()),
                    @Nombre = dttTemporadasBase.Rows[i]["Nombre"].ToString(),
                });
            }
            ///Resumen
            for (int i = 0; i < dttDatosResumen.Rows.Count; i++)
            {
                lstDatosResumen.Add(new
                {
                    @IDAppNetProyeccionForeCast = Convert.ToInt32(dttDatosResumen.Rows[i]["IDAppNetProyeccionForeCast"].ToString()),
                    @Canal = dttDatosResumen.Rows[i]["Canal"].ToString(),
                    @IDCliente = Convert.ToInt64(dttDatosResumen.Rows[i]["IDCliente"].ToString()),
                    @IDProductos = Convert.ToInt64(dttDatosResumen.Rows[i]["IDProductos"].ToString()),
                    @IDPresentacion = Convert.ToInt64(dttDatosResumen.Rows[i]["IDPresentacion"].ToString()),
                    @IDTemporadaBase = Convert.ToInt64(dttDatosResumen.Rows[i]["IDTemporadaBase"].ToString()),
                    @Seleccionado = Convert.ToBoolean(dttDatosResumen.Rows[i]["Seleccionado"].ToString()),
                    @Modificado = Convert.ToBoolean(dttDatosResumen.Rows[i]["Modificado"].ToString())
                });
            }

            return new
            {
                Canales = lstCanales,
                Clientes = lstClientes,
                Productos = lstProductos,
                Presentaciones = lstPresentaciones,
                TemporadasBase = lstTemporadasBase,
                DatosResumen = lstDatosResumen
            };
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
        /// Metodo encargado de salvar los cambios realizados sobre el control de proyeccion
        /// </summary>
        /// <param name="xIdProyeccionVenta">ID unico de la proyeccion</param>
        /// <param name="xIDModificacion">ID modificación</param>
        public bool guardarCambiosProyeccion(int xIdProyeccionVenta, string xIDModificacion, clsAppNetUsuarios objUsuario)
        {
            try
            {
                this.objControlador.setCommand("GuardarCambiosProductosProyectadosSemanas");
                this.objControlador.addNewParameter("@IDProyeccionVenta", xIdProyeccionVenta);
                this.objControlador.addNewParameter("@IDModificacion", xIDModificacion);
                this.objControlador.addNewParameter("@IDUsuario", objUsuario.ID);
                this.objControlador.execScalar();
                this.objControlador.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                cargarErro(ex);
                return false;
            }
        }

        /// <summary>
        /// Metodo encargado de guardar modificaciones de productos a proyectar.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="lstItems"></param>
        /// <returns></returns>
        public clsResultadoJson guardarListadoItems(
            int xIDForeCast,
            List<clsclsAppNetProyeccionForeCast_ItemsSencillo> lstItems,
            clsAppNetUsuarios objUsuario)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("GuardarDetalleItemsModificados");
                this.objControlador.addNewParameter("@IdForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@IDUsuario", objUsuario.ID);
                this.objControlador.addNewParameter("@ItemsModificados", procesamientoObjetoAXML.listadoAXML(lstItems, "@ItemsModificados"));
                int xCantidadDatos = (int)this.objControlador.execScalar();
                this.objControlador.closeConnection();
                objResultado.objResultado = SHA.Encriptar(xIDForeCast.ToString());
                objResultado.ResultadoProceso = true;
            }
            catch (Exception ex)
            {
                objResultado.ResultadoProceso = false;
                objResultado.MensajeProceso = ex.Message;
            }
            return objResultado;
        }


        /// <summary>
        /// Metodo utiizado para crear la lista de movimientos forecast
        /// </summary>
        /// <param name="IDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Movimientos> consultaListadoMovimientosForeCast(long IDForeCast)
        {
            List<clsAppNetProyeccionForeCast_Movimientos> lstMovimientosForeCast = new List<clsAppNetProyeccionForeCast_Movimientos>();
            this.objControlador.setCommand("ConsultaMovimientosForeCast");
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", IDForeCast);
            DataTable dttDatos = this.objControlador.execTableResult();
            this.objControlador.closeConnection();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsAppNetProyeccionForeCast_Movimientos objItem = new clsAppNetProyeccionForeCast_Movimientos
                {
                    IDMovimiento = Convert.ToInt32(dttDatos.Rows[i]["IDMovimiento"].ToString()),
                    IDUsuario = Convert.ToInt32(dttDatos.Rows[i]["IDUsuario"].ToString()),
                    NombreUsuario = dttDatos.Rows[i]["NombreUsuario"].ToString(),
                    IDTipo = Convert.ToInt32(dttDatos.Rows[i]["IDTipo"].ToString()),
                    NombreTipo = dttDatos.Rows[i]["NombreTipo"].ToString(),
                    FechaMovimiento = Convert.ToDateTime(dttDatos.Rows[i]["FechaMovimiento"].ToString()),
                    Descripcion = dttDatos.Rows[i]["Descripción"].ToString()
                };
                lstMovimientosForeCast.Add(objItem);
            }
            return lstMovimientosForeCast;
        }


        /// <summary>
        /// Metodo encargado de insertar archivo en BD
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xNombre"></param>
        /// <param name="xCantidadBytes"></param>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public long insertarArchivoForeCast(
            int xIDForeCast,
            string xNombre,
            string xExtension,
            long xCantidadBytes,
            clsAppNetUsuarios objUsuario,
            int ddlTipoArchivo,
            string txtObservaciones,
            DateTime txtFechaArchivo
            )
        {
            try
            {
                this.objControlador.setCommand("InsertarArchivoNuevoForeCast");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                this.objControlador.addNewParameter("@Extension", xExtension);
                this.objControlador.addNewParameter("@TamanoArchivo", xCantidadBytes);
                this.objControlador.addNewParameter("@IDAppNetUsuarios", objUsuario.ID);
                this.objControlador.addNewParameter("@IDTipoArchivo", ddlTipoArchivo);
                this.objControlador.addNewParameter("@Observaciones", txtObservaciones);
                this.objControlador.addNewParameter("@FechaArchivo", txtFechaArchivo);
                long lValor = (long)this.objControlador.execScalar();
                this.objControlador.closeConnection();
                return lValor;
            }
            catch (Exception ex)
            {
                cargarErro(ex);
                return -1;
            }
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de archivos del forecast.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos> consultaListadoArchivos(long xIDForeCast)
        {
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = new List<clsAppNetProyeccionForeCast_Archivos>();
            try
            {
                this.objControlador.setCommand("consultarArchivosForeCast");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                DataTable dttArchivos = this.objControlador.execTableResult();
                for (int i = 0; i < dttArchivos.Rows.Count; i++)
                {
                    clsAppNetProyeccionForeCast_Archivos objArchivo = new clsAppNetProyeccionForeCast_Archivos();
                    objArchivo.ID = Convert.ToInt32(dttArchivos.Rows[i]["id"].ToString());
                    objArchivo.Nombre = dttArchivos.Rows[i]["Nombre"].ToString();
                    objArchivo.Observaciones = dttArchivos.Rows[i]["ObservacionesArchivo"].ToString();
                    objArchivo.FechaSubida = Convert.ToDateTime(dttArchivos.Rows[i]["FechaSubida"].ToString());
                    objArchivo.Fecha = Convert.ToDateTime(dttArchivos.Rows[i]["FechaArchivo"].ToString());
                    objArchivo.RutaCompleta = dttArchivos.Rows[i]["RutaCompleta"].ToString();
                    objArchivo.NombreEstado = dttArchivos.Rows[i]["Estado"].ToString();
                    objArchivo.SeEjecutaProceso = Convert.ToBoolean(dttArchivos.Rows[i]["SeEjecutaProceso"].ToString());
                    objArchivo.SeMuestraBtnError = Convert.ToBoolean(dttArchivos.Rows[i]["SeMuestraBtnError"].ToString());
                    objArchivo.NombreUsuarioSubida = dttArchivos.Rows[i]["NombreUsuario"].ToString();
                    objArchivo.TipoArchivo = dttArchivos.Rows[i]["NombreTipoArchivo"].ToString();
                    lstArchivos.Add(objArchivo);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
            return lstArchivos;
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de archivos del forecast.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos> consultaListadoArchivosConsolidados(long xIDForeCast)
        {
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = new List<clsAppNetProyeccionForeCast_Archivos>();
            try
            {
                this.objControlador.setCommand("consultarArchivosForeCast_Consolidados");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                DataTable dttArchivos = this.objControlador.execTableResult();
                for (int i = 0; i < dttArchivos.Rows.Count; i++)
                {
                    clsAppNetProyeccionForeCast_Archivos objArchivo = new clsAppNetProyeccionForeCast_Archivos();
                    objArchivo.ID = Convert.ToInt32(dttArchivos.Rows[i]["id"].ToString());
                    objArchivo.Nombre = dttArchivos.Rows[i]["Nombre"].ToString();
                    objArchivo.FechaSubida = Convert.ToDateTime(dttArchivos.Rows[i]["FechaCreacion"].ToString());
                    objArchivo.NombreUsuarioSubida = dttArchivos.Rows[i]["NombreUsuario"].ToString();
                    objArchivo.TipoArchivo = dttArchivos.Rows[i]["NombreTipoArchivo"].ToString();
                    objArchivo.cantidadCajas = Convert.ToInt64(dttArchivos.Rows[i]["CantidadCajasTotal"].ToString());
                    objArchivo.cantidadCajasArchivo = Convert.ToInt64(dttArchivos.Rows[i]["CantidadCajasTotalArchivo"].ToString());
                    objArchivo.cantidadTallos = Convert.ToInt64(dttArchivos.Rows[i]["CantidadTallosTotal"].ToString());
                    objArchivo.cantidadTallosArchivo = Convert.ToInt64(dttArchivos.Rows[i]["CantidadTallosTotalArchivo"].ToString());
                    lstArchivos.Add(objArchivo);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
            return lstArchivos;
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de archivos del forecast.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos> consultaListadoArchivosConsolidadosXCanal(long xIDForeCast, string xCanal)
        {
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = new List<clsAppNetProyeccionForeCast_Archivos>();
            try
            {
                this.objControlador.setCommand("consultarArchivosForeCast_ConsolidadosXCanal");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@Canal", xCanal);
                DataTable dttArchivos = this.objControlador.execTableResult();
                for (int i = 0; i < dttArchivos.Rows.Count; i++)
                {
                    clsAppNetProyeccionForeCast_Archivos objArchivo = new clsAppNetProyeccionForeCast_Archivos();
                    objArchivo.ID = Convert.ToInt32(dttArchivos.Rows[i]["id"].ToString());
                    objArchivo.Nombre = dttArchivos.Rows[i]["Nombre"].ToString();
                    objArchivo.FechaSubida = Convert.ToDateTime(dttArchivos.Rows[i]["FechaCreacion"].ToString());
                    objArchivo.NombreUsuarioSubida = dttArchivos.Rows[i]["NombreUsuario"].ToString();
                    objArchivo.TipoArchivo = dttArchivos.Rows[i]["NombreTipoArchivo"].ToString();
                    lstArchivos.Add(objArchivo);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
            return lstArchivos;
        }

        /// <summary>
        /// Metodo encargado de validar y cargar archivo
        /// </summary>
        /// <param name="xIDForeCast"></param>
        public void CargaYValidaArchivo(long xIDForeCast, clsAppNetUsuarios objUsuario)
        {
            try
            {
                this.objControlador.setCommand("pa_CargarYValidaArchivo");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast_Archivos", xIDForeCast);
                this.objControlador.addNewParameter("@IDUsuario", objUsuario.ID);
                this.objControlador.execNoResults();
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
        }


        /// <summary>
        /// Metodo encargado de validar y cargar archivo
        /// </summary>
        /// <param name="xIDForeCast"></param>
        public bool eliminaArchivos(string xstrIDArchivos)
        {
            try
            {
                this.objControlador.setCommand("EliminarArchivosPorID");
                this.objControlador.addNewParameter("@IDArchivos", xstrIDArchivos);
                DataTable dttDatos = this.objControlador.execTableResult();
                this.objControlador.closeConnection();
                /*ARCHIVOS A ELIMINAR*/
                string strCarpeta = clsConfiguracionConexion.carpetaServidorProduccion02;
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    string strArchivo = System.IO.Path.Combine(strCarpeta, dttDatos.Rows[i]["RutaCompleta"].ToString());
                    if (System.IO.File.Exists(strArchivo))
                    {
                        System.IO.File.Delete(strArchivo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
                return false;
            }
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de tipo de archivo
        /// </summary>
        /// <returns></returns>
        public SelectList ConsultaTipoArchivo()
        {
            this.objControlador.setCommand("ConsultaTipoArchivo");
            DataTable dttDatos = this.objControlador.execTableResult();
            List<SelectListItem> lstItems = new List<SelectListItem>();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = dttDatos.Rows[i]["Nombre"].ToString();
                objItem.Value = dttDatos.Rows[i]["ID"].ToString();
                lstItems.Add(objItem);
            }
            this.objControlador.closeConnection();
            return new SelectList(lstItems, "Value", "Text");
        }


        /// <summary>
        /// Metodo utilizadoo para consultar listado de archivos a consolidar
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos> consultaListadoArchivoWordClass(long xIDForeCast)
        {
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivosWC = new List<clsAppNetProyeccionForeCast_Archivos>();
            this.objControlador.setCommand("ConsultaListadoArchivosAConsolidarWorClass");
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsAppNetProyeccionForeCast_Archivos objArchvo = new clsAppNetProyeccionForeCast_Archivos();
                objArchvo.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                objArchvo.Nombre = "ID:" + objArchvo.ID.ToString() + ", Nombre: " + dttDatos.Rows[i]["Nombre"].ToString() + ", Fecha: " + Convert.ToDateTime(dttDatos.Rows[i]["FechaArchivo"].ToString()).ToString("MM/dd/yyyy");
                lstArchivosWC.Add(objArchvo);
            }

            this.objControlador.closeConnection();
            return lstArchivosWC;
        }

        /// <summary>
        /// Metodo utilizado para generar consolidado de archivos
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xIDArchivoPrincipal"></param>
        /// <returns></returns>
        public clsResultadoJson generarConsolidacionArchivo(
            long xIDForeCast,
            long xIDArchivoPrincipal,
            clsAppNetUsuarios xObjUsuario)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("consolidarArchivosForeCast");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast_Archivos", xIDArchivoPrincipal);
                this.objControlador.addNewParameter("@IDAppNetUsuario", xObjUsuario.ID);
                this.objControlador.execNoResults();
                this.objControlador.closeConnection();
                objResultado.objResultado = xIDForeCast;
            }
            catch (Exception ex)
            {
                objResultado.ResultadoProceso = false;
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Metodo encargdo de eliminar la consolidación de los archivos para el ForeCast.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public clsResultadoJson eliminarConsolidacionArchivos(long xIDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("eliminarArchivosForeCast_Consolidados");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.execNoResults();
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Metodo encargado de procesar solicitud de importación de datos al control de ForeCast
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="strCanal"></param>
        /// <param name="xSemanaInicial"></param>
        /// <param name="xSemanaFinal"></param>
        /// <param name="xTipoProceso"></param>
        /// <param name="xIDArchivoImportacion"></param>
        /// <returns></returns>
        public clsResultadoJson procesarImportacionDatos(
            long xIDForeCast,
            string strCanal,
            int xSemanaInicial,
            int xSemanaFinal,
            string xTipoProceso,
            long xIDArchivoImportacion,
            clsAppNetUsuarios objUsuario,
            string xIDModificacion,
            List<clsAppNetItemImportacion> lstItems
            )
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("importaDatosAProyeccion_" + xTipoProceso);
                this.objControlador.addNewParameter("@AppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@Canal", strCanal);
                this.objControlador.addNewParameter("@IDSemanaInicial", xSemanaInicial);
                this.objControlador.addNewParameter("@IDSemanaFinal", xSemanaFinal);
                this.objControlador.addNewParameter("@ArchivoImportacion", xIDArchivoImportacion);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                this.objControlador.addNewParameter("@IDModificacion", xIDModificacion);
                DataSet dtsDatos = this.objControlador.execDataSetResult();
                this.objControlador.closeConnection();

                DataTable dttDatos = dtsDatos.Tables[0];
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetItemImportacion objItem = new clsAppNetItemImportacion();
                    objItem.Canal = dttDatos.Rows[i]["Canal"].ToString();
                    objItem.IDCliente = Convert.ToInt32(dttDatos.Rows[i]["IDCliente"].ToString());
                    objItem.IDProductos = Convert.ToInt32(dttDatos.Rows[i]["IDProductos"].ToString());
                    objItem.IDPresentaciones = Convert.ToInt32(dttDatos.Rows[i]["IDPresentaciones"].ToString());
                    objItem.IDTemporadasBase = Convert.ToInt32(dttDatos.Rows[i]["IDTemporadasBase"].ToString());
                    objItem.IDSemana = Convert.ToInt32(dttDatos.Rows[i]["IDSemana"].ToString());
                    objItem.NombreSemana = dttDatos.Rows[i]["NombreSemana"].ToString();
                    objItem.CantidadCajas = Convert.ToDecimal(dttDatos.Rows[i]["CantidadCajas"].ToString());
                    lstItems.Add(objItem);
                }

                objResultado.MensajeProceso = dtsDatos.Tables[1].Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Metodo utilizado de consultar listado de validaciones del archivo
        /// </summary>
        /// <param name="xIDArchivoForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos_Validaciones> consultaListadoValidacioneArchivo(long xIDArchivoForeCast)
        {
            List<clsAppNetProyeccionForeCast_Archivos_Validaciones> lstValidaciones = new List<clsAppNetProyeccionForeCast_Archivos_Validaciones>();

            this.objControlador.setCommand("ConsultarValidacionesArchivoconErrores");
            this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast_Archivos", xIDArchivoForeCast);
            DataTable dttDatos = this.objControlador.execTableResult();
            this.objControlador.closeConnection();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                lstValidaciones.Add(new clsAppNetProyeccionForeCast_Archivos_Validaciones()
                {
                    ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString()),
                    Nombre = dttDatos.Rows[i]["MensajeValidacion"].ToString()
                });
            }

            return lstValidaciones;
        }

        /// <summary>
        /// Metodo encargadao de consultar llistado de ForeCast que se basan en los filtros
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <returns></returns>
        public List<clsAppNetConsolidacion_Detalle_ForeCast> consultaDetalleForeCastAConsolidar(
            clsAppNetProyeccionForeCast_Filtro objFiltro,
            clsAppNetUsuarios objUsuario,
            long xConsolidacion_Encabezado)
        {
            List<clsAppNetConsolidacion_Detalle_ForeCast> lstForeCast = new List<clsAppNetConsolidacion_Detalle_ForeCast>();
            this.objControlador.setCommand("AppNetProyeccionForeCast_Consulta_XConsolidar");
            this.objControlador.addNewParameter("@ID", objFiltro.IDF);
            this.objControlador.addNewParameter("@Nombre", clsGeneralBD.validaCadenaNUllAMenosUno(objFiltro.NombreF));
            this.objControlador.addNewParameter("@IDBas_TipoDatoPyG", objFiltro.BasTipoDatoPyGF.ID);
            this.objControlador.addNewParameter("@IDEstado", objFiltro.BasEstadoProyeccionF.ID);
            this.objControlador.addNewParameter("@IDUsuaio", objUsuario.ID);
            this.objControlador.addNewParameter("@AppNetConsolidacion_Encabezado", xConsolidacion_Encabezado);
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                DataRow dttFila = dttDatos.Rows[i];
                clsAppNetConsolidacion_Detalle_ForeCast objItem = new clsAppNetConsolidacion_Detalle_ForeCast();
                clsAppNetProyeccionForeCast objForeCast = new clsAppNetProyeccionForeCast();
                objForeCast.ID = Convert.ToInt64(dttFila["ID"].ToString());
                objForeCast.Nombre = dttFila["Nombre"].ToString();
                objForeCast.Descripcion = dttFila["Descripcion"].ToString();
                objForeCast.Bas_TipoDatoPYG = new clsTipoDatoPYG() { ID = Convert.ToInt64(dttFila["IDBas_TipoDatoPYG"].ToString()), Nombre = dttFila["Nombre_TipoDatoPyG"].ToString() };
                objForeCast.AnioSemanaInicial = new clsSemana() { ID = Convert.ToInt64(dttFila["AnioSemanaInicial"].ToString()), Nombre = dttFila["NombreAnioSemanaInicial"].ToString(), ANO = Convert.ToInt32(dttFila["AnioInicial"].ToString()) };
                objForeCast.AnioSemanaFinal = new clsSemana() { ID = Convert.ToInt64(dttFila["AnioSemanaFinal"].ToString()), Nombre = dttFila["NombreAnioSemanaFinal"].ToString(), ANO = Convert.ToInt32(dttFila["AnioFinal"].ToString()) };
                objForeCast.FechaCreacion = Convert.ToDateTime(dttFila["FechaCreacion"].ToString());
                objForeCast.FechaModificacion = Convert.ToDateTime(dttFila["FechaModificacion"].ToString());
                objForeCast.UsuarioCreacion = new clsAppNetUsuarios() { ID = Convert.ToInt64(dttFila["UsuarioCreacion"].ToString()), UserName = dttFila["UserNameCreacion"].ToString() };
                objForeCast.Usuariomodificacion = new clsAppNetUsuarios() { ID = Convert.ToInt64(dttFila["Usuariomodificacion"].ToString()), UserName = dttFila["UserNameModificacion"].ToString() };
                objForeCast.EstadoObj = new clsEstadoProyeccion() { ID = Convert.ToInt64(dttFila["IDEstado"].ToString()), Nombre = dttFila["NombreEstado"].ToString() };
                objForeCast.Consecutivo = Convert.ToInt32(dttFila["Consecutivo"].ToString());
                objForeCast.TipoProceso = new clsAppNetTipoProceso() { ID = Convert.ToInt64(dttFila["IDAppNetTipoProceso"].ToString()) };
                objForeCast.CargarAnteriorForeCast = Convert.ToBoolean(dttFila["CargarAnteriorForeCast"].ToString());
                objForeCast.IDForeCastAnterior = Convert.ToInt64(dttFila["IDForeCastAnterior"].ToString());
                objForeCast.DatosConsolidados = Convert.ToBoolean(dttFila["DatosConsolidados"].ToString());
                objItem.objAppNetProyeccionForeCast = objForeCast;
                objItem.Seleccionado = Convert.ToBoolean(dttFila["Seleccionado"].ToString());
                lstForeCast.Add(objItem);
            }

            this.objControlador.closeConnection();
            return lstForeCast;
        }

        /// <summary>
        /// Metodo encargado de validar objeto de ForeCast Antes de guardar en la base de datos
        /// </summary>
        /// <param name="objForeCast"></param>
        /// <returns></returns>
        private List<string> validacionObjetoConsolidadoForeCast(clsAppNetConsolidacion_Encabezado objForeCast)
        {
            List<string> cadenaValidacion = new List<string>();
            if (string.IsNullOrEmpty(objForeCast.Nombre))
            {
                cadenaValidacion.Add("Debe asignarle un nombre al ForeCast.");
            }
            if (objForeCast.lstForeCast.Count == 0)
            {
                cadenaValidacion.Add("Debe seleccionar un item a consolidar.");
            }
            return cadenaValidacion;
        }


        /// <summary>
        /// Procedimiento utilizado para guardar una consolidación del ForeCast
        /// </summary>
        /// <param name="objForeCast"></param>
        /// <returns></returns>
        public clsResultadoJson guardarConsolidadoForeCast(clsAppNetConsolidacion_Encabezado objForeCast)
        {
            clsResultadoJson _objresultado = new clsResultadoJson();
            List<string> lstMensajes = validacionObjetoConsolidadoForeCast(objForeCast);
            if (lstMensajes.Count == 0)
            {
                try
                {
                    this.objControlador.setCommand("InsertaModificaAppNetConsolidacion");
                    this.objControlador.addNewParameter("@ID", objForeCast.ID);
                    this.objControlador.addNewParameter("@Nombre", objForeCast.Nombre);
                    this.objControlador.addNewParameter("@Descripcion", clsGeneralBD.validaCadenaNUllAVacio(objForeCast.Descripcion));
                    this.objControlador.addNewParameter("@IDEstado", objForeCast.IDAppNetEstados);
                    this.objControlador.addNewParameter("@IDForeCast", objForeCast.lstForeCast[0].objAppNetProyeccionForeCast.ID);
                    this.objControlador.addNewParameter("@IDUsuario", objForeCast.IDAppNetUsuarioCreacion.ID);
                    int _Resultado = (int)this.objControlador.execScalar();
                    this.objControlador.closeConnection();
                    if (_Resultado != 0)
                    {
                        if (objForeCast.ID == -1)
                        {
                            _objresultado.MensajeProceso = "Consolidación carga exitosamente con ID: " + _Resultado.ToString();
                            _objresultado.objResultado = _Resultado;
                        }
                        else
                        {
                            _objresultado.MensajeProceso = "Consolidación modificada exitosamente con ID: " + _Resultado.ToString();
                            _objresultado.objResultado = _Resultado;
                        }
                    }
                    else
                    {
                        _objresultado.ResultadoProceso = false;
                        _objresultado.MensajeProceso = "Error inesperado";
                    }
                }
                catch (Exception ex)
                {
                    _objresultado.ResultadoProceso = false;
                    _objresultado.MensajeProceso = ex.Message;
                }
            }
            else
            {
                _objresultado.ResultadoProceso = false;
                _objresultado.objResultado = lstMensajes;
            }
            return _objresultado;
        }

        /// <summary>
        /// Metodo utilizado de consultar listado de consolidacion de ForeCast
        /// </summary>
        /// <param name="xID"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsAppNetConsolidacion_Encabezado> consultaListadoConsolidacionForeCast(clsAppNetProyeccionForeCast_Filtro objFiltro)
        {
            List<clsAppNetConsolidacion_Encabezado> lstEncabezados = new List<clsAppNetConsolidacion_Encabezado>();
            this.objControlador.setCommand("ConsultarConsolidadoForeCast");
            this.objControlador.addNewParameter("@ID", clsGeneralBD.validaEnteroCeroAMenosUno(objFiltro.IDF));
            this.objControlador.addNewParameter("@Nombre", clsGeneralBD.validaCadenaNUllAMenosUno(objFiltro.NombreF));
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                DataRow dttFila = dttDatos.Rows[i];
                clsAppNetConsolidacion_Encabezado objForeCast = new clsAppNetConsolidacion_Encabezado();
                objForeCast.ID = Convert.ToInt64(dttFila["ID"].ToString());
                objForeCast.Nombre = dttFila["Nombre"].ToString();
                objForeCast.Descripcion = dttFila["Descripcion"].ToString();
                objForeCast.FechaCreacion = Convert.ToDateTime(dttFila["FechaCreacion"].ToString());
                objForeCast.FechaUltimaModificacion = Convert.ToDateTime(dttFila["FechaUltimaModificacion"].ToString());
                objForeCast.IDAppNetUsuarioCreacion = new clsAppNetUsuarios()
                {
                    ID = Convert.ToInt32(dttFila["IDUsuarioCreacion"].ToString()),
                    Nombre = dttFila["NombreUsuario"].ToString()
                };
                objForeCast.IDAppNetEstados = Convert.ToInt32(dttFila["IDEstado"].ToString());
                objForeCast.NombreEstado = dttFila["NombreEstado"].ToString();
                objForeCast.IDAbcap_GeCo = Convert.ToInt32(dttFila["IDProyeccionVenta"].ToString());
                objForeCast.FechaExportacion = Convert.ToDateTime(dttFila["FechaExportacion_ProyeccionVenta"].ToString());

                lstEncabezados.Add(objForeCast);
            }

            this.objControlador.closeConnection();
            return lstEncabezados;
        }

        /// <summary>
        /// Metodo utilizado para eliminar el consolidado de forecast seleccionados
        /// </summary>
        /// <param name="xItemSeleccionados"></param>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public clsResultadoJson eliminarConsolidacionForeCast(string xItemSeleccionados, clsAppNetUsuarios objUsuario)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("EliminarConsolidacionForeCast");
                this.objControlador.addNewParameter("@ItemSeleccionados", xItemSeleccionados);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                this.objControlador.execNoResults();

                objResultado.MensajeProceso = "Ítems seleccionados eliminados correctamente.";
                objResultado.ResultadoProceso = true;
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Procedimiento utilizado para consolidar ForeCast.
        /// </summary>
        /// <param name="xIDConsolidacion"></param>
        public void consultaListadoConsolidacionForeCast(int xIDConsolidacion)
        {
            try
            {
                this.objControlador.setCommand("procesarConsolidacionCalcularTallos");
                this.objControlador.addNewParameter("@XIDConsolidacion", xIDConsolidacion);
                this.objControlador.execNoResults();
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de validaciones 
        /// </summary>
        /// <param name="xIDConsolidacion"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Archivos_Validaciones> consultaListadoValidacionConsolidacion(int xIDConsolidacion)
        {
            List<clsAppNetProyeccionForeCast_Archivos_Validaciones> lstValidaciones = new List<clsAppNetProyeccionForeCast_Archivos_Validaciones>();
            try
            {
                this.objControlador.setCommand("consultaListadoErroresConsolidacion");
                this.objControlador.addNewParameter("@AppNetConsolidacion_Encabezado", xIDConsolidacion);
                DataTable dttDatos = this.objControlador.execTableResult();

                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    lstValidaciones.Add(new clsAppNetProyeccionForeCast_Archivos_Validaciones()
                    {
                        Nombre = dttDatos.Rows[i]["MensajeValidacion"].ToString()
                    });
                }

                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstValidaciones;
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de clientes por canal
        /// </summary>
        /// <param name="xCanal"></param>
        /// <returns></returns>
        public List<clsCliente> consultaListadoClienteXCanal(String xCanal)
        {
            List<clsCliente> lstClientes = new List<clsCliente>();
            try
            {
                this.objControlador.setCommand("ConsultaClientesXCanal");
                this.objControlador.addNewParameter("@Canal", xCanal);
                DataTable dttDatos = this.objControlador.execTableResult();

                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    lstClientes.Add(new clsCliente()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDCliente"].ToString()),
                        Nombre = dttDatos.Rows[i]["NombreCliente"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstClientes;
        }

        /// <summary>
        /// Metodo utilizado para consultar productos sin proyectar
        /// </summary>
        /// <param name="xIDUnico"></param>
        /// <param name="xCLiente"></param>
        /// <param name="xCanal"></param>
        /// <param name="xFiltro"></param>
        /// <returns></returns>
        public List<clsProducto> consultarProductosSinProyectar(int xIDUnico, string xCLiente, string xCanal, string xFiltro)
        {
            List<clsProducto> lstProductos = new List<clsProducto>();
            try
            {
                this.objControlador.setCommand("ConsultaProductosSinUtilizar");
                this.objControlador.addNewParameter("@IDFOreCast", xIDUnico);
                this.objControlador.addNewParameter("@IDCliente", xCLiente);
                this.objControlador.addNewParameter("@Canal", xCanal);
                this.objControlador.addNewParameter("@Filtro", xFiltro);
                DataTable dttDatos = this.objControlador.execTableResult();

                for (int i = 0; i < dttDatos.Rows.Count; i++) {
                    lstProductos.Add(new clsProducto()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString()),
                        Nombre = dttDatos.Rows[i]["Nombre"].ToString(),
                        Codigo = dttDatos.Rows[i]["Codigo"].ToString(),
                        Revision = dttDatos.Rows[i]["Revision"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstProductos;
        }

        /// <summary>
        /// Metodo utilizado parao eliminar productos de proyeccion
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xCanal"></param>
        /// <param name="xCliente"></param>
        /// <param name="xProducto"></param>
        /// <param name="xOjbUsuario"></param>
        /// <returns></returns>
        public clsResultadoJson eliminarProductoDeProyeccion(int xIDForeCast, string xCanal, int xCliente, int xProducto, clsAppNetUsuarios xOjbUsuario)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("[eliminarProductoClienteProyectar]");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                this.objControlador.addNewParameter("@IDAppNetUsuario", xOjbUsuario.ID);
                this.objControlador.addNewParameter("@Canal", xCanal);
                this.objControlador.addNewParameter("@Cliente", xCliente);
                this.objControlador.addNewParameter("@Productos", xProducto);
                this.objControlador.execDataSetResult();
                this.objControlador.closeConnection();
            }
            catch (Exception ex) {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Metodo utilizado para llenar dataset con datos de productos proyectados
        /// </summary>
        /// <param name="hdfIDForeCsa"></param>
        /// <param name="hdfTipoArchivo"></param>
        /// <param name="ddlTipoExportacionArchivo"></param>
        /// <returns></returns>
        public DataTable consultaDatosForeCastExcel(clsAppNetUsuarios objUsuario,long hdfIDForeCsa, string hdfTipoArchivo, string ddlTipoExportacionArchivo) {
            DataTable dttDatos;
            try
            {
                this.objControlador.setCommand("GenerarDocumentoExcel_" + hdfTipoArchivo + "_" + ddlTipoExportacionArchivo);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", hdfIDForeCsa);
                dttDatos = this.objControlador.execTableResult();
                this.objControlador.closeConnection();
            }
            catch (Exception ex) {
                cargarErro(ex);
                dttDatos = null;
            }
            return dttDatos;
        }

        /// <summary>
        /// Metodo utilizado para llenar dataset con datos de productos proyectados
        /// </summary>
        /// <param name="hdfIDForeCsa"></param>
        /// <param name="hdfTipoArchivo"></param>
        /// <param name="ddlTipoExportacionArchivo"></param>
        /// <returns></returns>
        public List<clsResumenGenerarForeCast> consultaResumenGeneralForeCast(long xIdForeCast, clsAppNetUsuarios objUsuario) {
            List<clsResumenGenerarForeCast> lstResumenGeneral = new List<clsResumenGenerarForeCast>();
            try
            {
                this.objControlador.setCommand("consultaForeCastPorCanal");
                this.objControlador.addNewParameter("@IDProyeccionVenta", xIdForeCast);
                this.objControlador.addNewParameter("@IDAppNetUsurio", objUsuario.ID);
                DataTable dttDatos = this.objControlador.execTableResult();
                this.objControlador.closeConnection();

                for (int i = 0; i < dttDatos.Rows.Count; i++) {
                    clsResumenGenerarForeCast objItem = new clsResumenGenerarForeCast();
                    objItem.Canal = dttDatos.Rows[i]["Canal"].ToString();
                    objItem.Anio = Convert.ToInt32(dttDatos.Rows[i]["Ano"].ToString());
                    objItem.CantidadCajas = Convert.ToDecimal(dttDatos.Rows[i]["CantidadCajas"].ToString());
                    objItem.CantidadCajasArch = Convert.ToDecimal(dttDatos.Rows[i]["CantidadCajasArch"].ToString());
                    objItem.DiferenciaCajasArch = Convert.ToDecimal(dttDatos.Rows[i]["DiferenciaCajasArch"].ToString());
                    objItem.CantidadTallos = Convert.ToDecimal(dttDatos.Rows[i]["CantidadTallos"].ToString());
                    objItem.CantidadTallosArch = Convert.ToDecimal(dttDatos.Rows[i]["CantidadTallosArch"].ToString());
                    objItem.DiferenciaTallosArch = Convert.ToDecimal(dttDatos.Rows[i]["DiferenciaTallosArch"].ToString());
                    objItem.CantidadSinCruzar = Convert.ToDecimal(dttDatos.Rows[i]["CantidadSinCruzar"].ToString());
                    objItem.CantidadSinCruzarArch = Convert.ToDecimal(dttDatos.Rows[i]["CantidadSinCruzarArch"].ToString());
                    lstResumenGeneral.Add(objItem);
                }

            }
            catch (Exception ex) {
                cargarErro(ex);
            }
            return lstResumenGeneral;
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de items del forecast
        /// </summary>
        /// <param name="xCanal"></param>
        /// <param name="xAnio"></param>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public List<clsAppNetProyeccionForeCast_Items> consultaListadoInconsistencias(string xCanal, int xAnio, long xIDForeCast) {
            List<clsAppNetProyeccionForeCast_Items> lstItems = new List<clsAppNetProyeccionForeCast_Items>();
            try
            {
                this.objControlador.setCommand("ConsultaInconsistenciaPorCanalAnio");
                this.objControlador.addNewParameter("@Canal", xCanal);
                this.objControlador.addNewParameter("@Anio", xAnio);
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIDForeCast);
                DataTable dttDatos = this.objControlador.execTableResult();
                this.objControlador.closeConnection();

                for (int i = 0; i < dttDatos.Rows.Count; i++) {
                    clsAppNetProyeccionForeCast_Items objItem = new clsAppNetProyeccionForeCast_Items();

                    DataRow dtrDato = dttDatos.Rows[i];
                    objItem.Canal = new clsCanal() { Nombre = dtrDato["Canal"].ToString() };
                    objItem.Cliente = new clsCliente() { ID = Convert.ToInt32(dtrDato["IDCliente"].ToString()), Nombre = dtrDato["NombreCliente"].ToString() };
                    objItem.Productos = new clsProducto() { Codigo = dtrDato["CodigoProducto"].ToString(), Nombre = dtrDato["NombreProducto"].ToString() };
                    objItem.Presentacion = new clsPresentaciones() { Codigo = dtrDato["CodigoProducto"].ToString(), Nombre = dtrDato["NombreProducto"].ToString() };
                    objItem.Codigo = dtrDato["CodigoTemporadaBase"].ToString();
                    objItem.Nombre = dtrDato["NombreTemporadaBase"].ToString();
                    
                    lstItems.Add(objItem);
                }

            }
            catch (Exception ex) {
                cargarErro(ex);
            }
            return lstItems;
        }

        /// <summary>
        /// Metodo utilizado para exportar datos a gestion comercial
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public clsResultadoJson exportarDatosGestionComercial(long xIdForeCast) {
            clsResultadoJson objResultadoJson = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("ExportarDatosProyeccionVenta");
                this.objControlador.addNewParameter("@IDAppNetProyeccionForeCast", xIdForeCast);
                DataTable dttDatos = this.objControlador.execTableResult();

                int xValorSalida = Convert.ToInt32(dttDatos.Rows[0]["Resultado"].ToString());
                objResultadoJson.ResultadoProceso = xValorSalida == 1;
                objResultadoJson.MensajeProceso = dttDatos.Rows[0]["CadenaSalida"].ToString();

                dttDatos.Dispose();
                this.objControlador.closeConnection();
            }
            catch (Exception ex) {
                objResultadoJson.cargarErro(ex);
            }
            return objResultadoJson;
        }

    }
}