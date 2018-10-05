using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using GestionDocumental.Models.Clases.Radicacion;
using GestionDocumental.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;



namespace GestionDocumental.Models.ControladorDatos
{
    public class clsRadicacionBD : clsConfgiruacionBD
    {

        /// <summary>
        /// Metodo utilizado para consultar estado
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <param name="xIDEstado"></param>
        /// <returns></returns>
        public clsAppNetFlujoEstados consultaEstado(clsAppNetUsuarios objUsuario, int xIDEstado)
        {
            clsAppNetFlujoEstados objEstado = null;
            try
            {
                this.objControlador.setCommand("Estados_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@ID", xIDEstado);
                DataSet dtsDatos = this.objControlador.execDataSetResult();
                this.objControlador.closeConnection();

                DataTable dttEstados = dtsDatos.Tables[0];

                if (dttEstados.Rows.Count > 0)
                {
                    objEstado = new clsAppNetFlujoEstados();
                    objEstado.ID = Convert.ToInt32(dttEstados.Rows[0]["ID"].ToString());
                    objEstado.Nombre = dttEstados.Rows[0]["Nombre"].ToString();
                    objEstado.TextoBtn = dttEstados.Rows[0]["TextoBtn"].ToString();


                    DataTable dttAcciones = dtsDatos.Tables[1];
                    for (int i = 0; i < dttAcciones.Rows.Count; i++)
                    {
                        clsAppNetFlujoEstados_Acciones objAccion = new clsAppNetFlujoEstados_Acciones();
                        objAccion.ID = Convert.ToInt32(dttAcciones.Rows[i]["ID"].ToString());
                        objAccion.Nombre = dttAcciones.Rows[i]["Nombre"].ToString();
                        objAccion.IDAppNetFlujoEstados_Sigiente = Convert.ToInt32(dttAcciones.Rows[i]["IDAppNetFlujoEstados_Sigiente"].ToString());
                        objEstado.lstAcciones.Add(objAccion);
                    }

                    dttEstados.Dispose();
                    dttAcciones.Dispose();
                    dtsDatos.Dispose();
                }
                else
                {
                    throw new Exception("No existe el estado indicado.");
                }
            }
            catch (Exception ex)
            {
                this.cargarErro(ex);
            }
            return objEstado;
        }

        /// <summary>
        /// Listado de tipos de documentos
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public List<clsAppNetTipoDocumentos> consultaListadoTipoDocumento(clsAppNetUsuarios objUsuario)
        {
            List<clsAppNetTipoDocumentos> lstTipoDocumento = new List<clsAppNetTipoDocumentos>();
            try
            {
                this.objControlador.setCommand("TiposDocumento_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                DataTable dttDatos = this.objControlador.execTableResult();

                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetTipoDocumentos objTipoDocumento = new clsAppNetTipoDocumentos();
                    objTipoDocumento.ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString());
                    objTipoDocumento.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    objTipoDocumento.conConsecutivoAutomatico = Convert.ToBoolean(dttDatos.Rows[i]["conConsecutivoAutomatico"].ToString());
                    lstTipoDocumento.Add(objTipoDocumento);
                }

                this.objControlador.closeConnection();
                dttDatos.Dispose();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstTipoDocumento;
        }

        /// <summary>
        /// Metodo utilizado para guardar archivo temporal
        /// </summary>
        /// <param name="objSoporte"></param>
        public decimal guardarArchivoTemporal(clsAppNetUsuarios objUsuario, clsAppNetDocumento_Soporte objSoporte)
        {
            try
            {
                this.objControlador.setCommand("AppNetDocumentos_Archivos_tmp_Insertar");
                this.objControlador.addNewParameter("@IDAppNetDocumentos", objSoporte.guidDocumento);
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                this.objControlador.addNewParameter("@Nombre", objSoporte.Nombre);
                this.objControlador.addNewParameter("@Nombre_MeDato", objSoporte.SoloNombre);
                this.objControlador.addNewParameter("@Extension_MeDato", objSoporte.Extension);
                this.objControlador.addNewParameter("@Tamanio", objSoporte.Tamanio);
                this.objControlador.addNewParameter("@Ubicacion", objSoporte.Nombre);
                this.objControlador.addNewParameter("@Ubicacion_NombreArchivo", objSoporte.NuevoNombre);
                return (decimal)this.objControlador.execScalar();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
                return -1;
            }
        }

