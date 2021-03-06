﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcapGestionComercialMVC.Models.Clases;

namespace GestionDocumental.Models.Clases.Radicacion
{
    public class clsAppNetEmisor : clsBasicoConfiguracion
    {
        public int IDAppNetAdministrador { get; set; }
        public int IDAppNetTipoNumero { get; set; }
        public string Numero { get; set; }
        public int IDAppNetTipoEmisor { get; set; }
        public int IDAppNetEstados { get; set; }
        public string NombreAppNetEstados { get; set; }
    }
}