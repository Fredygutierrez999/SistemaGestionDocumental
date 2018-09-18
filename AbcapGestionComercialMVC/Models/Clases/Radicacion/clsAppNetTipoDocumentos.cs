using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetTipoDocumentos : clsBasicoConfiguracion
    {
        public bool conConsecutivoAutomatico { get; set; }
    }
}