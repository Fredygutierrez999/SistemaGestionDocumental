using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace GestionDocumental.Models.Clases
{
    public class clsConfiguracionAPP
    {


        public static string ubicacionApp(System.Web.HttpServerUtilityBase objControl) {
            return objControl.MapPath("/");
        }

        /// <summary>
        /// Carpeta temporal
        /// </summary>
        /// <returns></returns>
        public static string getCarpetaTemporal() {
            return ConfigurationManager.AppSettings["carpetaTemporal"].ToString();
        }

        /// <summary>
        /// Carpeta
        /// </summary>
        /// <returns></returns>
        public static string getCarpetaArchivos()
        {
            return ConfigurationManager.AppSettings["carpetaSubidaArchivos"].ToString();
        }

    }
}