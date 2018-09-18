using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionDocumental.Controllers
{
    public class BasicoController : Controller
    {
        // GET: Basico
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ViewResult ConfiguracionDocumentos() {
            return View();
        }

    }
}