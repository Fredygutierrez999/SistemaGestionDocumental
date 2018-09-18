using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases;
using Newtonsoft.Json;

namespace AbcapGestionComercialMVC.Controllers
{
    public class ControlProyeccionController : Controller
    {
        // GET: ControlProyeccion
        public ActionResult Index()
        {
            return View();
        }

        // GET: ControlProyeccion
        public ActionResult busquedaProyeccion(int ddlProyeccionVenta, string ddlCanal)
        {
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            objProyeccionBD.LimpiaSincronizacion(ddlProyeccionVenta, "100");

            clsProyeccionVenta objProyeccion = objProyeccionBD.consultarProyeccion(ddlProyeccionVenta, ddlCanal);
            return View("Index", objProyeccion);
        }

        /// <summary>
        /// Metodo utiilizado para sincronizar listado de movimientos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SincronizarBDTemporal(int xIdProvision,string strData, string IDModificacion)
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
        /// Get: Consultar proyecciones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult consultarProyecciones()
        {
            clsConsultaProyeccionFiltros objFiltros = new clsConsultaProyeccionFiltros();
            clsProyeccionVentaBD objProyeccionBD = new clsProyeccionVentaBD();
            List<clsProyeccionVenta> lstProyeccionVenta = objProyeccionBD.consultaBasicaProyecciones();
            List<clsCanal> lstCanales = objProyeccionBD.consultaCanales();
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

        /// <summary>
        /// Metodo encargado de crear nueva proyección
        /// </summary>
        /// <returns></returns>
        public PartialViewResult crearNuevaProyeccion() {
            return PartialView();
        }

        // GET: ControlProyeccion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControlProyeccion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControlProyeccion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ControlProyeccion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControlProyeccion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ControlProyeccion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControlProyeccion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
