using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.Seguridad;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetDocumento_Soporte : clsBasicoConfiguracion
    {
        public string NuevoNombre { get; set; }
        public string Extension { get; set; }
        public string SoloNombre { get; set; }
        public int Tamanio { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public clsAppNetUsuarios objUsuarioCreacion { get; set; }
        public clsAppNetUsuarios objUsuarioModificacion { get; set; }
        public string guidDocumento { get; set; }
        public decimal IDUniversal { get; set; }
        public DateTime fechaSubida { get; set; }
        public string Ubicacion { get; set; }
        public string Ubicacion_NombreArchivo { get; set; }
        public bool Temporal { get; set; }
        public long IDAppNetDocumentos { get; set; }
        public int IDAppNetAdministrador { get; set; }
        public string ContentType { get; set; }
        public int ContendorImagen { get; set; }
        public bool Eliminar { get; set; }

        public clsAppNetDocumento_Soporte()
        {

        }

    }
}