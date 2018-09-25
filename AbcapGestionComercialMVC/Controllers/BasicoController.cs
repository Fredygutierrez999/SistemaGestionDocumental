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

    }
}