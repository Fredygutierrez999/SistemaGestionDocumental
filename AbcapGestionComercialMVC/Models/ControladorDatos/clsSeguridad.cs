using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;
using Seguridad.LDAPSecurity;
using System.IO;
using System.Text;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsSeguridad : clsConfgiruacionBD
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public clsSeguridad() { }

        #region ROLES
        /// <summary>
        /// Metodo encargado de consultar roles
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetRoles> consultaRoles(int xId, string xNombre)
        {
            List<clsAppNetRoles> lstRoles = new List<clsAppNetRoles>();
            this.objControlador.setCommand("AppNetRoles_Consulta");
            this.objControlador.addNewParameter("@IDRol", (xId == 0 ? -1 : xId));
            if (xNombre == null)
            {
                this.objControlador.addNewParameter("@Nombre", "-1");
            }
            else
            {
                this.objControlador.addNewParameter("@Nombre", xNombre);
            }
            DataSet dtsDatos = this.objControlador.execDataSetResult();

            DataTable dttDatos = dtsDatos.Tables[0];
            DataTable dttMenu = dtsDatos.Tables[1];

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsAppNetRoles objRol = new clsAppNetRoles(dttDatos.Rows[i]);
                DataRow[] lstMenuDelRol = dttMenu.Select("IDRol = " + objRol.ID.ToString());
                for (int j = 0; j < lstMenuDelRol.Count(); j++)
                {
                    objRol.Menu.Add(new clsAppNetMenu(lstMenuDelRol[j]));
                }
                lstRoles.Add(objRol);
            }

            this.objControlador.closeConnection();
            return lstRoles;
        }

        /// <summary>
        /// Metodo encargado de guardar cambios del rol
        /// </summary>
        /// <param name="objRol"></param>
        /// <returns></returns>
        public clsResultadoJson GuardaRol(clsAppNetRoles objRol, string xstrIDMenu)
        {
            clsResultadoJson objresultado = new clsResultadoJson();
            try
            {
                List<string> lstDatos = new List<string>();
                if (string.IsNullOrEmpty(objRol.Nombre))
                {
                    lstDatos.Add("Debe indicar un nombre");
                }
                if (objRol.TipoRol == null)
                {
                    lstDatos.Add("Debe seleccionar el tipo de rol");
                }

                if (lstDatos.Count == 0)
                {
                    this.objControlador.setCommand("guardarCrearRol");
                    this.objControlador.addNewParameter("@ID", objRol.ID);
                    this.objControlador.addNewParameter("@Nombre", objRol.Nombre);
                    this.objControlador.addNewParameter("@Estado", objRol.Estado);
                    this.objControlador.addNewParameter("@IDtipoRol", objRol.TipoRol.ID);
                    this.objControlador.addNewParameter("@Canal", objRol.Canal);
                    this.objControlador.addNewParameter("@IDMenu", xstrIDMenu);

                    int xIDRol = (int)this.objControlador.execScalar();
                    if (objRol.ID == 0)
                    {
                        objresultado.MensajeProceso = "Rol creado con exito con el id " + xIDRol.ToString() + ".";
                    }
                    else
                    {
                        objresultado.MensajeProceso = "Rol modificado con exito con el id " + xIDRol.ToString() + ".";
                    }
                    this.objControlador.closeConnection();
                }
                else
                {
                    objresultado.MensajeProceso = lstDatos[0];
                }
            }
            catch (Exception ex)
            {
                objresultado.cargarErro(ex);
            }
            return objresultado;
        }

        /// <summary>
        /// Consulta listado de tipo de rol
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetTipoRol> listadoTipoRol(bool conDetalleDefault)
        {
            List<clsAppNetTipoRol> lstTipoRoles = new List<clsAppNetTipoRol>();
            this.objControlador.setCommand("AppNetTipoRol_Consulta");
            DataTable dttDatos = this.objControlador.execTableResult();
            if (conDetalleDefault)
            {
                lstTipoRoles.Add(new clsAppNetTipoRol() { ID = -1, Nombre = "Todos" });
            }
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                lstTipoRoles.Add(new clsAppNetTipoRol(dttDatos.Rows[i]));
            }
            this.objControlador.closeConnection();
            return lstTipoRoles;
        }

        /// <summary>
        /// Consulta listado de canal 
        /// </summary>
        /// <returns></returns>
        public SelectList listadoSiglaAgrupacionCliente(bool conDetalleDefault)
        {
            List<SelectListItem> lstTipoRoles = new List<SelectListItem>();
            this.objControlador.setCommand("consultarSiglasCliente");
            DataTable dttDatos = this.objControlador.execTableResult();
            if (conDetalleDefault)
            {
                lstTipoRoles.Add(new SelectListItem() { Value = "-1", Text = "VACIO" });
            }
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = dttDatos.Rows[i]["SiglaAgrupacionCliente"].ToString();
                objItem.Value = dttDatos.Rows[i]["SiglaAgrupacionCliente"].ToString();
                lstTipoRoles.Add(objItem);
            }
            this.objControlador.closeConnection();
            return new SelectList(lstTipoRoles, "Value", "Text");
        }

        /// <summary>
        /// Consulta listado de menú
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetMenu> listadoMenuParaRol()
        {
            List<clsAppNetMenu> lstTipoRoles = new List<clsAppNetMenu>();
            this.objControlador.setCommand("consultaMenuParaRol");
            DataTable dttDatos = this.objControlador.execTableResult();
            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsAppNetMenu objItem = new clsAppNetMenu();
                objItem.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                objItem.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                lstTipoRoles.Add(objItem);
            }
            this.objControlador.closeConnection();
            return lstTipoRoles;
        }
        #endregion



        /// <summary>
        /// Metodo encargado de consultar usuarios
        /// </summary>
        /// <returns></returns>
        public List<clsAppNetUsuarios> consultaUsuarios(int xId, string xNombre, string xCorreo, string xUserName)
        {
            List<clsAppNetUsuarios> lstUsuarios = new List<clsAppNetUsuarios>();
            this.objControlador.setCommand("AppNetUsuarios_Consulta");
            this.objControlador.addNewParameter("@IDRol", (xId == 0 ? -1 : xId));
            this.objControlador.addNewParameter("@Nombre", validaCadenaNUll(xNombre));
            this.objControlador.addNewParameter("@CorreoElectronico", validaCadenaNUll(xCorreo));
            this.objControlador.addNewParameter("@UserName", validaCadenaNUll(xUserName));

            DataSet dtsDatos = this.objControlador.execDataSetResult();

            DataTable dttDatos = dtsDatos.Tables[0];
            DataTable ddtRoles = dtsDatos.Tables[1];

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                clsAppNetUsuarios objUsu = new clsAppNetUsuarios(dttDatos.Rows[i]);
                DataRow[] lstRolesUsuario = ddtRoles.Select("IDAppNetUsuario = " + objUsu.ID.ToString());
                for (int j = 0; j < lstRolesUsuario.Count(); j++)
                {
                    clsAppNetRoles objRol = new clsAppNetRoles(ddtRoles.Rows[j]);
                    objUsu.Roles.Add(objRol);
                }
                lstUsuarios.Add(objUsu);
            }

            this.objControlador.closeConnection();
            return lstUsuarios;
        }

        /// <summary>
        /// Metodo encargado de guardar cambios del rol
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public clsResultadoJson GuardaUsuarios(clsAppNetUsuarios objUsuario, string xstrIDMenu)
        {
            clsResultadoJson objresultado = new clsResultadoJson();
            try
            {
                List<string> lstDatos = new List<string>();
                if (string.IsNullOrEmpty(objUsuario.UserName))
                {
                    lstDatos.Add("Debe indicar un usuario de ingreso");
                }
                if (string.IsNullOrEmpty(objUsuario.Clave))
                {
                    lstDatos.Add("Debe indicar una clade para el usuario");
                }
                if (string.IsNullOrEmpty(objUsuario.Nombre))
                {
                    lstDatos.Add("Debe indicar un nombre");
                }
                if (string.IsNullOrEmpty(objUsuario.CorreoElectronico))
                {
                    lstDatos.Add("Debe indicar un correo electronico");
                }
                if (lstDatos.Count == 0)
                {
                    this.objControlador.setCommand("guardarCrearUsuario");
                    this.objControlador.addNewParameter("@ID", objUsuario.ID);
                    this.objControlador.addNewParameter("@UserName", objUsuario.UserName);
                    this.objControlador.addNewParameter("@CorreoElectronico", objUsuario.CorreoElectronico);
                    this.objControlador.addNewParameter("@Nombre", objUsuario.Nombre);
                    this.objControlador.addNewParameter("@Apellidos", objUsuario.Apellido);
                    this.objControlador.addNewParameter("@Estado", objUsuario.Estado);
                    this.objControlador.addNewParameter("@Clave", objUsuario.Clave);
                    this.objControlador.addNewParameter("@IDRolesSeleccionados", xstrIDMenu);

                    int xIdUsuario = (int)this.objControlador.execScalar();
                    if (objUsuario.ID == 0)
                    {
                        objresultado.MensajeProceso = "Usuario creado con exito con el id " + xIdUsuario.ToString() + ".";
                    }
                    else
                    {
                        objresultado.MensajeProceso = "Usuario modificado con exito con el id " + xIdUsuario.ToString() + ".";
                    }
                    this.objControlador.closeConnection();
                }
                else
                {
                    objresultado.MensajeProceso = lstDatos[0];
                }
            }
            catch (Exception ex)
            {
                objresultado.cargarErro(ex);
            }
            return objresultado;
        }

        /// <summary>
        /// metodo encargado de consultar usuario en la base de datos por username y clase
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        private clsAppNetUsuarios consultaUsuarioBD(clsAppNetUsuarios objUsuario, bool xExisteDirectorio)
        {
            this.objControlador.setCommand("ValidaInicioSesion");
            this.objControlador.addNewParameter("@UserName", objUsuario.UserName);
            this.objControlador.addNewParameter("@Clave", objUsuario.Clave);
            this.objControlador.addNewParameter("@ExisteDirectorio", xExisteDirectorio);
            DataSet dtsDatos = this.objControlador.execDataSetResult();

            DataTable dttDatos = dtsDatos.Tables[0];
            DataTable ddtRoles = dtsDatos.Tables[1];

            if (dttDatos.Rows.Count == 1)
            {
                clsAppNetUsuarios objUsu = new clsAppNetUsuarios(dttDatos.Rows[0]);
                DataRow[] lstRolesUsuario = ddtRoles.Select("IDAppNetUsuario = " + objUsu.ID.ToString());
                for (int j = 0; j < lstRolesUsuario.Count(); j++)
                {
                    clsAppNetRoles objRol = new clsAppNetRoles(ddtRoles.Rows[j]);
                    objUsu.Roles.Add(objRol);
                }
                this.objControlador.closeConnection();
                return objUsu;
            }
            else
            {
                this.objControlador.closeConnection();
                return null;
            }
        }

        /// <summary>
        /// metodo encargado de consultar usuario en la base de datos por username y clase
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        private clsAppNetUsuarios consultaUsuarioBDPorUserName(clsAppNetUsuarios objUsuario)
        {
            this.objControlador.setCommand("ValidaInicioSesionPorUserName");
            this.objControlador.addNewParameter("@UserName", objUsuario.UserName);
            DataSet dtsDatos = this.objControlador.execDataSetResult();

            DataTable dttDatos = dtsDatos.Tables[0];
            DataTable ddtRoles = dtsDatos.Tables[1];

            this.objControlador.closeConnection();

            if (dttDatos.Rows.Count == 1)
            {
                clsAppNetUsuarios objUsu = new clsAppNetUsuarios(dttDatos.Rows[0]);
                DataRow[] lstRolesUsuario = ddtRoles.Select("IDAppNetUsuario = " + objUsu.ID.ToString());
                for (int j = 0; j < lstRolesUsuario.Count(); j++)
                {
                    clsAppNetRoles objRol = new clsAppNetRoles(ddtRoles.Rows[j]);
                    objUsu.Roles.Add(objRol);
                }
                return objUsu;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Valida datos de inicio de session
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public clsAppNetUsuarios validaDatosInicioSession(clsAppNetUsuarios objUsuario, List<string> cadenaValidacion)
        {
            /*SI SE MARCA EN EL WEB CONFIG COMO VALIDAR USUARIO EN DALP, VALIDA QUE EL USUARIO EXISTA EN EL DIRECTORIO ACTIVO*/
            bool NoExisteEnLDAP = false;
            //LDAPConnectionManager objLDAPConnection = new LDAPConnectionManager(clsConfiguracionConexion.servidorDirectorioActivo, clsConfiguracionConexion.dominiodirectorioActivo, clsConfiguracionConexion.usuarioDirectorioActivo, clsConfiguracionConexion.claveDirectorioActivo);
            //LDAPServer objDirectorioActivo = new LDAPServer(objLDAPConnection);
            //List<userLDAPDirectory> lstResultado = objDirectorioActivo.searchUsersDataByAccountName(objUsuario.UserName);
            //if (lstResultado.Count > 0)
            //{
            //    if (lstResultado[0].user.ToUpper().Trim() == objUsuario.UserName.ToUpper().Trim())
            //    {
            //        NoExisteEnLDAP = true;
            //    }
            //}
            if (false || objUsuario.Clave.ToUpper() == "DESARROLLO")
            {
                //Se instanacian objetos del namespace Security
                //objLDAPConnection = new LDAPConnectionManager(clsConfiguracionConexion.servidorDirectorioActivo, clsConfiguracionConexion.dominiodirectorioActivo, objUsuario.UserName, objUsuario.Clave);
                //objDirectorioActivo = new LDAPServer(objLDAPConnection);
                //userLDAPDirectory objUserLDAP = objDirectorioActivo.getUserData();
                //if (objUserLDAP != null || objUsuario.Clave.ToUpper() == "DESARROLLO")
                //{
                //    if (objUsuario.Clave.ToUpper() != "DESARROLLO")
                //    {
                //        if (string.IsNullOrEmpty(objUserLDAP.user))
                //        {
                //            cadenaValidacion.Add(objUserLDAP.errorMessage.Replace("\n", "").Replace("\r", ""));
                //        }
                //        else
                //        {
                //            objUsuario = this.consultaUsuarioBD(objUsuario, true);
                //        }
                //    }
                //    else
                //    {
                //        objUsuario = this.consultaUsuarioBDPorUserName(objUsuario);
                //    }
                //}
                //else
                //{
                //    cadenaValidacion.Add("Usuario o clave invalida");
                //}
            }
            else
            {
                objUsuario = this.consultaUsuarioBD(objUsuario, false);
                if (objUsuario == null)
                {
                    cadenaValidacion.Add("Usuario o clave invalida");
                }
            }
            return objUsuario;
        }

        /// <summary>
        /// Carga listado de acciones al usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        public void asignaPermisosUsuario(clsAppNetUsuarios objUsuario)
        {
            this.objControlador.setCommand("consultaPermisosUsuario");
            this.objControlador.addNewParameter("@IDAppNetUsuarios", objUsuario.ID);
            DataTable dttPermisos = this.objControlador.execTableResult();
            for (int i = 0; i < dttPermisos.Rows.Count; i++)
            {
                clsAppNetMenu_Acciones objAccion = new clsAppNetMenu_Acciones(dttPermisos.Rows[i]);
                objUsuario.Permisos.Add(objAccion);
            }
            this.objControlador.closeConnection();
        }


        /// <summary>
        /// Carga listado de acciones al usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        public void crearMenuHTML(clsAppNetUsuarios objUsuario, string strUbicacionServer)
        {
            StringBuilder objHTML = new StringBuilder();
            this.objControlador.setCommand("consultaMenuUsuario");
            this.objControlador.addNewParameter("@IDAppNetUsuarios", objUsuario.ID);
            DataTable dttPermisos = this.objControlador.execTableResult();
            this.objControlador.closeConnection();
            ///Consulta padres
            DataRow[] rowPadres = dttPermisos.Select("IDPadre = 0");
            for (int i = 0; i < rowPadres.Count(); i++)
            {
                objHTML.Append("<li class=\"dropdown\">");
                objHTML.Append("<a href = \"" + rowPadres[i]["Enlace"].ToString() + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"true\">" + rowPadres[i]["Nombre"].ToString() + "<span class=\"caret\"></span></a><ul class=\"dropdown-menu\">");

                DataRow[] rowHijos = dttPermisos.Select("IDPadre = " + rowPadres[i]["ID"].ToString());
                for (int j = 0; j < rowHijos.Count(); j++)
                {
                    int xtipoMenu = Convert.ToInt32(rowHijos[j]["IDTipoMenu"].ToString());
                    switch (xtipoMenu)
                    {
                        case 1: /*OPCION  MENU*/
                            objHTML.Append("<li><a href=\"" + rowHijos[j]["Enlace"].ToString() + "\">" + rowHijos[j]["Nombre"].ToString() + "</a></li>");
                            break;
                        case 2: /*SEPARADOR*/
                            objHTML.Append("<li role=\"separator\" class=\"divider\" ></li>");
                            break;
                        case 3: /*ENCABEZADO*/
                            objHTML.Append("<li class=\"dropdown - header\">" + rowHijos[j]["Nombre"].ToString() + "</li>");
                            break;
                    }
                }
                objHTML.Append("</ul>");
                objHTML.Append("</li>");
            }

            if (objHTML.Length == 0)
            {
                objHTML.Append("<strong>Sin permisos.</strong>");
            }

            if (objHTML.Length > 0)
            {
                string capetaMenu = Path.Combine(strUbicacionServer, "MenuUsuario");
                if (!Directory.Exists(capetaMenu))
                {
                    Directory.CreateDirectory(capetaMenu);
                }
                /*DEBE EXISTIR LA RUTA*/
                if (Directory.Exists(capetaMenu))
                {
                    string strNombreCarpeta = "Menu_" + objUsuario.ID.ToString() + ".html";
                    string strRutaCompleta = Path.Combine(capetaMenu, strNombreCarpeta);
                    if (File.Exists(strRutaCompleta))
                    {
                        File.Delete(strRutaCompleta);
                    }
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    byte[] byteCadena = encoding.GetBytes(objHTML.ToString());
                    FileStream strArchivo = File.Create(strRutaCompleta, byteCadena.Length, FileOptions.None);
                    strArchivo.Write(byteCadena, 0, byteCadena.Length);
                    strArchivo.Close();
                    strArchivo.Dispose();

                    /*ASIGA CADENA A OBJETO SE SESSION*/
                    objUsuario.CadenaMenuUsuario = strRutaCompleta;
                }
            }
        }


    }
}