        /// <summary>
        /// Metodo utilizado para consultar el tipo de archivo
        /// </summary>
        /// <param name="xstrExtension"></param>
        /// <returns></returns>
        public int consultaTipoArchivo(string xstrExtension)
        {
            try
            {
                this.objControlador.setCommand("AppNetDocumentos_Archivos_ContenType_Consulta");
                this.objControlador.addNewParameter("@Extension", xstrExtension);
                return (int)this.objControlador.execScalar();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
                return 0;
            }
        }

        /// <summary>
        /// Valida el número de documento
        /// </summary>
        /// <param name="objDocumento"></param>
        /// <returns></returns>
        private List<string> validaNumeroDocumento(clsAppNetUsuarios objUsuario, clsAppNetDocumentos objDocumento)
        {
            List<string> lstItems = new List<string>();
            this.objControlador.setCommand("[AppNetDocumentos_Guardar_validacion]");
            this.objControlador.addNewParameter("@ID", objDocumento.ID);
            this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
            this.objControlador.addNewParameter("@IDAppNetFlujoEstados", objDocumento.AppNetFlujoEstados.ID);
            this.objControlador.addNewParameter("@IDAppNetFlujoEstados_Accion", objDocumento.IDSiguienteAccion);
            this.objControlador.addNewParameter("@IDAppNetTipoDocumentos", objDocumento.AppNetTipoDocumentos.ID);
            this.objControlador.addNewParameter("@IDAppNetEmisor", objDocumento.IDAppNetEmisor.ID);
            this.objControlador.addNewParameter("@NumeroDocumento", objDocumento.NumeroDocumento);
            this.objControlador.addNewParameter("@FechaDocumento", objDocumento.FechaDocumento);
            this.objControlador.addNewParameter("@FechaRecepcion", objDocumento.FechaRecepcion);
            this.objControlador.addNewParameter("@Nota", clsGeneralBD.validaCadenaNUllAVacio(objDocumento.Nota));
            this.objControlador.addNewParameter("@IDAppNetUsuariosCreacion", objUsuario.ID);
            this.objControlador.addNewParameter("@GuiDUnicoArchivos", objDocumento.getIDUnicoObjeto);
            this.objControlador.addNewParameter("@IDArchivosEliminados", clsGeneralBD.validaCadenaNUllAVacio(objDocumento.xIDArchivosEliminados));
            DataTable dttDatos = this.objControlador.execTableResult();
            for (var i = 0; i < dttDatos.Rows.Count; i++)
            {
                lstItems.Add(dttDatos.Rows[i]["Validaciones"].ToString());
            }
            return lstItems;
        }

        /// <summary>
        /// Proceso utilizado en validar datos
        /// </summary>
        /// <param name="objDocumento"></param>
        /// <returns></returns>
        private List<string> validaDatosDocumento(clsAppNetUsuarios objUsuario, clsAppNetDocumentos objDocumento)
        {
            List<string> lstValidacione = new List<string>();
            if (objDocumento.IDSiguienteAccion == -1)
            {
                lstValidacione.Add("Debe seleccionar una opción.");
            }
            if (objDocumento.AppNetTipoDocumentos == null)
            {
                lstValidacione.Add("Debe seleccionar un tipo de documento.");
            }
            if (string.IsNullOrEmpty(clsGeneralBD.validaCadenaNUllAVacio(objDocumento.NumeroDocumento).Trim()))
            {
                lstValidacione.Add("Debe ingresar un número de documento.");
            }
            if (objDocumento.FechaDocumento == DateTime.MinValue)
            {
                lstValidacione.Add("Debe indicar una fecha para el documento.");
            }
            if (objDocumento.FechaRecepcion == DateTime.MinValue)
            {
                lstValidacione.Add("Debe indicar una fecha de recepción.");
            }
            lstValidacione.AddRange(validaNumeroDocumento(objUsuario, objDocumento));
            return lstValidacione;
        }

