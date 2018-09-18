using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsCliente : clsBasicoConfiguracion
    {
        #region Parametros
        public int IDCanales { get; set; }
        public int IDSubCanales { get; set; }
        public int IDDivisionesClientes { get; set; }
        public int IDCiudades { get; set; }
        #endregion

        public clsCliente() { }
    }
}