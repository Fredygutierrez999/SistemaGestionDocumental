using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsConfiguracionConexion
    {
        /// <summary>
        /// Carga cadena de conexion a la base de datos de produccion
        /// </summary>
        public static string cadenaConexionBDProduccion
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["defaultConnString"].ToString();
            }
        }

        /// <summary>
        /// Carga cadena de conexion a la base de datos de PQR
        /// </summary>
        public static string cadenaConexionPQR
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConexionPQR"].ToString();
            }
        }

        /// <summary>
        /// Servidor de directorio activo
        /// </summary>
        public static string servidorDirectorioActivo
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPDirectoryPath"].ToString();
            }
        }

        /// <summary>
        /// Servidor de directorio activo
        /// </summary>
        public static string usuarioDirectorioActivo
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPUsuario"].ToString();
            }
        }

        /// <summary>
        /// Servidor de directorio activo
        /// </summary>
        public static string claveDirectorioActivo
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPClave"].ToString();
            }
        }

        /// <summary>
        /// Dominio del directorio activo
        /// </summary>
        public static string dominiodirectorioActivo
        {
            get
            {
                return ConfigurationManager.AppSettings["domain"].ToString();
            }
        }

        /// <summary>
        /// Indica si el logue se hace por directorio activo [true] 0 desde base de datos [false]
        /// </summary>
        public static bool validarDesdeDirectorio
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["VALIDARDESDEDIRECTORIOACTIVO"].ToString());
            }
        }

        /// <summary>
        /// Indica si el direcctorio donde se carga la imagen es local
        /// </summary>
        public static bool direccionLocalCargaArchivos
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["directorioLocalApp"].ToString());
            }
        }

        /// <summary>
        /// Indica el directorio donde se carga la imagen
        /// </summary>
        public static string carpetaCargaArchivos
        {
            get
            {
                return ConfigurationManager.AppSettings["carpetaSubidaArchivos"].ToString();
            }
        }

        /// <summary>
        /// Indica el direcctorio temporal donde se carga la imagen
        /// </summary>
        public static string carpetaCargaArchivosTemporal
        {
            get
            {
                return ConfigurationManager.AppSettings["carpetaTemporal"].ToString();
            }
        }

        /// <summary>
        /// Indica si el logue se hace por directorio activo [true] 0 desde base de datos [false]
        /// </summary>
        public static bool validarUsuarioALRegistrarDALP
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["validarExistenciaUsuarioEnLDAPRegistro"].ToString());
            }
        }

        /// <summary>
        /// Habilita el envio del correo
        /// </summary>
        public static bool habilitaEnvioCorreo
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["habilitarEnvioCorreo"].ToString());
            }
        }

        /// <summary>
        /// SMTP
        /// </summary>
        public static string CorreoSmtp
        {
            get
            {
                return ConfigurationManager.AppSettings["smtp"].ToString();
            }
        }

        /// <summary>
        /// Usuario correo
        /// </summary>
        public static string UsuarioCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["usuariosmtp"].ToString();
            }
        }

        /// <summary>
        /// Clave correo
        /// </summary>
        public static string ClaveCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["clavesmtp"].ToString();
            }
        }

        /// <summary>
        /// puerto correo
        /// </summary>
        public static int puertoSmtp
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["puertosmtp"].ToString());
            }
        }

        /// <summary>
        /// SSL
        /// </summary>
        public static bool habilitaSSL
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
            }
        }

        /// <summary>
        /// NombreComprobanteNota
        /// </summary>
        public static string nombreComprobanteNota
        {
            get
            {
                return ConfigurationManager.AppSettings["NombreNota"].ToString();
            }
        }


        /// <summary>
        /// Carga cadena de conexion a la base de datos de produccion
        /// </summary>
        public static string carpetaServidorNotas
        {
            get
            {
                return ConfigurationManager.AppSettings["UbicacionServidorReporte"].ToString();
            }
        }

        /// <summary>
        /// Carga cadena de conexion a la base de datos de produccion
        /// </summary>
        public static string prefijoArchivoNota
        {
            get
            {
                return ConfigurationManager.AppSettings["prefijoArchivoNota"].ToString();
            }
        }


        /// <summary>
        /// Ubicación carpeta
        /// </summary>
        public static string carpetaServidorProduccion02
        {
            get
            {
                return ConfigurationManager.AppSettings["UbicacionCarpetaArchivos"].ToString();
            }
        }

    }
}