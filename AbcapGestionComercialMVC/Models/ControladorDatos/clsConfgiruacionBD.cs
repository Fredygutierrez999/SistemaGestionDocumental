using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using dataAccessManager.sql;
using System.Configuration;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsConfgiruacionBD
    {

        private string _strConexion;
        public dataAccessController objControlador;
        public int CERO = 0;
        public int UNO = 1;
        public int DOS = 2;
        public int TRES = 3;
        public int CUATRO = 4;
        public int CINCO = 5;
        public int SEIX = 6;
        public int SIETE = 7;
        public int OCHO = 8;
        public int NUEVE = 9;

        /// <summary>
        /// Constructor
        /// </summary>
        public clsConfgiruacionBD()
        {
            this._strConexion = ConfigurationManager.ConnectionStrings["defaultConnString"].ToString();
            this.objControlador = new dataAccessController(this._strConexion);
        }

        /// <summary>
        /// Metodo encargado de cargar detalles del error
        /// </summary>
        /// <param name="ex"></param>
        public void cargarErro(Exception ex)
        {
            this.cadenaError = ex.Message;
        }

        /// <summary>
        /// Cadena de conexión
        /// </summary>
        public string getCadenaconexion { get; set; }

        /// <summary>
        /// Con errores
        /// </summary>
        public bool conErrores { get; set; }

        /// <summary>
        /// Cadena de error
        /// </summary>
        public string cadenaError { get; set; }

        /// <summary>
        /// Metodo encargado de retornar un valor -1 si la cadena es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public string validaCadenaNUll(string xStrCandea)
        {
            return (xStrCandea == null ? "-1" : xStrCandea);
        }

        /// <summary>
        /// Valor entero
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static long valorEntero(string xStrCandea)
        {
            long xIDValorEntero = 0;
            if (!long.TryParse(xStrCandea, out xIDValorEntero))
            {
                xIDValorEntero = -1;
            }
            return xIDValorEntero;
        }

        /// <summary>
        /// Metodo encargado de retornar un valor -1 si la cadena es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static string validaCadenaNUllAVacio(string xStrCandea)
        {
            return (xStrCandea == null ? string.Empty : xStrCandea);
        }


        /// <summary>
        /// Metodo encargado de retornar un valor -1 si la cadena es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static string validaCadenaNUllAMenosUno(string xStrCandea)
        {
            return (xStrCandea == null ? "-1" : xStrCandea);
        }

        /// <summary>
        /// Metodo encargado de retornar un valor -1 si la cadena es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static int validaEnteroCeroAMenosUno(int xValor)
        {
            return xValor == 0 ? -1 : xValor;
        }

        /// <summary>
        /// Metodo encargado de retornar un valor -1 si la cadena es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static long validaEnteroCeroAMenosUno(long xValor)
        {
            return xValor == 0 ? -1 : xValor;
        }

        /// <summary>
        /// Metodo encargado de retornar un BDNULL si la fecha es nula
        /// </summary>
        /// <param name="xStrCandea"></param>
        /// <returns></returns>
        public static object validaFechaBDNUll(DateTime xValor)
        {
            if (xValor == DateTime.MinValue)
            {
                return DBNull.Value;
            }
            else
            {
                return xValor;
            }
        }

    }
}