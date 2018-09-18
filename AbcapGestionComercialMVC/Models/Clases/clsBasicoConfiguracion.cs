using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.Clases
{
    [Serializable()]
    public class clsBasicoConfiguracion : IBasico
    {
        #region Variables privadas
        private string _GuidUnico;
        private long _ID;
        private string _Nombre;
        private string _Codigo;
        private int _Estado;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public clsBasicoConfiguracion()
        {
            this._GuidUnico = Guid.NewGuid().ToString();
        }

        #region Parametros
        /// <summary>
        /// ID unico para el objeto Generado
        /// </summary>
        [JsonIgnore]
        public string getIDUnicoObjeto
        {
            get { return this._GuidUnico; }
            set { this._GuidUnico = value; }
        }

        /// <summary>
        /// ID unico de BD para el registro
        /// </summary>
        public long ID {
            get { return this._ID; }
            set { this._ID = value; }
        }

        /// <summary>
        /// Nombre BD del registro
        /// </summary>
        public string Nombre {
            get { return this._Nombre; }
            set { this._Nombre = value; }
        }

        /// <summary>
        /// Codigo BD del registro
        /// </summary>
        public string Codigo
        {
            get { return this._Codigo; }
            set { this._Codigo = value; }
        }


        public int Estado {
            get { return this._Estado; }
            set { this._Estado = value; }
        }
        #endregion
    }
}