using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AbcapGestionComercialMVC.Models.Clases.Seguridad
{
    [Serializable()]
    public class clsAppNetMenu : clsBasicoConfiguracion
    {
        public long IDPadre { get; set; }
        public string Enlace { get; set; }
        public string Imagen { get; set; }

        public clsAppNetMenu() {
        }

        /// <summary>
        /// Sobrecarga de constructor
        /// </summary>
        /// <param name=""></param>
        public clsAppNetMenu(DataRow dtRow)
        {
            this.ID = Convert.ToInt64(dtRow["Id"].ToString());
            this.IDPadre = Convert.ToInt64(dtRow["IdPadre"].ToString());
            this.Nombre = dtRow["Nombre"].ToString();
            this.Enlace = dtRow["Enlace"].ToString();
            this.Imagen = dtRow["Imagen"].ToString();
            this.Estado = Convert.ToInt32(dtRow["Estado"].ToString());
        }

    }
}