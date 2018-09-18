using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases.ForeCast;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases.Productos;
using AbcapGestionComercialMVC.App_Start;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;

namespace AbcapGestionComercialMVC.Controllers
{
    [AbcapGestionComercialMVC.validaSession]
    public class ForeCastController : Controller
    {
        /// <summary>
        /// Usuario session
        /// </summary>
        private clsAppNetUsuarios Usuario
        {
            get
            {
                return (clsAppNetUsuarios)Session["UserData"];
            }
        }

        /// <summary>
        /// Procesos ejecutandose en hilo por el usuario
        /// </summary>
        private List<System.Threading.Thread> Procesos
        {
            get
            {
                if (Session["Procesos"] == null)
                {
                    Session["Procesos"] = new List<System.Threading.Thread>();
                }
                return (List<System.Threading.Thread>)Session["Procesos"];
            }
        }

        /// <summary>
        /// Procesos ejecutandose en hilo por el usuario
        /// </summary>
        private List<clsProcesos> lstProcesos
        {
            get
            {
                if (Session["lstProcesos"] == null)
                {
                    Session["lstProcesos"] = new List<clsProcesos>();
                }
                return (List<clsProcesos>)Session["lstProcesos"];
            }
        }

        private int _TIPOPROCESO = 1;
        // GET: ControlProyeccion
        public ActionResult Index(string xIDUnico)
        {
            string xCadenRequest = SHA.DesEncriptar(xIDUnico);
            int xIntIDUnico = 0;
            if (int.TryParse(xCadenRequest, out xIntIDUnico))
            {
                //clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
                //objProyeccionBD.LimpiaSincronizacion(xIntIDUnico, "100");
                //clsProyeccionVenta objProyeccion = objProyeccionBD.consultarProyeccion(xIntIDUnico, "-1");
                ViewBag.IDUnico = xIDUnico;
                ViewBag.pantallaCompleta = false;
                return View();
            }
            else
            {
                return RedirectToAction("Consulta", "ForeCast");
            }
        }

        // GET: ControlProyeccion
        public ActionResult busquedaProyeccion(int ddlProyeccionVenta, string ddlCanal)
        {
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            clsGeneralBD objGeneralBD = new clsGeneralBD();
            clsForeCastBD objForeCast = new clsForeCastBD();

            string xIDUnico = SHA.Encriptar(ddlProyeccionVenta.ToString());
            string xModificacion = ddlProyeccionVenta.ToString() + "-" + ((clsAppNetUsuarios)Session["UserData"]).ID.ToString() + "-" + ddlCanal;
            objProyeccionBD.LimpiaSincronizacion(ddlProyeccionVenta, xModificacion);

            clsProyeccionVenta objProyeccion = objProyeccionBD.consultarProyeccion(ddlProyeccionVenta, ddlCanal);
            objProyeccion.Canal = ddlCanal;

            clsAppNetProyeccionForeCast objForeCastItem = objForeCast.consultaForeCast(
                new clsAppNetProyeccionForeCast_Filtro()
                {
                    IDF = ddlProyeccionVenta
                }, this.Usuario)[0];

            List<clsSemana> lstAnios = objGeneralBD.consultaAnios();
            List<clsSemana> lstSemanasInicial = objGeneralBD.consultaSemanas(objForeCastItem.AnioSemanaInicial.ANO);
            List<clsSemana> lstSemanasFinal = objGeneralBD.consultaSemanas(objForeCastItem.AnioSemanaFinal.ANO);

            ViewBag.AnioInicial = new SelectList(lstAnios, "ID", "Nombre", objForeCastItem.AnioSemanaInicial.ANO);
            ViewBag.SemanasInicial = new SelectList(lstSemanasInicial, "ID", "Nombre", objForeCastItem.AnioSemanaInicial.ID);
            ViewBag.AnioFinal = new SelectList(lstAnios, "ID", "Nombre", objForeCastItem.AnioSemanaInicial.ANO);
            ViewBag.SemanasFinal = new SelectList(lstSemanasFinal, "ID", "Nombre", objForeCastItem.AnioSemanaFinal.ID);

            ViewBag.IDModificacion = xModificacion;
            ViewBag.IDUnico = xIDUnico;
            ViewBag.pantallaCompleta = false;
            return View("Index", objProyeccion);
        }

        /// <summary>
        /// Metodo utiilizado para sincronizar listado de movimientos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SincronizarBDTemporal(int xIdProvision, string strData, string IDModificacion)
        {
            clsResultadoJson objResultadoJson = new clsResultadoJson();
            try
            {

                if (strData != string.Empty)
                {
                    clsProyeccionVentaBD objProyeccionVenta = new clsProyeccionVentaBD();
                    List<clsItemModificacion> lstMovimientos = JsonConvert.DeserializeObject<List<clsItemModificacion>>(strData);
                    objProyeccionVenta.sincronizacion(xIdProvision, lstMovimientos, IDModificacion);
                }
                objResultadoJson.ResultadoProceso = true;
                objResultadoJson.MensajeProceso = "Sincronizado";
                objResultadoJson.MensajeTecnicoFormulario = "Última sincronización " + DateTime.Now.Date.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();
            }
            catch (Exception ex)
            {
                objResultadoJson.ResultadoProceso = false;
                objResultadoJson.MensajeProceso = ex.Message;
            }
            return Json(objResultadoJson);
        }

