using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetFlujoEstados : clsBasicoConfiguracion
    {
        public int IDAppNetAdministrador { get; set; }
        public bool ConCorreo { get; set; }
        public bool ConCorreoGrupo { get; set; }
        public int IDAppNetEstado { get; set; }
        public string TextoBtn { get; set; }

        public List<clsAppNetFlujoEstados_Acciones> lstAcciones { get; set; }

        public clsAppNetFlujoEstados() {
            this.lstAcciones = new List<clsAppNetFlujoEstados_Acciones>();
        }
    }
}