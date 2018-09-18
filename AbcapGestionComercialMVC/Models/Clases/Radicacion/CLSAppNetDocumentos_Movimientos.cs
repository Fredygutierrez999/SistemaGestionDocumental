using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetDocumentos_Movimientos : clsBasicoConfiguracion
    {
        public clsAppNetFlujoEstados AppNetFlujoEstados { get; set; }
        public string Descripcion_Movimiento { get; set; }
        public clsAppNetUsuarios AppNetUsuariosCreacion { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public clsAppNetUsuarios AppNetUsuariosSolicitud { get; set; }

        public clsAppNetDocumentos_Movimientos() {
        }
    }
}