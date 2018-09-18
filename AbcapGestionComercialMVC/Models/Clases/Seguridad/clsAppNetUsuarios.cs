using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AbcapGestionComercialMVC.Models.Clases.Seguridad
{
    [Serializable]
    public class clsAppNetUsuarios : clsBasicoConfiguracion
    {
        public string UserName { get; set; }
        public string CorreoElectronico { get; set; }
        public string Apellido { get; set; }
        public string Imagen { get; set; }
        public string Telefono { get; set; }
        public string Clave { get; set; }
        public bool LoginPorDirectorioActivo { get; set; }
        public List<clsAppNetRoles> Roles { get; set; }
        public List<clsAppNetMenu> MenuResumen { get; set; }
        public List<clsAppNetMenu_Acciones> Permisos { get; set; }
        public string CadenaMenuUsuario { get; set; }
        public long IDTemporal { get; set; }
        public int IDAdministrador { get; set; }

        public clsAppNetUsuarios()
        {
            this.Roles = new List<clsAppNetRoles>();
            this.MenuResumen = new List<clsAppNetMenu>();
            this.Permisos = new List<clsAppNetMenu_Acciones>();
            this.CadenaMenuUsuario = string.Empty;
            this.IDAdministrador = 1;
        }

        public clsAppNetUsuarios(DataRow dtrDatos)
        {
            this.ID = Convert.ToInt64(dtrDatos["ID"].ToString());
            this.UserName = dtrDatos["UserName"].ToString();
            this.CorreoElectronico = dtrDatos["CorreoElectronico"].ToString();
            this.Nombre = dtrDatos["Nombre"].ToString();
            this.Apellido = dtrDatos["Apellidos"].ToString();
            this.Imagen = dtrDatos["Imagen"].ToString();
            this.Telefono = dtrDatos["Telefono"].ToString();
            this.Estado = Convert.ToInt32(dtrDatos["Estado"].ToString());
            this.Clave = dtrDatos["Clave"].ToString();
            this.LoginPorDirectorioActivo = Convert.ToBoolean(dtrDatos["LoginPorDirectorioActivo"].ToString());
            this.Roles = new List<clsAppNetRoles>();
            this.MenuResumen = new List<clsAppNetMenu>();
            this.Permisos = new List<clsAppNetMenu_Acciones>();
            this.CadenaMenuUsuario = string.Empty;
            this.IDAdministrador = 1;
        }

    }
}