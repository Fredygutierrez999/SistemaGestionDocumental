using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AbcapGestionComercialMVC.Models.Clases.Seguridad
{
    [Serializable()]
    public class clsAppNetTipoRol : clsBasicoConfiguracion
    {
        public Boolean MostrarComboCanal { get; set; }

        public clsAppNetTipoRol() { }
        public clsAppNetTipoRol(DataRow dtrFila) {
            this.ID = Convert.ToInt32(dtrFila["ID"].ToString());
            this.Nombre = dtrFila["Nombre"].ToString();
            this.MostrarComboCanal = Convert.ToBoolean(dtrFila["MostrarComboCanal"].ToString());
        }

        public clsAppNetTipoRol(int xID, string xNombre, bool xMostrarCampoCanal) {
            this.ID = xID;
            this.Nombre = xNombre;
            this.MostrarComboCanal = xMostrarCampoCanal;
        }
    }
}