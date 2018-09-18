using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AbcapGestionComercialMVC.Models.Clases.General;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsGeneralBD : clsConfgiruacionBD
    {
        public clsGeneralBD() { }

        /// <summary>
        /// Consulta listado de años    
        /// </summary>
        /// <returns></returns>
        public List<clsSemana> consultaAnios()
        {
            List<clsSemana> lstSemanasAnio = new List<clsSemana>();
            this.objControlador.setCommand("consultaAnio");
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                lstSemanasAnio.Add(new clsSemana() { ID = Convert.ToInt64(dttDatos.Rows[i]["Ano"].ToString()), Nombre = dttDatos.Rows[i]["Nombre"].ToString() });
            }

            this.objControlador.closeConnection();
            return lstSemanasAnio;
        }

        /// <summary>
        /// Consulta listado de semanas pertenecientes a un año
        /// </summary>
        /// <param name="xAnio"></param>
        /// <returns></returns>
        public List<clsSemana> consultaSemanas(int xAnio)
        {
            List<clsSemana> lstSemanasAnio = new List<clsSemana>();
            this.objControlador.setCommand("consultaSemanasAnio");
            this.objControlador.addNewParameter("@Anio", xAnio);
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++)
            {
                lstSemanasAnio.Add(new clsSemana() { ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString()), Nombre = dttDatos.Rows[i]["Nombre"].ToString() });
            }

            this.objControlador.closeConnection();
            return lstSemanasAnio;
        }

        /// <summary>
        /// Metodo encargado de consultar listado de estados
        /// </summary>
        /// <param name="xIdProceso"></param>
        /// <returns></returns>
        public List<clsEstadoProyeccion> ConsultaListaEstados(int xIdProceso)
        {
            List<clsEstadoProyeccion> lstEstados = new List<clsEstadoProyeccion>();
            this.objControlador.setCommand("consultaEstados");
            this.objControlador.addNewParameter("@IDProceso", xIdProceso);
            DataTable dttDatos = this.objControlador.execTableResult();

            for (int i = 0; i < dttDatos.Rows.Count; i++) {
                lstEstados.Add(new clsEstadoProyeccion() { ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString()), Nombre = dttDatos.Rows[i]["Nombre"].ToString() });
            }
            this.objControlador.closeConnection();
            return lstEstados;
        }
    }
}