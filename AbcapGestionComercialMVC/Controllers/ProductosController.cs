using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases.Productos;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases;

namespace AbcapGestionComercialMVC.Controllers
{
    [AbcapGestionComercialMVC.validaSession]
    public class ProductosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Metodo utilizado para cargar los productos de en la proyeccion
        /// </summary>
        /// <param name="txtCodigo"></param>
        /// <param name="txtNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoProductos(string txtCodigo, string txtNombre)
        {
            clsProductoBD objProductoBD = new clsProductoBD();
            List<clsProductosSincronizacion> lstProductos = objProductoBD.consultaListadoProductos(clsGeneralBD.validaCadenaNUllAMenosUno(txtCodigo), clsGeneralBD.validaCadenaNUllAMenosUno(txtNombre));
            return PartialView(lstProductos);
        }

        public ActionResult Sincronizacion() {
            return View();
        }

        /// <summary>
        /// Metodo utilizado para cargar los productos a sincronizados
        /// </summary>
        /// <param name="txtCodigo"></param>
        /// <param name="txtNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoProductosASincronizar(string txtCodigo, string txtNombre) {
            clsProductoBD objProductoBD = new clsProductoBD();
            List<clsProductosSincronizacion> lstProductos = objProductoBD.consultaListadoProductosParaSincronizar(clsGeneralBD.validaCadenaNUllAMenosUno(txtCodigo), clsGeneralBD.validaCadenaNUllAMenosUno(txtNombre));
            return PartialView(lstProductos);
        }


        /// <summary>
        /// Procedimiento utilizado para sincronizar productos desde producción
        /// </summary>
        /// <param name="xIDProducto"></param>
        /// <returns></returns>
        public JsonResult SincronizarProductoProduccion(int xIDProducto) {
            clsResultadoJson objResultado = new clsResultadoJson();

            clsProductoBD objProductoBD = new clsProductoBD();
            objResultado = objProductoBD.ImportarProductoProyeccion(xIDProducto);

            return Json(objResultado);
        }


        /// <summary>
        /// Procedimiento utilizado para sincronizar productos desde producción
        /// </summary>
        /// <param name="xIDProducto"></param>
        /// <returns></returns>
        public JsonResult ActualizarProductoProduccion(int xIDProducto)
        {
            clsResultadoJson objResultado = new clsResultadoJson();

            clsProductoBD objProductoBD = new clsProductoBD();
            objResultado = objProductoBD.ActualizarProductoProyeccion(xIDProducto);
          
            return Json(objResultado);
        }


    }
}