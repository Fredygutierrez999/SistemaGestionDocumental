﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsProducto : clsBasicoConfiguracion
    {
        public string Revision { get; set; }
    }
}