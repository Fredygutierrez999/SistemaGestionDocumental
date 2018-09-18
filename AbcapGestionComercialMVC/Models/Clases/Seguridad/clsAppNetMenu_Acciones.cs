using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AbcapGestionComercialMVC.Models.Clases.Seguridad
{
    public class clsAppNetMenu_Acciones : clsBasicoConfiguracion
    {
        public int IDAppNetMenu { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }

        public clsAppNetMenu_Acciones() {}
        public clsAppNetMenu_Acciones(DataRow dtrDatos) {
            this.ID = Convert.ToInt32(dtrDatos["ID"].ToString());
            this.IDAppNetMenu = Convert.ToInt32(dtrDatos["IDAppNetMenu"].ToString());
            this.Controlador = dtrDatos["Controlador"].ToString();
            this.Accion = dtrDatos["Accion"].ToString();
        }

    }
}