        /// <summary>
        /// Metodo utilizado para guardar/modificar documento
        /// </summary>
        /// <returns></returns>
        public clsResultadoJson Guardar(clsAppNetUsuarios objUsuario, clsAppNetDocumentos objDocumento, System.Web.HttpServerUtilityBase objControl)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                List<string> lstValidacione = validaDatosDocumento(objUsuario, objDocumento);
                if (lstValidacione.Count == 0)
                {
                    this.objControlador.setCommand("AppNetDocumentos_Guardar");
                    this.objControlador.addNewParameter("@ID", objDocumento.ID);
                    this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                    this.objControlador.addNewParameter("@IDAppNetFlujoEstados", objDocumento.AppNetFlujoEstados.ID);
                    this.objControlador.addNewParameter("@IDAppNetFlujoEstados_Accion", objDocumento.IDSiguienteAccion);
                    this.objControlador.addNewParameter("@IDAppNetTipoDocumentos", objDocumento.AppNetTipoDocumentos.ID);
                    this.objControlador.addNewParameter("@IDAppNetEmisor", objDocumento.IDAppNetEmisor.ID);
                    this.objControlador.addNewParameter("@NumeroDocumento", objDocumento.NumeroDocumento);
                    this.objControlador.addNewParameter("@FechaDocumento", objDocumento.FechaDocumento);
                    this.objControlador.addNewParameter("@FechaRecepcion", objDocumento.FechaRecepcion);
                    this.objControlador.addNewParameter("@Nota", clsGeneralBD.validaCadenaNUllAVacio(objDocumento.Nota));
                    this.objControlador.addNewParameter("@IDAppNetUsuariosCreacion", objUsuario.ID);
                    this.objControlador.addNewParameter("@GuiDUnicoArchivos", objDocumento.getIDUnicoObjeto);
                    this.objControlador.addNewParameter("@IDArchivosEliminados", clsGeneralBD.validaCadenaNUllAVacio(objDocumento.xIDArchivosEliminados));
                    DataSet dtsDatos = this.objControlador.execDataSetResult();
                    if (dtsDatos.Tables[0].Rows.Count > 0)
                    {
                        DataTable dttDatos = dtsDatos.Tables[0];
                        objResultado.MensajeProceso = dttDatos.Rows[0]["Mensaje"].ToString();
                        objDocumento.ID = Convert.ToInt64(dttDatos.Rows[0]["ID"].ToString());

                        string xstrCarpeta = clsConfiguracionAPP.ubicacionApp(objControl);
                        string xstrAdministrador = objUsuario.IDAdministrador.ToString();

                        string xstrCarpetaArchivadora = Path.Combine(xstrCarpeta, clsConfiguracionAPP.getCarpetaArchivos());

                        /*CREA CARPETA DE ADMINISTRADOR*/
                        if (!Directory.Exists(xstrCarpetaArchivadora))
                        {
                            Directory.CreateDirectory(xstrCarpetaArchivadora);
                        }

                        string xstrCarpetaAdministrador = Path.Combine(xstrCarpetaArchivadora, xstrAdministrador);

                        /*CREA CARPETA DE ADMINISTRADOR*/
                        if (!Directory.Exists(xstrCarpetaAdministrador))
                        {
                            Directory.CreateDirectory(xstrCarpetaAdministrador);
                        }

                        /*CREA CARPETA DE ADMINISTRADOR*/
                        if (!Directory.Exists(xstrCarpetaAdministrador))
                        {
                            Directory.CreateDirectory(xstrCarpetaAdministrador);
                        }

                        /*CREA CARPETA DE DOCUMENTO*/
                        string xstrDocumento = objDocumento.ID.ToString();
                        string xstrCarpetaDocumento = Path.Combine(xstrCarpetaAdministrador, xstrDocumento);
                        if (!Directory.Exists(xstrCarpetaDocumento))
                        {
                            Directory.CreateDirectory(xstrCarpetaDocumento);
                        }

                        string xstrCarpetaTemporal = Path.Combine(xstrCarpeta, clsConfiguracionAPP.getCarpetaTemporal());

                        /*ARCHIVOS NUEVOS*/
                        DataTable dttArchivos = dtsDatos.Tables[1];
                        for (int i = 0; i < dttArchivos.Rows.Count; i++)
                        {
                            string xstrArchivo = Path.Combine(xstrCarpetaTemporal, dttArchivos.Rows[i]["Ubicacion_NombreArchivo"].ToString() + dttArchivos.Rows[i]["Extension_MeDato"].ToString());
                            if (File.Exists(xstrArchivo))
                            {
                                File.Move(xstrArchivo, Path.Combine(xstrCarpetaDocumento, dttArchivos.Rows[i]["Ubicacion_NombreArchivo"].ToString() + dttArchivos.Rows[i]["Extension_MeDato"].ToString()));
                            }
                            else
                            {
                                objResultado.MensajeProceso += "<br\\>No existe el archivo temporal.";
                            }
                        }

                        /*ARCHIVOS ELIMINADOS*/
                        DataTable dttArchivosGuardados = dtsDatos.Tables[2];
                        for (int i = 0; i < dttArchivosGuardados.Rows.Count; i++)
                        {
                            string xstrArchivo = Path.Combine(xstrCarpetaDocumento, dttArchivosGuardados.Rows[i]["Ubicacion_NombreArchivo"].ToString() + dttArchivosGuardados.Rows[i]["Extension_MeDato"].ToString());
                            if (File.Exists(xstrArchivo))
                            {
                                File.Delete(xstrArchivo);
                            }
                            else
                            {
                                objResultado.MensajeProceso += "<br\\>No existe el archivo a eliminar.";
                            }
                        }

                        objResultado.objResultado = objDocumento.ID;

                    }
                    else
                    {
                        objResultado.ResultadoProceso = false;
                        objResultado.MensajeProceso = "El proceso no respondio correctamente.";
                    }
                    dtsDatos.Dispose();
                    this.objControlador.closeConnection();
                }
                else
                {
                    objResultado.MensajeProceso = lstValidacione[0];
                    objResultado.ResultadoProceso = false;
                }
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

