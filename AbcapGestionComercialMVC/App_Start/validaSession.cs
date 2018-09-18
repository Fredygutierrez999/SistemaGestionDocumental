using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.ControladorDatos;

namespace AbcapGestionComercialMVC
{
    public class validaSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Validar la información que se encuentra en la sesión.
            if (HttpContext.Current.Session["USERDATA"] == null)
            {
                // Si la información es nula, redireccionar a 
                // página de error u otra página deseada.
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                    { "Controller", "Seguridad" },
                    { "Action", "InicioSesion" }
                });
            }
            else
            {
                clsAppNetUsuarios objUsuario = (clsAppNetUsuarios)HttpContext.Current.Session["USERDATA"];
                /*la combinación de controlador y accion debe existir en los permisos para poder acceder -  de lo contrario redirecciona al Index - Home*/
                if (!objUsuario.Permisos.Exists(delegate (clsAppNetMenu_Acciones objItemMenu)
                {
                    return objItemMenu.Controlador.ToUpper().Trim() == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper().Trim() &&
                    objItemMenu.Accion.ToUpper().Trim() == filterContext.ActionDescriptor.ActionName.ToUpper().Trim();
                }))
                {
                    filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary {
                    { "Controller", "Home" },
                    { "Action", "Index" }
               });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}