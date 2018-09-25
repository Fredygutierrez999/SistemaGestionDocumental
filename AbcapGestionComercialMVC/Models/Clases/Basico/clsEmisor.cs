using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumental.Models.Clases.Basico
{
    public class clsEmisor
    {
        public int IDAppNetAdministrador { get; set; }
        public long ID { get; set; }
        public int IDAppNetTipoNumero { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
        public int IDAppNetTipoEmisor { get; set; }
        public int IDAppNetEstados { get; set; }
        public string NombreAppNetEstados { get; set; }
    }
}