        /// <summary>
        /// Consulta listado de documentos
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetDocumentos> consultaListadoDocumentos(clsAppNetUsuarios objUsuario, clsAppNetDocumentos_Filtros objFiltro)
        {
            List<clsAppNetDocumentos> lstDocumentos = new List<clsAppNetDocumentos>();
            try
            {
                this.objControlador.setCommand("AppNetDocumentos_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@xBID", clsGeneralBD.validaEnteroCeroAMenosUno(objFiltro.xBID));
                this.objControlador.addNewParameter("@xBIDTipoDocumento", clsGeneralBD.validaEnteroCeroAMenosUno(objFiltro.xBIDTipoDocumento));
                this.objControlador.addNewParameter("@xBIDEstadoDocumento", clsGeneralBD.validaEnteroCeroAMenosUno(objFiltro.xBIDEstadoDocumento));
                this.objControlador.addNewParameter("@xBNumeroDocumento", clsGeneralBD.validaCadenaNUllAMenosUno(objFiltro.xBNumeroDocumento));
                this.objControlador.addNewParameter("@xBFechaDocumento", clsGeneralBD.validaFechaBDNUll(objFiltro.xBFechaDocumento));
                this.objControlador.addNewParameter("@xBFechaRecepcion", clsGeneralBD.validaFechaBDNUll(objFiltro.xBFechaRecepcion));
                this.objControlador.addNewParameter("@xBNota", clsGeneralBD.validaCadenaNUllAMenosUno(objFiltro.xBNota));
                this.objControlador.addNewParameter("@xFechaCreacion", clsGeneralBD.validaFechaBDNUll(objFiltro.xBFechaCreacion));
                this.objControlador.addNewParameter("@IDUsuarioConsulta", objUsuario.ID);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                DataTable dttDatos = dtsDatos.Tables[0];
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetDocumentos objDocumento = new clsAppNetDocumentos();
                    objDocumento.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                    objDocumento.IDAppNetAdministrador = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetAdministrador"].ToString());
                    objDocumento.AppNetFlujoEstados = new clsAppNetFlujoEstados()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetFlujoEstados"].ToString()),
                        Nombre = dttDatos.Rows[i]["IDAppNetFlujoEstados_Nombre"].ToString()
                    };
                    objDocumento.AppNetTipoDocumentos = new clsAppNetTipoDocumentos()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetTipoDocumentos"].ToString()),
                        Nombre = dttDatos.Rows[i]["IDAppNetTipoDocumentos_Nombre"].ToString()
                    };
                    objDocumento.IDAppNetEmisor = new clsAppNetEmisor()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetEmisor"].ToString()),
                        Nombre = dttDatos.Rows[i]["NombreEmisor"].ToString()
                    };
                    objDocumento.NumeroDocumento = dttDatos.Rows[i]["NumeroDocumento"].ToString();
                    objDocumento.FechaDocumento = Convert.ToDateTime(dttDatos.Rows[i]["FechaDocumento"].ToString());
                    objDocumento.FechaRecepcion = Convert.ToDateTime(dttDatos.Rows[i]["FechaRecepcion"].ToString());
                    objDocumento.Nota = dttDatos.Rows[i]["Nota"].ToString();
                    objDocumento.FechaCreacion = Convert.ToDateTime(dttDatos.Rows[i]["FechaCreacion"].ToString());
                    objDocumento.AppNetUsuariosCreacion = new clsAppNetUsuarios()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetUsuariosCreacion"].ToString()),
                        Nombre = dttDatos.Rows[i]["IDAppNetUsuariosCreacion_Nombre"].ToString()
                    };
                    objDocumento.FechaModificacion = Convert.ToDateTime(dttDatos.Rows[i]["FechaModificacion"].ToString());
                    objDocumento.AppNetUsuariosModificacion = new clsAppNetUsuarios()
                    {
                        ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetUsuariosModificacion"].ToString()),
                        Nombre = dttDatos.Rows[i]["IDAppNetUsuariosModificacion_Nombre"].ToString()
                    };

                    /*Carga listado de archivos abjuntos*/
                    List<DataRow> lstSoportes = dtsDatos.Tables[1].Select("IDAppNetDocumentos = " + objDocumento.ID.ToString()).ToList();
                    for (int j = 0; j < lstSoportes.Count; j++)
                    {
                        DataRow dtrDato = lstSoportes[j];
                        clsAppNetDocumento_Soporte objSoporte = new clsAppNetDocumento_Soporte();
                        objSoporte.ID = Convert.ToInt64(dtrDato["ID"].ToString());
                        objSoporte.Nombre = dtrDato["Nombre"].ToString();
                        objSoporte.SoloNombre = dtrDato["Nombre_MeDato"].ToString();
                        objSoporte.Extension = dtrDato["Extension_MeDato"].ToString();
                        objSoporte.Tamanio = Convert.ToInt32(dtrDato["Tamanio"].ToString());
                        objSoporte.fechaSubida = Convert.ToDateTime(dtrDato["Fecha_Subida"].ToString());
                        objSoporte.Ubicacion = dtrDato["Ubicacion"].ToString();
                        objSoporte.Ubicacion_NombreArchivo = dtrDato["Ubicacion_NombreArchivo"].ToString();
                        objSoporte.Temporal = false;
                        objSoporte.ContendorImagen = Convert.ToInt32(dtrDato["ContendorImagen"].ToString());
                        objDocumento.lstSoportes.Add(objSoporte);
                    }

                    lstDocumentos.Add(objDocumento);

                }
                dttDatos.Dispose();
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstDocumentos;
        }

        /// <summary>
        /// Metodo utilizado para consulta archivo
        /// </summary>
        /// <param name="xIDArchivo"></param>
        /// <param name="xTipoArchivo"></param>
        /// <returns></returns>
        public List<clsAppNetDocumento_Soporte> consultaSoporteStream(long xIDArchivo, bool xTipoArchivo)
        {
            List<clsAppNetDocumento_Soporte> lstSoporte = new List<clsAppNetDocumento_Soporte>();
            this.objControlador.setCommand("AppNetDocumentos_Archivos_ConsultaArchivoStream");
            this.objControlador.addNewParameter("@IDArchivo", xIDArchivo);
            this.objControlador.addNewParameter("@IDTipoArchivo", xTipoArchivo);
            DataTable dttArchivos = this.objControlador.execTableResult();

            for (int i = 0; i < dttArchivos.Rows.Count; i++)
            {
                clsAppNetDocumento_Soporte objSoporte = new clsAppNetDocumento_Soporte();
                objSoporte.Nombre = dttArchivos.Rows[i]["Nombre"].ToString();
                if (xTipoArchivo == false)
                {
                    objSoporte.IDAppNetDocumentos = Convert.ToInt64(dttArchivos.Rows[i]["IDAppNetDocumentos"].ToString());
                }
                objSoporte.IDAppNetAdministrador = Convert.ToInt32(dttArchivos.Rows[i]["IDAppNetAdministrador"].ToString());
                objSoporte.Ubicacion = dttArchivos.Rows[i]["UbicacionArchivo"].ToString();
                objSoporte.ContentType = dttArchivos.Rows[i]["ContentType"].ToString();
                lstSoporte.Add(objSoporte);
            }

            dttArchivos.Dispose();
            this.objControlador.closeConnection();
            return lstSoporte;
        }

        /// <summary>
        /// Consulta listado de movimientos del documento
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetDocumentos_Movimientos> consultaListadoMovimientos(clsAppNetUsuarios objUsuario, long xIDDocumento)
        {
            List<clsAppNetDocumentos_Movimientos> lstMovimientos = new List<clsAppNetDocumentos_Movimientos>();
            try
            {
                this.objControlador.setCommand("AppNetDocumentos_Movimientos_Consulta");
                this.objControlador.addNewParameter("@IDAppNetDocumentos", xIDDocumento);
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                DataTable dttDatos = this.objControlador.execTableResult();

                for (int i = 0; dttDatos.Rows.Count > i; i++)
                {
                    clsAppNetDocumentos_Movimientos objMovimiento = new clsAppNetDocumentos_Movimientos();
                    objMovimiento.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                    objMovimiento.AppNetFlujoEstados = new clsAppNetFlujoEstados() { ID = Convert.ToInt32(dttDatos.Rows[i]["IDAppNetFlujoEstados"].ToString()), Nombre = dttDatos.Rows[i]["NombreAppNetFlujoEstados"].ToString() };
                    objMovimiento.Descripcion_Movimiento = dttDatos.Rows[i]["Descripcion_Movimiento"].ToString();
                    objMovimiento.AppNetUsuariosCreacion = new clsAppNetUsuarios() { ID = Convert.ToInt64(dttDatos.Rows[i]["IDAppNetUsuariosCreacion"].ToString()), Nombre = dttDatos.Rows[i]["AppNetUsuariosCreacion"].ToString() };
                    objMovimiento.FechaMovimiento = Convert.ToDateTime(dttDatos.Rows[i]["FechaMovimiento"].ToString());
                    lstMovimientos.Add(objMovimiento);
                }

                dttDatos.Dispose();
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstMovimientos;
        }

        /// <summary>
        /// Metodo utilizado para consultar el flujo de documentos
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public List<clsAppNetFlujoEstados> consultaListadoEstados(clsAppNetUsuarios objUsuario)
        {
            List<clsAppNetFlujoEstados> lstEstados = new List<clsAppNetFlujoEstados>();
            try
            {
                this.objControlador.setCommand("AppNetFlujoEstados_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                DataTable dttEstados = dtsDatos.Tables[0];
                DataTable dttAcciones = dtsDatos.Tables[1];
                for (int i = 0; i < dttEstados.Rows.Count; i++)
                {
                    clsAppNetFlujoEstados objEstado = new clsAppNetFlujoEstados();
                    objEstado.ID = Convert.ToInt64(dttEstados.Rows[i]["ID"].ToString());
                    objEstado.Nombre = dttEstados.Rows[i]["Nombre"].ToString();

                    List<DataRow> lstItems = dttAcciones.Select("IDAppNetFlujoEstados = " + objEstado.ID.ToString()).ToList();
                    for (int j = 0; j < lstItems.Count; j++)
                    {
                        clsAppNetFlujoEstados_Acciones objAccion = new clsAppNetFlujoEstados_Acciones();
                        objAccion.ID = Convert.ToInt32(lstItems[j]["ID"].ToString());
                        objAccion.Nombre = lstItems[j]["Nombre"].ToString();
                        objAccion.IDAppNetFlujoEstados_Sigiente = Convert.ToInt32(lstItems[j]["IDAppNetFlujoEstados_Sigiente"].ToString());
                        objEstado.lstAcciones.Add(objAccion);
                    }
                    lstEstados.Add(objEstado);
                }
                this.objControlador.closeConnection();
                dtsDatos.Dispose();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEstados;
        }


        /// <summary>
        /// Metodo utilizado para consultar el flujo de documentos
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public List<clsAppNetFlujoEstados> consultaListadoEstados(clsAppNetUsuarios objUsuario, int xID)
        {
            List<clsAppNetFlujoEstados> lstEstados = new List<clsAppNetFlujoEstados>();
            try
            {
                this.objControlador.setCommand("AppNetFlujoEstados_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@ID", xID);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                DataTable dttEstados = dtsDatos.Tables[0];
                DataTable dttAcciones = dtsDatos.Tables[1];
                for (int i = 0; i < dttEstados.Rows.Count; i++)
                {
                    clsAppNetFlujoEstados objEstado = new clsAppNetFlujoEstados();
                    objEstado.ID = Convert.ToInt64(dttEstados.Rows[i]["ID"].ToString());
                    objEstado.Nombre = dttEstados.Rows[i]["Nombre"].ToString();
                    objEstado.OPcionMasiva = Convert.ToBoolean(dttEstados.Rows[i]["OPcionMasiva"].ToString());

                    List<DataRow> lstItems = dttAcciones.Select("IDAppNetFlujoEstados = " + objEstado.ID.ToString()).ToList();
                    for (int j = 0; j < lstItems.Count; j++)
                    {
                        clsAppNetFlujoEstados_Acciones objAccion = new clsAppNetFlujoEstados_Acciones();
                        objAccion.ID = Convert.ToInt32(lstItems[j]["ID"].ToString());
                        objAccion.Nombre = lstItems[j]["Nombre"].ToString();
                        objAccion.IDAppNetFlujoEstados_Sigiente = Convert.ToInt32(lstItems[j]["IDAppNetFlujoEstados_Sigiente"].ToString());
                        objEstado.lstAcciones.Add(objAccion);
                    }
                    lstEstados.Add(objEstado);
                }
                this.objControlador.closeConnection();
                dtsDatos.Dispose();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEstados;
        }

        /// <summary>
        /// Metodo utilizado para consultar el flujo de documentos
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public List<clsAppNetFlujoEstados> consultaListadoEstados(clsAppNetUsuarios objUsuario, int xID, string xNombre)
        {
            List<clsAppNetFlujoEstados> lstEstados = new List<clsAppNetFlujoEstados>();
            try
            {
                this.objControlador.setCommand("AppNetFlujoEstados_Consulta");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@ID", xID);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                DataTable dttEstados = dtsDatos.Tables[0];
                DataTable dttAcciones = dtsDatos.Tables[1];
                for (int i = 0; i < dttEstados.Rows.Count; i++)
                {
                    clsAppNetFlujoEstados objEstado = new clsAppNetFlujoEstados();
                    objEstado.ID = Convert.ToInt64(dttEstados.Rows[i]["ID"].ToString());
                    objEstado.Nombre = dttEstados.Rows[i]["Nombre"].ToString();
                    objEstado.OPcionMasiva = Convert.ToBoolean(dttEstados.Rows[i]["OPcionMasiva"].ToString());
                    objEstado.ConCorreo = Convert.ToBoolean(dttEstados.Rows[i]["ConCorreo"].ToString());
                    objEstado.SeleccionaUsuarioAnterior = Convert.ToBoolean(dttEstados.Rows[i]["SeleccionaUsuarioAnterio"].ToString());

                    List<DataRow> lstItems = dttAcciones.Select("IDAppNetFlujoEstados = " + objEstado.ID.ToString()).ToList();
                    for (int j = 0; j < lstItems.Count; j++)
                    {
                        clsAppNetFlujoEstados_Acciones objAccion = new clsAppNetFlujoEstados_Acciones();
                        objAccion.ID = Convert.ToInt32(lstItems[j]["ID"].ToString());
                        objAccion.Nombre = lstItems[j]["Nombre"].ToString();
                        objAccion.IDAppNetFlujoEstados_Sigiente = Convert.ToInt32(lstItems[j]["IDAppNetFlujoEstados_Sigiente"].ToString());
                        objEstado.lstAcciones.Add(objAccion);
                    }
                    lstEstados.Add(objEstado);
                }
                this.objControlador.closeConnection();
                dtsDatos.Dispose();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEstados;
        }


        /// <summary>
        /// Metodo utilizado para consultar el flujo de documentos
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public List<clsAppNetUsuarios> consultaListadoResponsables(int xIDEstado, int xIDAccion, clsAppNetUsuarios objUsuario)
        {
            List<clsAppNetUsuarios> lstEstados = new List<clsAppNetUsuarios>();
            try
            {
                this.objControlador.setCommand("ConsultaResponsableSiguienteEstado");
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@IDAppNetFlujoEstados_Acciones", xIDAccion);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                DataSet dtsDatos = this.objControlador.execDataSetResult();

                DataTable dttEstados = dtsDatos.Tables[0];
                for (int i = 0; i < dttEstados.Rows.Count; i++)
                {
                    clsAppNetUsuarios objEstado = new clsAppNetUsuarios();
                    objEstado.ID = Convert.ToInt64(dttEstados.Rows[i]["ID"].ToString());
                    objEstado.Nombre = dttEstados.Rows[i]["Nombre"].ToString();
                    lstEstados.Add(objEstado);
                }
                this.objControlador.closeConnection();
                dtsDatos.Dispose();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEstados;
        }

        /// <summary>
        /// Metodo utilizado para asignación masiva de documentos al flujo documental
        /// </summary>
        /// <param name="xIDDOcumentos"></param>
        /// <param name="xIDEstado"></param>
        /// <param name="xIDAccion"></param>
        /// <param name="xResponsable"></param>
        /// <returns></returns>
        public clsResultadoJson asignacionFlujoDocumentos(string xIDDOcumentos, int xIDEstado, int xIDAccion, int xResponsable, clsAppNetUsuarios objUsuario)
        {
            clsResultadoJson objJsonResultado = new clsResultadoJson();
            try
            {
                this.objControlador.setCommand("asignacionMasivaDocumentos");
                this.objControlador.addNewParameter("@IDDocumentos", xIDDOcumentos);
                this.objControlador.addNewParameter("@IDEstado", xIDEstado);
                this.objControlador.addNewParameter("@IDAccion", xIDAccion);
                this.objControlador.addNewParameter("@IDResponsable", xResponsable);
                this.objControlador.addNewParameter("@IDAppNetAdministrador", objUsuario.IDAdministrador);
                this.objControlador.addNewParameter("@IDAppNetUsuario", objUsuario.ID);
                this.objControlador.execScalar();
                this.objControlador.closeConnection();
                objJsonResultado.MensajeProceso = "Proceso realizado correctamente.";
            }
            catch (Exception ex)
            {
                objJsonResultado.cargarErro(ex);
            }
            return objJsonResultado;
        }

        /// <summary>
        /// Metodo utilizado para asignación masiva de documentos al flujo documental
        /// </summary>
        /// <param name="xIDDOcumentos"></param>
        /// <param name="xIDEstado"></param>
        /// <param name="xIDAccion"></param>
        /// <param name="xResponsable"></param>
        /// <returns></returns>
        public void cargaTablaTemporalAcciones(string xGuid, int xIDEstado)
        {
            try
            {
                this.objControlador.setCommand("cargaTemporalEstado");
                this.objControlador.addNewParameter("@CodigoGuid", xGuid);
                this.objControlador.addNewParameter("@IDAppNetFlujoEstados", xIDEstado);
                this.objControlador.execScalar();
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
        }

        /// <summary>
        /// Metodo utilizado para consultar temporalpara la edicion de estado
        /// </summary>
        /// <param name="xGuid"></param>
        /// <returns></returns>
        public List<clsAppNetFlujoEstados_Acciones> cargaTemporalPorGuid(string xGuid)
        {
            this.objControlador.setCommand("consultaTemporal");
            this.objControlador.addNewParameter("@CodigoGuid", xGuid);
            DataTable dttDatos = this.objControlador.execTableResult();
            List<clsAppNetFlujoEstados_Acciones> lstAcciones = new List<clsAppNetFlujoEstados_Acciones>();
            for (int j = 0; j < dttDatos.Rows.Count; j++)
            {
                clsAppNetFlujoEstados_Acciones objAccion = new clsAppNetFlujoEstados_Acciones();
                objAccion.ID = Convert.ToInt32(dttDatos.Rows[j]["ID"].ToString());
                objAccion.Nombre = dttDatos.Rows[j]["Nombre"].ToString();
                objAccion.IDAppNetFlujoEstados_Sigiente = Convert.ToInt32(dttDatos.Rows[j]["IDAppNetFlujoEstados_Sigiente"].ToString());
                lstAcciones.Add(objAccion);
            }
            this.objControlador.closeConnection();
            return lstAcciones;
        }



        /// <summary>
        /// Metodo utilizado para consultar temporalpara la edicion de estado
        /// </summary>
        /// <param name="xGuid"></param>
        /// <returns></returns>
        public clsResultadoJson guardarAccionTemporal(clsAppNetFlujoEstados_Acciones objAccion, clsAppNetUsuarios objUsuario)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            this.objControlador.setCommand("consultaTemporal");
            this.objControlador.closeConnection();
            return objResultado;
        }


    }
}