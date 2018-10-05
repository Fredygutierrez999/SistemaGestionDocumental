using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AbcapGestionComercialMVC.Models.Clases.General;
using GestionDocumental.Models.Clases.Radicacion;
using GestionDocumental.Models.ControladorDatos;
using GestionDocumental.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Controllers
{
    public class RadicacionController : Controller
    {
        #region PROPIEDADES
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
        #endregion

        /// <summary>
        /// Metodo Index
        /// </summary>
        /// <param name="IDDocumento"></param>
        /// <returns></returns>
        public ActionResult Index(long? IDDocumento)
        {
            clsAppNetDocumentos objDocumento = null;
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            clsBasico objBasicoBD = new clsBasico();

            List<clsAppNetTipoDocumentos> lstTipoDocumento = null;

            if (IDDocumento == null)
            {
                objDocumento = new clsAppNetDocumentos();
            }
            else
            {
                long xIDDocumento = Convert.ToInt64(IDDocumento);
                List<clsAppNetDocumentos> lstDocumentos = objRadicacionBD.consultaListadoDocumentos(this.Usuario, new clsAppNetDocumentos_Filtros()
                {
                    xBID = xIDDocumento
                });
                if (lstDocumentos.Count == 1)
                {
                    objDocumento = lstDocumentos[0];
                }
            }
            objDocumento.AppNetFlujoEstados = objRadicacionBD.consultaEstado(this.Usuario, (int)objDocumento.AppNetFlujoEstados.ID);
            lstTipoDocumento = objRadicacionBD.consultaListadoTipoDocumento(this.Usuario);
            List<clsAppNetEmisor> lstEmisor = objBasicoBD.consultaListadoEmisor(-1, "-1", "-1");

            ViewBag.lstAcciones = new SelectList(objDocumento.AppNetFlujoEstados.lstAcciones, "ID", "Nombre");
            ViewBag.lstTiposDocumentos = new SelectList(lstTipoDocumento, "ID", "Nombre");
            ViewBag.lstEmisores = new SelectList(lstEmisor, "ID", "Nombre");

            return View(objDocumento);
        }

        /// <summary>
        /// Metodo utilizado para subir archivos
        /// </summary>
        /// <param name="xIDSession"></param>
        /// <returns></returns>
        public JsonResult subirArchivos(string xIDSession)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();

            string xNuevoDocumento = xIDSession.ToString().Replace("-", "");
            clsResultadoJson objJson = new clsResultadoJson();

            int xCantidadArchivo = Request.Files.Count;
            if (xCantidadArchivo > 0)
            {
                string xUrlUbicacion = clsConfiguracionAPP.ubicacionApp(Server);
                string xCarpetaTemporal = Path.Combine(xUrlUbicacion, clsConfiguracionAPP.getCarpetaTemporal());
                try
                {
                    Directory.CreateDirectory(xCarpetaTemporal);
                }
                catch (Exception ex)
                {
                    objJson.cargarErro(ex);
                }
                if (objJson.ResultadoProceso)
                {
                    List<clsAppNetDocumento_Soporte> lstSoporte = new List<clsAppNetDocumento_Soporte>();
                    clsBasicoConfiguracion objBasico = new clsBasicoConfiguracion();
                    for (int i = 0; i < xCantidadArchivo; i++)
                    {
                        string xNuevoNombre = Guid.NewGuid().ToString().Replace("-", "");
                        string xNombre = string.IsNullOrEmpty(Path.GetExtension(Request.Files[i].FileName)) ? "Abjunto_PDF_" + DateTime.Now.ToString("yyyy-mm-dd HH:MM") : Request.Files[i].FileName;
                        string xExtension = string.IsNullOrEmpty(Path.GetExtension(xNombre)) ? ".PDF" : Path.GetExtension(xNombre);
                        int xTamano = Request.Files[i].ContentLength;
                        string xSoloNombre = Path.GetFileNameWithoutExtension(xNombre);

                        //CREA OBJETO
                        clsAppNetDocumento_Soporte objSoporte = new clsAppNetDocumento_Soporte();
                        objSoporte.Nombre = xNombre;
                        objSoporte.SoloNombre = xSoloNombre;
                        objSoporte.Extension = xExtension;
                        objSoporte.Tamanio = xTamano;
                        objSoporte.NuevoNombre = xNuevoNombre;
                        objSoporte.guidDocumento = xNuevoDocumento;
                        objSoporte.Temporal = true;
                        objSoporte.ContendorImagen = objRadicacionBD.consultaTipoArchivo(objSoporte.Extension);

                        string xUbicacionArchivo = Path.Combine(xCarpetaTemporal, xNuevoNombre + xExtension);
                        Request.Files[i].SaveAs(xUbicacionArchivo);

                        //Guarda archivo temporal
                        objSoporte.ID = (long)objRadicacionBD.guardarArchivoTemporal(this.Usuario, objSoporte);

                        //Listado de archivos
                        lstSoporte.Add(objSoporte);

                    }
                    objJson.objResultado = lstSoporte;

                }
            }
            else
            {
                objJson.ResultadoProceso = false;
                objJson.MensajeProceso = "No se recibio ningun archivo para realizar la subida.";
            }

            return Json(objJson);
        }

        /// <summary>
        /// Metodo utilizado para guardar o modificar documento
        /// </summary>
        /// <param name="objDocumentos"></param>
        /// <returns></returns>
        public JsonResult Guardar(clsAppNetDocumentos objDocumentos)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                clsRadicacionBD objRadicacion = new clsRadicacionBD();
                objResultado = objRadicacion.Guardar(this.Usuario, objDocumentos, Server);
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para consultar documento
        /// </summary>
        /// <param name="xIDArchivo"></param>
        /// <param name="xTipoArchivo"></param>
        /// <returns></returns>
        public FileStreamResult CargaArchivo(long xIDArchivo, bool xTipoArchivo)
        {
            try
            {
                clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
                List<clsAppNetDocumento_Soporte> lstSoporte = objRadicacionBD.consultaSoporteStream(xIDArchivo, xTipoArchivo);
                if (lstSoporte.Count == 1)
                {
                    clsAppNetDocumento_Soporte objSoporte = lstSoporte[0];
                    string xstrPathArchivador = Path.Combine(clsConfiguracionAPP.ubicacionApp(Server), xTipoArchivo ? clsConfiguracionAPP.getCarpetaTemporal() : clsConfiguracionAPP.getCarpetaArchivos());
                    if (xTipoArchivo == false)
                    {
                        xstrPathArchivador = Path.Combine(xstrPathArchivador, objSoporte.IDAppNetAdministrador.ToString());
                        xstrPathArchivador = Path.Combine(xstrPathArchivador, objSoporte.IDAppNetDocumentos.ToString());
                    }
                    xstrPathArchivador = Path.Combine(xstrPathArchivador, objSoporte.Ubicacion);
                    Stream objStream = new FileStream(xstrPathArchivador, FileMode.Open, FileAccess.Read);
                    Response.AddHeader("Content-Disposition", "filename=" + objSoporte.Nombre);
                    return new FileStreamResult(objStream, objSoporte.ContentType);
                }
                else
                {
                    throw new Exception("No se puede cargar el archivo. Posiblemente existe mas de un registro en la base de datos o no existe ningun registro con los datos indicados.");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Metodo utilizado para consultar documentos asignados al usuario
        /// </summary>
        /// <returns></returns>
        public ViewResult MisDocumentos()
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();

            List<clsAppNetTipoDocumentos> lstTipoDocumento = new List<clsAppNetTipoDocumentos>();
            lstTipoDocumento.Add(new clsAppNetTipoDocumentos() { ID = -1, Nombre = "--No aplica--" });
            lstTipoDocumento.AddRange(objRadicacionBD.consultaListadoTipoDocumento(this.Usuario));

            List<clsAppNetFlujoEstados> lstFlujo = new List<clsAppNetFlujoEstados>();
            lstFlujo.Add(new clsAppNetFlujoEstados() { ID = -1, Nombre = "--Todos--" });
            lstFlujo.AddRange(objRadicacionBD.consultaListadoEstados(this.Usuario));

            ViewBag.lstTiposDocumentos = new SelectList(lstTipoDocumento, "ID", "Nombre");
            ViewBag.lstEstados = new SelectList(lstFlujo, "ID", "Nombre");
            ViewBag.lstEstadosData = lstFlujo;

            return View();
        }

        /// <summary>
        /// Metodo utilizado para consultar listado de documentos
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <returns></returns>
        public PartialViewResult MisDocumentos_Detalle(clsAppNetDocumentos_Filtros objFiltro)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            List<clsAppNetDocumentos> lstDocumento = objRadicacionBD.consultaListadoDocumentos(this.Usuario, objFiltro);
            ViewBag.permiteSeleccionar = true;
            return PartialView(lstDocumento);
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de documentos
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <returns></returns>
        public PartialViewResult MisDocumentos_Detalle_Todos(clsAppNetDocumentos_Filtros objFiltro)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            clsAppNetUsuarios objUsuarioTemporal = new clsAppNetUsuarios() { ID = -1, IDAdministrador = this.Usuario.IDAdministrador };
            List<clsAppNetDocumentos> lstDocumento = objRadicacionBD.consultaListadoDocumentos(objUsuarioTemporal, objFiltro);
            ViewBag.permiteSeleccionar = false;
            return PartialView("MisDocumentos_Detalle", lstDocumento);
        }


        /// <summary>
        /// Metodo utilizado para consultar todos documentos
        /// </summary>
        /// <returns></returns>
        public ViewResult ConsultaDocumentos()
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();

            List<clsAppNetTipoDocumentos> lstTipoDocumento = new List<clsAppNetTipoDocumentos>();
            lstTipoDocumento.Add(new clsAppNetTipoDocumentos() { ID = -1, Nombre = "--No aplica--" });
            lstTipoDocumento.AddRange(objRadicacionBD.consultaListadoTipoDocumento(this.Usuario));

            List<clsAppNetFlujoEstados> lstFlujo = new List<clsAppNetFlujoEstados>();
            lstFlujo.Add(new clsAppNetFlujoEstados() { ID = -1, Nombre = "--Todos--" });
            lstFlujo.AddRange(objRadicacionBD.consultaListadoEstados(this.Usuario));

            ViewBag.lstTiposDocumentos = new SelectList(lstTipoDocumento, "ID", "Nombre");
            ViewBag.lstEstados = new SelectList(lstFlujo, "ID", "Nombre");
            ViewBag.lstEstadosData = lstFlujo;
            return View();
        }

        /// <summary>
        /// Metodo utilizado para consultar movimientos de un documento
        /// </summary>
        /// <param name="xIDDocumento"></param>
        /// <returns></returns>
        public PartialViewResult MisDocumentos_Movimientos(long xIDDocumento)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            List<clsAppNetDocumentos_Movimientos> lstMovimientos = objRadicacionBD.consultaListadoMovimientos(this.Usuario, xIDDocumento);
            return PartialView(lstMovimientos);
        }

        /// <summary>
        /// Consulta estado por ID
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult consultaEstados(int xID)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            List<clsAppNetFlujoEstados> lstFlujo = objRadicacionBD.consultaListadoEstados(this.Usuario, xID);
            return Json(lstFlujo[0]);
        }


        /// <summary>
        /// Consulta estado por ID
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult consultaListadoResponsables(int xIDEstado, int xIDAccion)
        {
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            List<clsAppNetUsuarios> lstUsuario = objRadicacionBD.consultaListadoResponsables(xIDEstado, xIDAccion, this.Usuario);
            return Json(lstUsuario);
        }

        /// <summary>
        /// Metodo utilizado para asignar documentos asignados a siguiente estado
        /// </summary>
        /// <param name="xIDDocumentos"></param>
        /// <param name="xEstado"></param>
        /// <param name="xAccion"></param>
        /// <param name="xIDResponsable"></param>
        /// <returns></returns>
        public JsonResult asignacionDocumentosASiguienteEstado(string xIDDocumentos, int xEstado, int xAccion, int xIDResponsable)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsRadicacionBD objRadicacionBD = new clsRadicacionBD();
            objResultado = objRadicacionBD.asignacionFlujoDocumentos(xIDDocumentos, xEstado, xAccion, xIDResponsable, this.Usuario);
            return Json(objResultado);
        }

    }
}