        /// <summary>
        /// Metodo utiilizado para sincronizar listado de movimientos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SincronizarYGuardaDatosFinal(int xIdProvision, string strData, string IDModificacion)
        {
            clsResultadoJson objResultadoJson = new clsResultadoJson();
            try
            {

                clsForeCastBD objForeCastBD = new clsForeCastBD();
                if (strData != string.Empty)
                {
                    List<clsItemModificacion> lstMovimientos = JsonConvert.DeserializeObject<List<clsItemModificacion>>(strData);
                    objForeCastBD.sincronizacion(xIdProvision, lstMovimientos, IDModificacion);
                }

                /*Guarda cambios realizados sobre el control de proyecciones*/
                if (objForeCastBD.guardarCambiosProyeccion(xIdProvision, IDModificacion, this.Usuario))
                {
                    objResultadoJson.ResultadoProceso = true;
                    objResultadoJson.MensajeProceso = "Sincronizado y Guardado";
                    objResultadoJson.MensajeTecnicoFormulario = "Última sincronización " + DateTime.Now.Date.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();
                }
                else
                {
                    objResultadoJson.ResultadoProceso = false;
                    objResultadoJson.MensajeProceso = objForeCastBD.cadenaError;
                    objResultadoJson.MensajeTecnicoFormulario = "Última sincronización " + DateTime.Now.Date.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();
                }
            }
            catch (Exception ex)
            {
                objResultadoJson.ResultadoProceso = false;
                objResultadoJson.MensajeProceso = ex.Message;
            }
            return Json(objResultadoJson);
        }

        /// <summary>
        /// Get: Consultar proyecciones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult consultarProyecciones(string IDUnico)
        {
            string xCadenRequest = SHA.DesEncriptar(IDUnico);
            int xIntIDUnico = 0;
            if (int.TryParse(xCadenRequest, out xIntIDUnico))
            {
                clsConsultaProyeccionFiltros objFiltros = new clsConsultaProyeccionFiltros();
                clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
                List<clsProyeccionVenta> lstProyeccionVenta = objProyeccionBD.consultaBasicaProyecciones(xIntIDUnico);
                List<clsCanal> lstCanales = objProyeccionBD.consultaCanalesUsuarioYForeCast((clsAppNetUsuarios)Session["UserData"], xIntIDUnico);
                List<SelectListItem> lstProyecciones = new List<SelectListItem>();
                List<SelectListItem> lstCanal = new List<SelectListItem>();
                /*Carga proyecciones*/
                lstProyeccionVenta.ForEach(delegate (clsProyeccionVenta objProteccion)
                {
                    SelectListItem objItemNuevo = new SelectListItem();
                    objItemNuevo.Value = Convert.ToString(objProteccion.ID);
                    objItemNuevo.Text = objProteccion.Nombre;
                    lstProyecciones.Add(objItemNuevo);
                });

                /*Carga Canales*/
                lstCanales.ForEach(delegate (clsCanal objProteccion)
                {
                    SelectListItem objItemNuevo = new SelectListItem();
                    objItemNuevo.Value = objProteccion.Nombre;
                    objItemNuevo.Text = objProteccion.Nombre;
                    lstCanal.Add(objItemNuevo);
                });

                objFiltros.Proyecciones = new SelectList(lstProyecciones, "Value", "Text", lstProyecciones[0].Value);
                objFiltros.Canales = new SelectList(lstCanal, "Value", "Text", lstCanal[0].Value);

                return PartialView(objFiltros);
            }
            else
            {
                return PartialView(new clsConsultaProyeccionFiltros());
            }
        }

        /// <summary>
        /// Metodo encargado de crear nueva proyección
        /// </summary>
        /// <returns></returns>
        public PartialViewResult crearNuevaProyeccion()
        {
            return PartialView();
        }

        /// <summary>
        /// Metodo encargado de consultar proyecciones de fore cast
        /// </summary>
        /// <returns></returns>
        public ViewResult Consulta()
        {
            clsForeCastBD objFcBD = new clsForeCastBD();
            clsGeneralBD objGeneralBD = new clsGeneralBD();

            List<clsTipoDatoPYG> lstTipoDato = objFcBD.consultaListadoTipoDatoPyG();
            lstTipoDato.Add(new clsTipoDatoPYG() { ID = -1, Nombre = "--Todos.." });

            List<clsEstadoProyeccion> lstEstadosProyeccion = objGeneralBD.ConsultaListaEstados(1);
            lstEstadosProyeccion.Add(new clsEstadoProyeccion() { ID = -1, Nombre = "--Todos--" });

            ViewBag.ListadoTipoDato = new SelectList(lstTipoDato, "ID", "Nombre", -1);
            ViewBag.EstadosProyeccion = new SelectList(lstEstadosProyeccion, "ID", "Nombre", -1);

            return View(new clsAppNetProyeccionForeCast_Filtro() { TipoProcesoF = new clsAppNetTipoProceso() { ID = this._TIPOPROCESO } });
        }

        /// <summary>
        /// Consulta de listado de ForeCast
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ConsultaListadoForeCast(clsAppNetProyeccionForeCast_Filtro objFiltro)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast> lstForecCast = objForeCastBD.consultaForeCast(objFiltro, this.Usuario);
            return PartialView(lstForecCast);
        }

        /// <summary>
        /// Metodo encargado de consultar semanas del año
        /// </summary>
        /// <param name="xAnio"></param>
        /// <returns></returns>
        public JsonResult consultaSemanaPorAnio(int xAnio)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsGeneralBD objGeneralBD = new clsGeneralBD();
            objResultado.objResultado = objGeneralBD.consultaSemanas(xAnio);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo encargado de consultar semanas del año
        /// </summary>
        /// <param name="xAnio"></param>
        /// <returns></returns>
        public JsonResult crearNombreAutomaticoFcts(int idBasTipoDatoPyG, int xSemanaInicia, int xSemanaFinal)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            clsAppNetUsuarios objUsuario = (clsAppNetUsuarios)Session["UserData"];
            objResultado.MensajeProceso = objForeCastBD.generaNombreAutomaticoFcts(idBasTipoDatoPyG, xSemanaInicia, xSemanaFinal, objUsuario.ID);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para iniciar la creación o modificacion de un ForeCast
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public PartialViewResult AdministrarForeCast(long xIDForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            clsGeneralBD objGeneralBD = new clsGeneralBD();

            clsAppNetProyeccionForeCast objForeCast;
            if (xIDForeCast != 0)
            {
                objForeCast = objForeCastBD.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro() { IDF = xIDForeCast }, this.Usuario)[0];
            }
            else
            {
                objForeCast = new clsAppNetProyeccionForeCast();
                objForeCastBD.asignarValoresForeCastDefecto(objForeCast);
                objForeCast.TipoProceso.ID = this._TIPOPROCESO;
            }
            List<clsTipoDatoPYG> lstTipoDato = objForeCastBD.consultaListadoTipoDatoPyG();

