using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using GestionDocumental.Models.ControladorDatos;
using GestionDocumental.Models.Clases.Radicacion;


namespace GestionDocumental.Controllers
{
    public class BasicoController : Controller
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


        // GET: Basico
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ViewResult ConfiguracionDocumentos()
        {
            return View();
        }

        /// <summary>
        /// Metodo para cargar emisor
        /// </summary>
        /// <returns></returns>
        public ViewResult Emisor()
        {
            return View();
        }

        /// <summary>
        /// Metodo utilizado para listar los emisores
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Emisor_Listados(string xID, string xNumero, string xNombre)
        {
            clsBasico objBasico = new clsBasico();
            int xIDEntero = clsGeneralBD.validaCadenaNUllAEnteroMenosUno(xID);
            List<clsAppNetEmisor> lstEmisor = objBasico.consultaListadoEmisor(xIDEntero, clsGeneralBD.validaCadenaNUllAMenosUno(xNumero), clsGeneralBD.validaCadenaNUllAMenosUno(xNombre));
            return PartialView(lstEmisor);
        }

        /// <summary>
        /// Consulta administrador
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        public PartialViewResult AdministrarEmisor(int xID)
        {
            clsBasico objBasico = new clsBasico();
            clsAppNetEmisor objItemEmisor = xID != -1 ? objBasico.consultaListadoEmisor(xID, "-1", "-1")[0] : new clsAppNetEmisor();
            List<clsAppNetTipoNumero> lstTipoNumeroDocumento = objBasico.consultaTipoNumero();
            List<clsAppNetEstadoProceso> lstEstadoEmisorDocumento = objBasico.consultaEstadoPorProceso(3); /*ESTADO EMISOR*/

            ViewBag.tipoNumeroDocumento = new SelectList(lstTipoNumeroDocumento, "ID", "Nombre", objItemEmisor.IDAppNetTipoNumero);
            ViewBag.estadosEmisor = new SelectList(lstEstadoEmisorDocumento, "ID", "Nombre", objItemEmisor.IDAppNetEstados);
            return PartialView(objItemEmisor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEmisor"></param>
        /// <returns></returns>
        public JsonResult GuardaModificaEmisor(clsAppNetEmisor objEmisor)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsBasico objBasico = new clsBasico();
            objEmisor.IDAppNetAdministrador = this.Usuario.IDAdministrador;
            objResultado = objBasico.GuardarModificarEmisor(objEmisor);
            return Json(objResultado);
        }

        /// <summary>
        /// Metodo utilizado para listar los emisores
        /// </summary>
        /// <returns></returns>
        public JsonResult Emisor_ListadosJson(string xFiltro)
        {
            clsResultadoJson objResultadoJson = new clsResultadoJson();
            long xLonNumero = 0;
            string xNumero = "-1";
            string xNombre = "-1";
            if (long.TryParse(xFiltro, out xLonNumero))
            {
                xNumero = xFiltro;
            }
            else
            {
                xNombre = xFiltro;
            }
            clsBasico objBasico = new clsBasico();
            List<clsAppNetEmisor> lstEmisor = objBasico.consultaListadoEmisor(-1, xNumero, xNombre);
            objResultadoJson.objResultado = lstEmisor;
            return Json(objResultadoJson);
        }

        /// <summary>
        /// Vista de flujo documental
        /// </summary>
        /// <returns></returns>
        public ViewResult FlujoDocumental()
        {
            return View();
        }

        /// <summary>
        /// Metodo utilizado para listar los emisores
        /// </summary>
        /// <returns></returns>
        public PartialViewResult FlujoDocumental_Listados(string xID, string xNombre)
        {
            clsBasico objBasico = new clsBasico();
            clsRadicacionBD objRadicaBD = new clsRadicacionBD();
            int xIDEntero = clsGeneralBD.validaCadenaNUllAEnteroMenosUno(xID);
            List<clsAppNetFlujoEstados> lstEstados = objRadicaBD.consultaListadoEstados(this.Usuario, xIDEntero, clsGeneralBD.validaCadenaNUllAMenosUno(xNombre));
            return PartialView(lstEstados);
        }

        /// <summary>
        /// Metodo utilizado para administrar flujo de documentos
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        public PartialViewResult FlujoDocumentalAdministrador(int xID)
        {
            clsAppNetFlujoEstados objEstados;
            clsRadicacionBD objRadicaBD = new clsRadicacionBD();
            if (xID == -1)
            {
                objEstados = new clsAppNetFlujoEstados();
            }
            else
            {
                objEstados = objRadicaBD.consultaListadoEstados(this.Usuario, xID)[0];
            }
            ViewBag.Guid = Guid.NewGuid().ToString();
            objRadicaBD.cargaTablaTemporalAcciones(ViewBag.Guid, xID);
            return PartialView(objEstados);
        }


        /// <summary>
        /// Metodo utilizado para administrar flujo de documentos
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        public PartialViewResult FlujoDocumentalAdministrador_Responsables(string xIDGuid)
        {
            List<clsAppNetFlujoEstados_Acciones> objAcciones;
            clsRadicacionBD objRadicaBD = new clsRadicacionBD();
            objAcciones = objRadicaBD.cargaTemporalPorGuid(xIDGuid);
            return PartialView(objAcciones);
        }



        /// <summary>
        /// Metodo utilizado para administrar flujo de documentos por ID
        /// </summary>
        /// <param name="xID"></param>
        /// <returns></returns>
        public PartialViewResult FlujoDocumentalAdministrador_XAccion(string xIDGuid, int xID)
        {
            List<clsAppNetFlujoEstados_Acciones> objAcciones;
            clsRadicacionBD objRadicaBD = new clsRadicacionBD();
            objAcciones = objRadicaBD.cargaTemporalPorGuid(xIDGuid);
            clsAppNetFlujoEstados_Acciones objAccion = null;
            if (xID != -1)
            {
                objAccion = objAcciones.Find(delegate (clsAppNetFlujoEstados_Acciones objAccionItem) { return objAccionItem.ID == xID; });
            }
            else
            {
                objAccion = new clsAppNetFlujoEstados_Acciones();
            }

            List<clsAppNetFlujoEstados> lstFlujo = new List<clsAppNetFlujoEstados>();
            lstFlujo.AddRange(objRadicaBD.consultaListadoEstados(this.Usuario));

            ViewBag.lstEstados = new SelectList(lstFlujo, "ID", "Nombre");
            ViewBag.lstEstadosData = lstFlujo;

            objAccion.guidUnico = xIDGuid;
            return PartialView(objAccion);
        }

        /// <summary>
        /// Guarda acción
        /// </summary>
        /// <param name="objAccion"></param>
        /// <returns></returns>
        public JsonResult GuardarAccionTemporal(clsAppNetFlujoEstados_Acciones objAccion)
        {
            clsResultadoJson objEstado = new clsResultadoJson();
            clsRadicacionBD objRadicacion = new clsRadicacionBD();

            objEstado = objRadicacion.guardarAccionTemporal(objAccion, this.Usuario);

            return Json(objEstado);
        }

    }
}