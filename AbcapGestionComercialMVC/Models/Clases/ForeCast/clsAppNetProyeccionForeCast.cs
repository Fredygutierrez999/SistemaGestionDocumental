using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases.General;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;

namespace AbcapGestionComercialMVC.Models.Clases.ForeCast
{
    [Serializable]
    public class clsAppNetProyeccionForeCast : clsBasicoConfiguracion
    {
        public string Descripcion { get; set; }
        public clsTipoDatoPYG  Bas_TipoDatoPYG { get; set; }
        public clsSemana AnioSemanaInicial { get; set; }
        public clsSemana AnioSemanaFinal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public clsAppNetUsuarios UsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public clsAppNetUsuarios Usuariomodificacion { get; set; }
        public long IDForeCastAnterior { get; set; }
        public bool CargarAnteriorForeCast { get; set; }
        public clsEstadoProyeccion EstadoObj { get; set; }
        public clsAppNetTipoProceso TipoProceso { get; set; }
        public int Consecutivo { get; set; }
        public bool DatosConsolidados { get; set; }


        public clsAppNetProyeccionForeCast() {
            this.TipoProceso = new clsAppNetTipoProceso();
        }

    }
}