            List<clsSemana> lstAnios = objGeneralBD.consultaAnios();
            List<clsSemana> lstSemanasInicial = objGeneralBD.consultaSemanas(objForeCast.AnioSemanaInicial.ANO);
            List<clsSemana> lstSemanasFinal = objGeneralBD.consultaSemanas(objForeCast.AnioSemanaFinal.ANO);
            List<clsAppNetProyeccionForeCast> lstForeCastAnteriores = new List<clsAppNetProyeccionForeCast>();
            lstForeCastAnteriores.Add(new clsAppNetProyeccionForeCast() { ID = -1, Nombre = "--Seleccione una opción--" });
            lstForeCastAnteriores.AddRange(objForeCastBD.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro()
            {
                BasTipoDatoPyGF = new clsTipoDatoPYG() { ID = -1 },
                BasEstadoProyeccionF = new clsEstadoProyeccion() { ID = -1 }
            }, this.Usuario));
            List<clsEstadoProyeccion> lstEstadosProyeccion = objGeneralBD.ConsultaListaEstados(1);
            lstForeCastAnteriores.ForEach(delegate (clsAppNetProyeccionForeCast objUsuario)
            {
                if (objUsuario.ID != -1)
                {
                    objUsuario.Nombre = "ID: " + objUsuario.Consecutivo.ToString() + " - " + objUsuario.Nombre;
                }
            });

            /*Carga listados*/
            ViewBag.ListadoTipoDato = new SelectList(lstTipoDato, "ID", "Nombre", objForeCast.Bas_TipoDatoPYG.ID);
            ViewBag.AnioInicial = new SelectList(lstAnios, "ID", "Nombre", objForeCast.AnioSemanaInicial.ANO);
            ViewBag.SemanasInicial = new SelectList(lstSemanasInicial, "ID", "Nombre", objForeCast.AnioSemanaInicial.ID);
            ViewBag.AnioFinal = new SelectList(lstAnios, "ID", "Nombre", objForeCast.AnioSemanaFinal.ANO);
            ViewBag.SemanasFinal = new SelectList(lstSemanasFinal, "ID", "Nombre", objForeCast.AnioSemanaFinal.ID);
            ViewBag.ForeCastAnteriores = new SelectList(lstForeCastAnteriores, "ID", "Nombre", objForeCast.IDForeCastAnterior);
            ViewBag.EstadosProyeccion = new SelectList(lstEstadosProyeccion, "ID", "Nombre", 1);

            return PartialView(objForeCast);
        }

        /// <summary>
        /// Metodo encargado de guardar ForeCast
        /// </summary>
        /// <param name="objForeCast"></param>
        /// <returns></returns>
        public JsonResult guardarForeCast(clsAppNetProyeccionForeCast objForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            objForeCast.Usuariomodificacion = ((clsAppNetUsuarios)Session["UserData"]);
            clsResultadoJson objResultado = objForeCastBD.guardarForeCast(objForeCast);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo encargado de inicializar proceso de configuración de productos a proyectar
        /// </summary>
        /// <param name="xIDUnico"></param>
        /// <returns></returns>
        public ActionResult ConfiguracionClientesProductos(string xIDUnico)
        {
            string xCadenRequest = SHA.DesEncriptar(xIDUnico);
            int xIntIDUnico = 0;
            if (int.TryParse(xCadenRequest, out xIntIDUnico))
            {
                clsAppNetUsuarios objUsuario = (clsAppNetUsuarios)Session["UserData"];
                clsForeCastBD objForeCasBD = new clsForeCastBD();
                clsAppNetProyeccionForeCast objForeCast = objForeCasBD.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro() { IDF = xIntIDUnico }, this.Usuario)[0];
                ViewBag.Data = objForeCasBD.ProductosyClientesProyectar(xIntIDUnico, objUsuario);
                return View(objForeCast);
            }
            else
            {
                return RedirectToAction("Consulta", "ForeCast");
            }
        }

        /// <summary>
        /// Metodo encargado de guardar cambios realizados sobre listadao de productos
        /// </summary>
        /// <param name="ID">ID ForeCast</param>
        /// <param name="hdfDataModificada">String con Json Modificado</param>
        /// <returns></returns>
        public JsonResult GuardarConfiguracionClientesProductos(int ID, string hdfDataModificada)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                clsForeCastBD objForeCast = new clsForeCastBD();
                List<clsAppNetProyeccionForeCast> lstItems = objForeCast.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro()
                {
                    IDF = ID
                }, this.Usuario);

                if (lstItems.Count > 0)
                {
                    if (lstItems[0].EstadoObj.ID == 1)
                    {
                        var serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = 999999999;
                        List<clsclsAppNetProyeccionForeCast_ItemsSencillo> lstObjetos = serializer.Deserialize<List<clsclsAppNetProyeccionForeCast_ItemsSencillo>>(hdfDataModificada);
                        if (lstObjetos.Count > 0)
                        {
                            objResultado = objForeCast.guardarListadoItems(ID, lstObjetos, this.Usuario);
                        }
                        else
                        {
                            objResultado.ResultadoProceso = true;
                            objResultado.objResultado = SHA.Encriptar(ID.ToString());
                        }
                    }
                    else
                    {
                        objResultado.ResultadoProceso = true;
                        objResultado.objResultado = SHA.Encriptar(ID.ToString());
                    }
                }
                else
                {
                    objResultado.ResultadoProceso = false;
                    objResultado.MensajeProceso = "Sin permisos para modificar proyección / ForeCast.";
                }
            }
            catch (Exception ex)
            {
                objResultado.ResultadoProceso = false;
                objResultado.MensajeProceso = ex.Message;
            }
            return Json(objResultado);
        }

        /// <summary>
        /// Consulta listado de productos sin asignar cliente
        /// </summary>
        /// <param name="xCodigo"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult consultaProductosSinAsignarCliente(string txtCodigo, string txtNombre)
        {
            clsProductoBD objProductoBD = new clsProductoBD();
            List<clsProductosSincronizacion> lstProductos = objProductoBD.consultaListadoProductosSinCliente(clsGeneralBD.validaCadenaNUllAMenosUno(txtCodigo), clsGeneralBD.validaCadenaNUllAMenosUno(txtNombre));
            return PartialView(lstProductos);
        }


        #region CONSULTA DE MOVIMIENTOS DE FORECAST

        /// <summary>
        /// Crea un listado con los movimientos realizados en el forecast.
        /// </summary>
        /// <param name="IDForeCast"></param>
        /// <returns></returns>
        public PartialViewResult consultaMovimientosForeCast(long IDForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Movimientos> lstMovimientos = objForeCastBD.consultaListadoMovimientosForeCast(IDForeCast);
            return PartialView(lstMovimientos);
        }

        #endregion

        #region ARCHIVOS FORECAST

        /// <summary>
        /// metodo utilzado para mostrar datos para la carga de archivos
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult administrarArchivosProyeccion(long xIDForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            clsAppNetProyeccionForeCast objForeCastb = objForeCastBD.consultaForeCast(
                    new clsAppNetProyeccionForeCast_Filtro()
                    {
                        IDF = xIDForeCast
                    }, this.Usuario)[0];

            if (objForeCastb != null)
            {

            }
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = objForeCastBD.consultaListadoArchivos(xIDForeCast);
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivosConsolidados = objForeCastBD.consultaListadoArchivosConsolidados(xIDForeCast);
            ViewBag.IDForeCast = xIDForeCast;
            ViewBag.ObjForeCast = objForeCastb;
            ViewBag.lstArchivosConsolidados = lstArchivosConsolidados;
            return PartialView(lstArchivos);
        }

        /// <summary>
        /// metodo utilzado para subir nuevos archivos
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult iniciarNuevaSubida()
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            ViewBag.LstTipoArchivo = objForeCastBD.ConsultaTipoArchivo();
            return PartialView();
        }

        /// <summary>
        /// metodo encargado de subir archivos de ForeCast
        /// </summary>
        /// <returns></returns>
        public JsonResult SubirArchivosForeCast(int IDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();

            try
            {
                string ddlTipoArchivo = Request.Params["ddlTipoArchivo"].ToString();
                string txtObservaciones = Request.Params["txtObservaciones"].ToString();
                DateTime txtFechaArchivo = Convert.ToDateTime(Request.Params["txtFechaArchivo"].ToString());
                if (this.Request.Files.Count > 0)
                {
                    /*VALIDA DATOS DEL ARCHIVO*/
                    List<string> lstCadenaValidaciones = new List<string>();
                    for (int i = 0; i < this.Request.Files.Count; i++)
                    {
                        string xExtension = System.IO.Path.GetExtension(this.Request.Files[i].FileName);
                        if (xExtension.ToUpper() != ".txt".ToUpper() && xExtension.ToUpper() != ".csv".ToUpper())
                        {
                            lstCadenaValidaciones.Add("Formato de archivo incorrecto.");
                        }
                    }

                    /*VALIDA QUE EL CUMPLA LAS VALIDACIONES*/
                    if (lstCadenaValidaciones.Count == 0)
                    {
                        clsForeCastBD objForeCastBD = new clsForeCastBD();
                        clsAppNetProyeccionForeCast objForeCast = objForeCastBD.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro() { IDF = IDForeCast }, this.Usuario)[0];
                        if (objForeCast != null)
                        {
                            string strArchivador = Path.Combine(clsConfiguracionConexion.carpetaServidorProduccion02, objForeCast.ID.ToString());
                            if (!Directory.Exists(strArchivador))
                            {
                                Directory.CreateDirectory(strArchivador);
                            }
                            for (int i = 0; i < this.Request.Files.Count; i++)
                            {
                                HttpPostedFileBase objFile = this.Request.Files[i];
                                string xExtension = System.IO.Path.GetExtension(objFile.FileName);
                                long xIDArchivo = objForeCastBD.insertarArchivoForeCast(
                                    IDForeCast,
                                    objFile.FileName.ToString(),
                                    xExtension,
                                    objFile.ContentLength,
                                    this.Usuario,
                                    Convert.ToInt32(ddlTipoArchivo),
                                    txtObservaciones,
                                    txtFechaArchivo
                                    );
                                if (xIDArchivo != -1)
                                {
                                    string strArchivo = Path.Combine(strArchivador, xIDArchivo.ToString());
                                    objFile.SaveAs(strArchivo + xExtension);
                                }
                                else
                                {
                                    objResultado.ResultadoProceso = false;
                                    objResultado.MensajeProceso = "Error guardando archivo. Detalle del error: " + objForeCastBD.cadenaError;
                                }
                            }

                        }
                        else
                        {
                            objResultado.ResultadoProceso = false;
                            objResultado.MensajeProceso = "No se encuentra el Fore Cast.";
                        }
                    }
                    else
                    {
                        objResultado.ResultadoProceso = false;
                        objResultado.MensajeProceso = lstCadenaValidaciones[0];
                    }
                }
                else
                {
                    objResultado.ResultadoProceso = false;
                    objResultado.MensajeProceso = "La cantidad de archivos no puede ser cero";
                }
            }
            catch (Exception Ex)
            {
                objResultado.cargarErro(Ex);
                objResultado.ResultadoProceso = false;
            }
            return Json(objResultado);
        }

        /// <summary>
        /// Metodoe utilizado para realizar la consolidacion de los archivos cargados
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult validaSiProcesaWC(long xIDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsForeCastBD objForeCatBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = objForeCatBD.consultaListadoArchivoWordClass(xIDForeCast);
            objResultado.ResultadoProceso = lstArchivos.Count > 0;
            objResultado.objResultado = xIDForeCast;
            return Json(objResultado);
        }

        /// <summary>
        /// Metodoe utilizado para realizar la consolidacion de los archivos cargados
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult consolidarArchivos(long xIDForeCast)
        {
            clsForeCastBD objForeCatBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Archivos> lstArchivos = objForeCatBD.consultaListadoArchivoWordClass(xIDForeCast);
            ViewBag.IDForeCast = xIDForeCast;
            return PartialView(new SelectList(lstArchivos, "ID", "Nombre"));
        }

        /// <summary>
        /// Metodo utilizado para consolidar archivos de Word class y GG
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <param name="xIDArchivoPrincipal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult generarConsolidadoArchivos(long xIDForeCast, long xIDArchivoPrincipal)
        {
            clsForeCastBD objForeCast = new clsForeCastBD();
            clsResultadoJson objResultado = objForeCast.generarConsolidacionArchivo(xIDForeCast, xIDArchivoPrincipal, this.Usuario);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para validar archivo subido al sistema
        /// </summary>
        /// <param name="xIDArchivo"></param>
        /// <returns></returns>
        public JsonResult ValidarArchivo(long xIDArchivo)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            System.Threading.Thread objProceso = new System.Threading.Thread(validarArchivoBD);
            this.Usuario.IDTemporal = xIDArchivo;
            objProceso.Start(this.Usuario);

            Procesos.Add(objProceso);
            objResultado.ResultadoProceso = true;
            return Json(objResultado);
        }

        /// <summary>
        /// Proceso utilizado en multi hilo para no congelar el sistema del cliente
        /// </summary>
        /// <param name="objArchivo"></param>
        private void validarArchivoBD(object objArchivo)
        {
            clsAppNetUsuarios IdArchivo = (clsAppNetUsuarios)objArchivo;
            clsForeCastBD objForeCasBD = new clsForeCastBD();
            objForeCasBD.CargaYValidaArchivo(IdArchivo.IDTemporal, IdArchivo);
        }


        /// <summary>
        /// Proceso utilizado en multi hilo para no congelar el sistema del cliente
        /// </summary>
        /// <param name="objArchivo"></param>
        public JsonResult eliminarArchivo(string strIDArchivos)
        {
            clsResultadoJson objJson = new clsResultadoJson();
            clsForeCastBD objForeCasBD = new clsForeCastBD();

            if (objForeCasBD.eliminaArchivos(strIDArchivos))
            {
                objJson.ResultadoProceso = true;
                objJson.MensajeProceso = "Archivos eliminados exitosamente.";
            }
            else
            {
                objJson.ResultadoProceso = false;
                objJson.MensajeProceso = objForeCasBD.cadenaError;
            }

            return Json(objJson);
        }

        /// <summary>
        /// Metodo utilizado para eliminar la consolidación de los archivos realizada.
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public JsonResult eliminarConsolidacionArchivo(long xIDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsForeCastBD objForeCast = new clsForeCastBD();
            objResultado = objForeCast.eliminarConsolidacionArchivos(xIDForeCast);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para cargar popup con datos de importacion de forecast
        /// </summary>
        /// <param name="ddlProyeccionVenta"></param>
        /// <param name="ddlCanal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ImportarDatosAForeCast(string ddlProyeccionVenta, string ddlCanal, string hdfIDModificacion)
        {

            long xIDForeCast = Convert.ToInt64(SHA.DesEncriptar(ddlProyeccionVenta));
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            clsGeneralBD objGeneralBD = new clsGeneralBD();
            clsForeCastBD objForeCast = new clsForeCastBD();

            clsAppNetProyeccionForeCast objForeCastItem = objForeCast.consultaForeCast(
                new clsAppNetProyeccionForeCast_Filtro()
                {
                    IDF = xIDForeCast
                }, this.Usuario)[0];


            List<clsSemana> lstAnios = objGeneralBD.consultaAnios();
            List<clsSemana> lstSemanasInicial = objGeneralBD.consultaSemanas(objForeCastItem.AnioSemanaInicial.ANO);
            List<clsSemana> lstSemanasFinal = objGeneralBD.consultaSemanas(objForeCastItem.AnioSemanaFinal.ANO);
            List<clsAppNetProyeccionForeCast_Archivos> lstConsolidado = new List<clsAppNetProyeccionForeCast_Archivos>();
            lstConsolidado.Add(new clsAppNetProyeccionForeCast_Archivos()
            {
                ID = -1,
                Nombre = "--Seleccione una opción--"
            });
            lstConsolidado.AddRange(objForeCast.consultaListadoArchivosConsolidadosXCanal(xIDForeCast, ddlCanal));

            ViewBag.AnioInicial = new SelectList(lstAnios, "ID", "Nombre", objForeCastItem.AnioSemanaInicial.ANO);
            ViewBag.SemanasInicial = new SelectList(lstSemanasInicial, "ID", "Nombre", objForeCastItem.AnioSemanaInicial.ID);
            ViewBag.AnioFinal = new SelectList(lstAnios, "ID", "Nombre", objForeCastItem.AnioSemanaFinal.ANO);
            ViewBag.SemanasFinal = new SelectList(lstSemanasFinal, "ID", "Nombre", objForeCastItem.AnioSemanaFinal.ID);
            ViewBag.lstArchivosConsolidados = new SelectList(lstConsolidado, "ID", "Nombre");
            ViewBag.Canal = ddlCanal;
            ViewBag.IDForeCast = xIDForeCast;
            ViewBag.IDModificacion = hdfIDModificacion;

            return PartialView();

        }


        /// <summary>
        /// Metodo encargando de realizar importación de datos
        /// </summary>
        /// <returns></returns>
        public JsonResult procesarImportacionDatos()
        {
            clsResultadoJson objResultado = new clsResultadoJson();

            try
            {
                long xIDForeCast = Convert.ToInt64(Request.Params["hdfIDForeCast"].ToString());
                string strCanal = Request.Params["hdfCanal"].ToString();
                int xSemanaInicial = Convert.ToInt32(Request.Params["ddlSemanaInicialImport"].ToString());
                int xSemanaFinal = Convert.ToInt32(Request.Params["ddlSemanaFinalImport"].ToString());
                string xTipoProceso = Request.Params["chkOpcion"].ToString();
                long xIDArchivoImportacion = Convert.ToInt64(Request.Params["ddlArchivoImportar"].ToString());
                string xIDModificacion = Request.Params["hdfIdModificacion"].ToString();

                clsForeCastBD objForeCastBD = new clsForeCastBD();
                List<clsAppNetItemImportacion> lstImportacion = new List<clsAppNetItemImportacion>();
                objResultado = objForeCastBD.procesarImportacionDatos(xIDForeCast, strCanal, xSemanaInicial, xSemanaFinal, xTipoProceso, xIDArchivoImportacion, this.Usuario, xIDModificacion, lstImportacion);
                objResultado.objResultado = lstImportacion;

            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }

            var jsonResult = Json(objResultado, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = 50000000;
            return jsonResult;
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de validaciones del archivo del ForeCast
        /// </summary>
        /// <param name="xIDArchivoForeCast"></param>
        /// <returns></returns>
        public PartialViewResult ConsultaListadoValidacionArchivo(long xIDArchivoForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Archivos_Validaciones> lstArchivo = objForeCastBD.consultaListadoValidacioneArchivo(xIDArchivoForeCast);
            return PartialView(lstArchivo);
        }

        #endregion

        #region CONSOLIDACION DE FORECAST

        /// <summary>
        /// Metodo utilizado para consolidar foreCast en un solo ID de proyeccion
        /// </summary>
        /// <returns></returns>
        public ViewResult Consolidacion()
        {
            return View();
        }

        /// <summary>
        /// Carga consolidación por ID
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CreaModificaConsolidacion(long IDConsolidacion)
        {
            clsAppNetConsolidacion_Encabezado objConsolidacion = new clsAppNetConsolidacion_Encabezado();

            clsForeCastBD objForeCastBD = new clsForeCastBD();
            clsGeneralBD objGeneralBD = new clsGeneralBD();

            objConsolidacion.lstForeCast.AddRange(
                objForeCastBD.consultaDetalleForeCastAConsolidar(new clsAppNetProyeccionForeCast_Filtro()
                {
                    BasEstadoProyeccionF = new clsEstadoProyeccion() { ID = 1 }
                }, this.Usuario, IDConsolidacion)
            );

            List<clsEstadoProyeccion> lstEstado = objGeneralBD.ConsultaListaEstados(2);
            ViewBag.Estados = new SelectList(lstEstado, "ID", "Nombre");
            objConsolidacion.ID = IDConsolidacion;

            return PartialView(objConsolidacion);
        }

        /// <summary>
        /// Metodo utilizado para guardar la consolidación de ForeCast
        /// </summary>
        /// <returns></returns>
        public JsonResult GuardarConsolidacion()
        {
            clsResultadoJson objResultado = new clsResultadoJson();

            try
            {
                string xID = Request.Params["hdfID"].ToString();
                string xNombre = Request.Params["Nombre"].ToString();
                string xDescripcion = Request.Params["Descripcion"].ToString();
                int xIDEstado = 4;
                int xIDForeCast = Convert.ToInt32(Request.Params["HdfForeCastSeleccionado"].ToString());

                clsAppNetConsolidacion_Encabezado objEncabezado = new clsAppNetConsolidacion_Encabezado();
                objEncabezado.ID = Convert.ToInt64(xID);
                objEncabezado.Nombre = xNombre;
                objEncabezado.Descripcion = xDescripcion;
                objEncabezado.IDAppNetEstados = xIDEstado;
                objEncabezado.IDAppNetUsuarioCreacion = this.Usuario;
                objEncabezado.lstForeCast.Add(new clsAppNetConsolidacion_Detalle_ForeCast()
                {
                    objAppNetProyeccionForeCast = new clsAppNetProyeccionForeCast() { ID = xIDForeCast }
                });

                clsForeCastBD objForeCastBD = new clsForeCastBD();
                objResultado = objForeCastBD.guardarConsolidadoForeCast(objEncabezado);
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }

            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para realizar la consulta de consolidados realizados
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <returns></returns>
        public PartialViewResult consultaListadoConsolidado(clsAppNetProyeccionForeCast_Filtro objFiltro)
        {
            List<clsAppNetConsolidacion_Encabezado> lstConsolidado = new List<clsAppNetConsolidacion_Encabezado>();
            clsForeCastBD objForecast = new clsForeCastBD();

            lstConsolidado = objForecast.consultaListadoConsolidacionForeCast(objFiltro);

            return PartialView(lstConsolidado);
        }

        /// <summary>
        /// Eliminar consolidacion de ForeCast seleccionados
        /// </summary>
        /// <param name="xItemsSeleccionados"></param>
        /// <returns></returns>
        public JsonResult eliminarConsolidadosDeForeCast(string xItemsSeleccionados)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsForeCastBD objForeCast = new clsForeCastBD();

            objResultado = objForeCast.eliminarConsolidacionForeCast(xItemsSeleccionados, this.Usuario);

            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para disparar hilos en el servidor
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public JsonResult iniciaProcesoConsolidacion(int xIDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsProcesos objProceso = new clsProcesos();
            objProceso.Nombre = "Ejecución de consolidación de datos y calculo de tallos.";
            objProceso.objProceso = new System.Threading.Thread(ejecutarProcesoConsolidacion);
            objProceso.objProceso.Start(xIDForeCast);
            lstProcesos.Add(objProceso);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo encargado de iniciar consolidado de ForeCast
        /// </summary>
        /// <param name="xIDForeCast"></param>
        private void ejecutarProcesoConsolidacion(object objIDForeCast)
        {
            int xIDForeCast = Convert.ToInt32(objIDForeCast);
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            try
            {
                objForeCastBD.consultaListadoConsolidacionForeCast(xIDForeCast);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Metodo utilizado para generar listado de errores de la consolidación
        /// </summary>
        /// <param name="xIDConsolidacion"></param>
        /// <returns></returns>
        public PartialViewResult consultarListadoErroresConsolidacion(int xIDConsolidacion)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Archivos_Validaciones> lstValidaciones = objForeCastBD.consultaListadoValidacionConsolidacion(xIDConsolidacion);
            return PartialView(lstValidaciones);
        }

        /// <summary>
        /// Carga ventana para asignar productos al cliente
        /// </summary>
        /// <param name="XCliente"></param>
        /// <param name="xCanal"></param>
        /// <returns></returns>
        public PartialViewResult ConsultaProductosSinAsignar(int xIntIDUnico, int XCliente, string xCanal)
        {
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<SelectListItem> lstCanal = new List<SelectListItem>();
            List<clsCanal> lstCanales = objProyeccionBD.consultaCanalesUsuarioYForeCast((clsAppNetUsuarios)Session["UserData"], xIntIDUnico);
            List<clsCliente> lstClientes = objForeCastBD.consultaListadoClienteXCanal(xCanal);

            /*Carga Canales*/
            lstCanales.ForEach(delegate (clsCanal objProteccion)
            {
                SelectListItem objItemNuevo = new SelectListItem();
                objItemNuevo.Value = objProteccion.Nombre;
                objItemNuevo.Text = objProteccion.Nombre;
                lstCanal.Add(objItemNuevo);
            });


            ViewBag.Canales = new SelectList(lstCanal, "Value", "Text", xCanal);
            ViewBag.Clientes = new SelectList(lstClientes, "ID", "NOmbre", XCliente);

            return PartialView();
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de productos sin asignar
        /// </summary>
        /// <param name="xIDUnico"></param>
        /// <param name="xCLiente"></param>
        /// <param name="xCanal"></param>
        /// <returns></returns>
        public JsonResult ConsultaListadoProductosSinAsignar(int xIDUnico, string xCLiente, string xCanal, string xFiltro)
        {
            clsResultadoJson objresultado = new clsResultadoJson();
            clsForeCastBD objForeCastBD = new clsForeCastBD();

            List<clsProducto> lstProductos = objForeCastBD.consultarProductosSinProyectar(xIDUnico, xCLiente, xCanal, xFiltro);
            objresultado.objResultado = lstProductos;

            return Json(objresultado);
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de clientes por canal
        /// </summary>
        /// <param name="strCanal"></param>
        /// <returns></returns>
        public JsonResult ConsultaListadoClientesXCanaJson(string strCanal)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsCliente> lstClientes = objForeCastBD.consultaListadoClienteXCanal(strCanal);
            return Json(new clsResultadoJson() { objResultado = lstClientes });
        }

        /// <summary>
        /// Metodo utilizado para cargar nuevos productos a proyectar a los clientes
        /// </summary>
        /// <param name="xCanal"></param>
        /// <param name="xCliente"></param>
        /// <param name="xProductos"></param>
        /// <returns></returns>
        public JsonResult cargaNuevosProductosAProyectarCliente(int xIntIDUnico, string xCanal, int xCliente, string xProductos)
        {
            clsAppNetUsuarios objUsuario = (clsAppNetUsuarios)Session["UserData"];
            clsForeCastBD objForeCasBD = new clsForeCastBD();
            return Json(new clsResultadoJson() { objResultado = objForeCasBD.ProductosyClientesProyectarNuevos(xIntIDUnico, xCanal, xCliente, xProductos, objUsuario) });
        }

        /// <summary>
        /// Metodo utilizado para eiminar producto de la proyeccion
        /// </summary>
        /// <param name="xIntIDUnico"></param>
        /// <param name="xCanal"></param>
        /// <param name="xCliente"></param>
        /// <param name="xProductos"></param>
        public JsonResult eliminarProductoVinculadoProyeccion(int xIntIDUnico, string xCanal, int xCliente, int xProductos)
        {
            clsAppNetUsuarios objUsuario = (clsAppNetUsuarios)Session["UserData"];
            clsForeCastBD objForeCasBD = new clsForeCastBD();
            clsResultadoJson objResultado = objForeCasBD.eliminarProductoDeProyeccion(xIntIDUnico, xCanal, xCliente, xProductos, objUsuario);
            return Json(objResultado);
        }

        /// <summary>
        /// Muestra ventana para exportar datos a excel
        /// </summary>
        /// <param name="IDUnico"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        public PartialViewResult seleccionarTipoArchivoExcel(long IDUnico, string tipoDocumento)
        {
            List<SelectListItem> lstItem = new List<SelectListItem>();
            lstItem.Add(new SelectListItem() { Value = "CAJA", Text = "Datos a nivel de caja" });
            lstItem.Add(new SelectListItem() { Value = "VARI", Text = "Datos a nivel de variedad" });
            lstItem.Add(new SelectListItem() { Value = "VARI_Access", Text = "Archivo plano comparación Access" });
            ViewBag.TipoArchivoExportacion = new SelectList(lstItem, "Value", "Text");
            ViewBag.IDForeCast = IDUnico;
            ViewBag.TipoArchivo = tipoDocumento;
            return PartialView();
        }

        /// <summary>
        /// Metodo utilizado para generar archivo EXCEL por stream
        /// </summary>
        /// <param name="hdfIDForeCsa"></param>
        /// <param name="hdfTipoArchivo"></param>
        /// <param name="ddlTipoExportacionArchivo"></param>
        /// <returns></returns>
        public void GenerarExcelParametros(long hdfIDForeCsa, string hdfTipoArchivo, string ddlTipoExportacionArchivo)
        {
            try
            {
                clsForeCastBD objForeCast = new clsForeCastBD();
                List<clsAppNetProyeccionForeCast> lstForeCast = objForeCast.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro() { IDF = hdfIDForeCsa }, this.Usuario);
                if (lstForeCast.Count > 0)
                {

                    clsAppNetProyeccionForeCast ObjForeCast = lstForeCast[0];
                    string strTipoArchivo = hdfTipoArchivo == "preFor" ? "AD" : "CO";
                    string strTipoExportacion = ddlTipoExportacionArchivo == "VARI" ? "NivelVariedades" : "NivelCajas";
                    string strFechaActual = DateTime.Now.ToString("mm_dd_yyyy_HH_MM_ss");
                    string strNombreArchivo = "Consolidado_" + ObjForeCast.Bas_TipoDatoPYG.Nombre + "_" + strTipoArchivo + "_" + strTipoExportacion + "_" + strFechaActual;

                    System.Data.DataTable dttDatos = objForeCast.consultaDatosForeCastExcel(this.Usuario, hdfIDForeCsa, hdfTipoArchivo, ddlTipoExportacionArchivo);

                    if (ddlTipoExportacionArchivo != "VARI_Access")
                    {
                        SpreadsheetLight.SLDocument objExcel = new SpreadsheetLight.SLDocument();
                        objExcel = ExcelAdministrador.MapeoDataSetTOExcel(objExcel, dttDatos, "Productos proyectados");
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + strNombreArchivo + ".xlsx");
                        objExcel.SaveAs(Response.OutputStream);
                        Response.End();
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream();
                        TextWriter tw = new StreamWriter(ms);

                        string strCadena = string.Empty;
                        for (int j = 0; j < dttDatos.Columns.Count; j++)
                        {
                            strCadena += dttDatos.Columns[j].ColumnName.ToString() + ";";
                        }
                        tw.WriteLine(strCadena.Substring(0, strCadena.Length - 1));

                        /*RECORRE LINEA A LINEA*/
                        for (int i = 0; i < dttDatos.Rows.Count; i++)
                        {
                            strCadena = string.Empty;
                            for (int j = 0; j < dttDatos.Columns.Count; j++)
                            {
                                strCadena += dttDatos.Rows[i][j].ToString() + ";";
                            }
                            tw.WriteLine(strCadena.Substring(0, strCadena.Length - 1));
                        }

                        tw.Flush();
                        byte[] bytes = ms.ToArray();
                        ms.Close();

                        Response.Clear();
                        Response.ContentType = "application/force-download";
                        Response.AddHeader("content-disposition", "attachment;filename="+ strNombreArchivo + ".txt");
                        Response.BinaryWrite(bytes);
                        Response.End();

                    }

                }
                else
                {
                    Response.Write("<h2>No existe ninguna proyección con los parametros indicados.</h2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<h2>" + ex.Message + "</h2>");
            }
        }

        /// <summary>
        /// Metodo utilzado para generar consolidado por canal
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public PartialViewResult GenerarGraficaConsolidadoPorCanal(long xIDForeCast)
        {
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            clsProyeccionVenta objProyeccion = objProyeccionBD.consultaGraficaConsolidadoForeCast(xIDForeCast, this.Usuario);
            return PartialView(objProyeccion);
        }

        /// <summary>
        /// metod utilizado para calcular resumen generall de ForeCast
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public PartialViewResult resumenGenerarForeCast(long xIDForeCast)
        {
            clsForeCastBD objForeCastBD = new clsForeCastBD();
            List<clsResumenGenerarForeCast> lstResumen = objForeCastBD.consultaResumenGeneralForeCast(xIDForeCast, this.Usuario);
            clsAppNetProyeccionForeCast objForeCast = objForeCastBD.consultaForeCast(new clsAppNetProyeccionForeCast_Filtro() { IDF = xIDForeCast }, this.Usuario)[0];
            ViewBag.MensajeError = objForeCastBD.cadenaError;
            ViewBag.ForeCast = objForeCast;
            return PartialView(lstResumen);
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de inconsistencias.
        /// </summary>
        /// <param name="xCanal">Canal</param>
        /// <param name="xAnio">Año</param>
        /// <param name="xIDForeCast">ID ForeCast</param>
        public PartialViewResult consultaInconsistenciaCanalAnio(string xCanal, int xAnio, long xIDForeCast)
        {
            clsForeCastBD objForeCast = new clsForeCastBD();
            List<clsAppNetProyeccionForeCast_Items> lstItems = objForeCast.consultaListadoInconsistencias(xCanal, xAnio, xIDForeCast);
            ViewBag.MensajeError = objForeCast.conErrores ? objForeCast.cadenaError : string.Empty;
            return PartialView(lstItems);
        }

        /// <summary>
        /// Metodo utilizado para exportar datos a Gestion Comercia
        /// </summary>
        /// <param name="xIDForeCast"></param>
        /// <returns></returns>
        public JsonResult ExportarDatosGestionComercial(long xIDForeCast)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsForeCastBD objForeCast = new clsForeCastBD();
            objResultado = objForeCast.exportarDatosGestionComercial(xIDForeCast);
            return Json(objResultado);
        }

        #endregion


    }
}