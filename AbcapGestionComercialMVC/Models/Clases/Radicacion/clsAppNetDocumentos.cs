using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetDocumentos : clsBasicoConfiguracion
    {

        public int IDAppNetAdministrador { get; set; }
        public clsAppNetFlujoEstados AppNetFlujoEstados { get; set; }
        public clsAppNetTipoDocumentos AppNetTipoDocumentos { get; set; }
        public clsAppNetEmisor IDAppNetEmisor { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string Nota { get; set; }
        public DateTime FechaCreacion { get; set; }
        public clsAppNetUsuarios AppNetUsuariosCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public clsAppNetUsuarios AppNetUsuariosModificacion { get; set; }
        public int IDSiguienteAccion { get; set; }
        public List<clsAppNetDocumento_Soporte> lstSoportes;
        public string xIDArchivosEliminados { get; set; }

        public clsAppNetDocumentos()
        {
            this.AppNetFlujoEstados = new clsAppNetFlujoEstados();
            this.AppNetTipoDocumentos = new clsAppNetTipoDocumentos();
            this.FechaDocumento = DateTime.Now.Date;
            this.FechaRecepcion = DateTime.Now.Date;
            this.lstSoportes = new List<clsAppNetDocumento_Soporte>();
        }

    }
}