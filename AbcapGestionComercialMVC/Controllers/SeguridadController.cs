using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;

namespace AbcapGestionComercialMVC.Controllers
{
    public class SeguridadController : Controller
    {

        #region ROLES
        /// <summary>
        /// Consulta de roles
        /// </summary>
        /// <returns></returns>
        [validaSession]
        public ViewResult Roles(string txtID, string txtNombre)
        {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<clsAppNetRoles> lstRoles = objSeguridad.consultaRoles(Convert.ToInt32(txtID), txtNombre);
            ViewBag.ListadoTipoRol = new SelectList(objSeguridad.listadoTipoRol(true), "ID", "Nombre");
            return View(lstRoles);
        }

        /// <summary>
        /// Consulta de roles
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [validaSession]
        public PartialViewResult ConsultDetalleRoles(string txtID, string txtNombre)
        {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<clsAppNetRoles> lstRoles = objSeguridad.consultaRoles(Convert.ToInt32((txtID == "" ? "0" : txtID)), txtNombre);
            return PartialView(lstRoles);
        }

        /// <summary>
        /// Crea o modifica un rol
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [validaSession]
        public PartialViewResult AdministrarRol(int xIDRol)
        {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<clsAppNetRoles> lstRoles = objSeguridad.consultaRoles(xIDRol, "-1");
            if (lstRoles.Count == 0)
            {
                clsAppNetRoles objRol = new clsAppNetRoles();
                lstRoles.Add(objRol);
            }

            List<clsAppNetTipoRol> lstTipoRol = objSeguridad.listadoTipoRol(false);
            ViewBag.ListadoTipoRolBD = lstTipoRol;
            ViewBag.ListadoTipoRol = new SelectList(lstTipoRol, "ID", "Nombre");
            ViewBag.ListadoSiglasCliente = objSeguridad.listadoSiglaAgrupacionCliente(true);
            ViewBag.ListadoMenu = objSeguridad.listadoMenuParaRol();
            return PartialView(lstRoles[0]);
        }

        /// <summary>
        /// Metodo encargado de Guardar cambios sobre el rol
        /// </summary>
        /// <param name="txtId"></param>
        /// <param name="txtNombre"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [validaSession]
        public JsonResult guardarRol(
            int hdfIdPopup, 
            string txtNombrePopup, 
            int ddlTipoRoloPopup,
            string ddlCanalPopup,
            string hdfSeleccionMenuPopup)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsSeguridad objSeguridad = new clsSeguridad();
            clsAppNetRoles objRol = new clsAppNetRoles();
            objRol.ID = Convert.ToInt64(hdfIdPopup);
            objRol.Nombre = txtNombrePopup;
            objRol.TipoRol = new clsAppNetTipoRol(ddlTipoRoloPopup, string.Empty, false);
            objRol.Canal = ddlCanalPopup;
            objRol.Estado = 1;
            objResultado = objSeguridad.GuardaRol(objRol, hdfSeleccionMenuPopup);
            return Json(objResultado);
        }
        #endregion

        #region USUARIOS

        /// <summary>
        /// Consulta de roles
        /// </summary>
        /// <returns></returns>
        [validaSession]
        public ViewResult Usuarios(string txtID, string txtNombre, string txtCorreo, string txtUserName)
        {
            return View();
        }

        /// <summary>
        /// Consulta de todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [validaSession]
        public PartialViewResult ConsultDetalleUsuarios(string txtID, string txtUsuario, string txtNombre, string txtCorreoElectronico)
        {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<clsAppNetUsuarios> lstUsuarios = objSeguridad.consultaUsuarios(Convert.ToInt32(clsConfgiruacionBD.valorEntero(txtID)), txtNombre, txtCorreoElectronico, txtUsuario);
            return PartialView(lstUsuarios);
        }

        /// <summary>
        /// Crea o modifica un rol
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [validaSession]
        public PartialViewResult AdministrarUsuario(int xIDUsuario)
        {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<clsAppNetUsuarios> lstUsuarios = objSeguridad.consultaUsuarios(xIDUsuario, "-1", "-1", "-1");
            if (lstUsuarios.Count == 0)
            {
                clsAppNetUsuarios objUsuario = new clsAppNetUsuarios();
                lstUsuarios.Add(objUsuario);
            }
            ViewBag.ListadoRoles = objSeguridad.consultaRoles(-1,"-1");
            return PartialView(lstUsuarios[0]);
        }

        /// <summary>
        /// Metodo encargado de Guardar cambios sobre el rol
        /// </summary>
        /// <param name="txtId"></param>
        /// <param name="txtNombre"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [validaSession]
        public JsonResult guardarUsuario(
            int hdfIdPopup,
            string txtUsuario,
            string txtClave,
            string txtNombrePopup,
            string txtCorreo,
            string txtApellidosPopup,
            string hdfSeleccionMenuPopup,
            bool chkAactivo)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            clsSeguridad objSeguridad = new clsSeguridad();
            clsAppNetUsuarios objUsuario = new clsAppNetUsuarios();
            objUsuario.ID = Convert.ToInt64(hdfIdPopup);
            objUsuario.UserName = txtUsuario;
            objUsuario.Clave = txtClave;
            objUsuario.Nombre = txtNombrePopup;
            objUsuario.Apellido = txtApellidosPopup;
            objUsuario.CorreoElectronico = txtCorreo;
            objUsuario.Estado = chkAactivo ? 1 : 0;
            objResultado = objSeguridad.GuardaUsuarios(objUsuario, hdfSeleccionMenuPopup);
            return Json(objResultado);
        }

        #endregion

        #region INICIO DE SESSION


        /// <summary>
        /// Pagina de inicio de sessión
        /// </summary>
        /// <returns></returns>
        public ViewResult InicioSesion() {
            return View(new clsAppNetUsuarios());
        }

        /// <summary>
        /// metodo encargado de validar el inicio se sesion de los usuarios
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult validarInicioSession(clsAppNetUsuarios objUsuario) {
            clsSeguridad objSeguridad = new clsSeguridad();
            List<string> strCadena = new List<string>();
            clsAppNetUsuarios objUsuarioBD = objSeguridad.validaDatosInicioSession(objUsuario, strCadena);
            if (strCadena.Count == 0)
            {
                objSeguridad.asignaPermisosUsuario(objUsuarioBD);
                string strUbicacionAPP = Server.MapPath("~");
                objSeguridad.crearMenuHTML(objUsuarioBD, strUbicacionAPP);
                Session["USERDATA"] = objUsuarioBD;
                return RedirectToAction("Index", "Home");
            }
            else {
                ViewBag.ListadoErrores = strCadena;
                return View("InicioSesion", objUsuario);
            }
        }

        /// <summary>
        /// Metodo encargado de cerrar sessión
        /// </summary>
        /// <returns></returns>
        public ActionResult CerrarSession() {
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("InicioSesion", "Seguridad");
        }

        #endregion

    }
}