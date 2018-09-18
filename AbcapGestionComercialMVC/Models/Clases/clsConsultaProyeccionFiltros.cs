using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcapGestionComercialMVC.Models.Clases;

namespace AbcapGestionComercialMVC.Models.Clases
{
    public class clsConsultaProyeccionFiltros
    {
        public SelectList Proyecciones { get; set; }
        public SelectList Canales { get; set; }

        public clsConsultaProyeccionFiltros() {
            
        }

    }
}