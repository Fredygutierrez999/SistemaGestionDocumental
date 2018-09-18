using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AbcapGestionComercialMVC.Models.Clases.General
{
    [Serializable()]
    public class clsSemana : clsBasicoConfiguracion
    {

        #region Parametros
        public int ANO { get; set; }
        public string SEMANA { get; set; }
        [JsonIgnore]
        public DateTime FECHA_INICIAL { get; set; }
        [JsonIgnore]
        public DateTime FECHA_FINAL { get; set; }
        [JsonIgnore]
        public string A_QUE_MES { get; set; }
        [JsonIgnore]
        public int NRO_SEM_DE_MES { get; set; }
        [JsonIgnore]
        public int DIAS_HABILES { get; set; }
        [JsonIgnore]
        public int DIAS_FESTIVOS { get; set; }
        [JsonIgnore]
        public int HORAS_HABILES { get; set; }
        [JsonIgnore]
        public string CER_INV { get; set; }
        [JsonIgnore]
        public decimal NUM_HOR { get; set; }
        [JsonIgnore]
        public int A_BOGOTA { get; set; }
        public decimal CantidadCajas { get; set; }
        #endregion

        #region Constructor
        public clsSemana() { }
        #endregion

    }
}