using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsPresentaciones : clsBasicoConfiguracion
    {
        public clsGrupoPresentacion grupoPresentacion { get; set; }
    }
}