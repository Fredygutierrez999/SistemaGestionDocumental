using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AbcapGestionComercialMVC.Models.Clases.Seguridad
{
    [Serializable]
    public class clsAppNetRoles : clsBasicoConfiguracion
    {
        public DateTime fechaAgregacion { get; set; }
        public List<clsAppNetMenu> Menu { get; set; }
        public clsAppNetTipoRol TipoRol { get; set; }
        public string Canal { get; set; }

        public clsAppNetRoles() {
            this.Menu = new List<clsAppNetMenu>();
        }

        public clsAppNetRoles(DataRow dtrDatos)
        {
            this.ID = Convert.ToInt64(dtrDatos["ID"].ToString());
            this.Nombre = dtrDatos["Nombre"].ToString();
            this.fechaAgregacion = Convert.ToDateTime(dtrDatos["FechaCreacion"].ToString());
            this.Menu = new List<clsAppNetMenu>();
            this.TipoRol = new clsAppNetTipoRol(Convert.ToInt32(dtrDatos["tipoRolID"].ToString()), dtrDatos["tipoRolNombre"].ToString(), Convert.ToBoolean(dtrDatos["tipoRolComboCanal"].ToString()));
            this.Canal = dtrDatos["Canal"].ToString();
        }

    }
}