using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AbcapGestionComercialMVC.Models.ControladorDatos;
using AbcapGestionComercialMVC.Models.Clases;
using AbcapGestionComercialMVC.Models.Clases.General;
using GestionDocumental.Models.Clases.Basico;
using GestionDocumental.Models.Clases.Radicacion;

namespace GestionDocumental.Models.ControladorDatos
{
    public class clsBasico : clsConfgiruacionBD
    {
        /// <summary>
        /// Metodo utilizado para consultar listado de Emisores
        /// </summary>
        /// <param name="xID"></param>
        /// <param name="xNumero"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsAppNetEmisor> consultaListadoEmisor(int xID, string xNumero, string xNombre)
        {
            List<clsAppNetEmisor> lstEmisores = new List<clsAppNetEmisor>();
            try
            {
                this.objControlador.setCommand("Emisor_Consultar");
                this.objControlador.addNewParameter("@ID", xID);
                this.objControlador.addNewParameter("@Numero", xNumero);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetEmisor objItem = new clsAppNetEmisor();
                    objItem.ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString());
                    objItem.Numero = dttDatos.Rows[i]["Numero"].ToString();
                    objItem.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    objItem.IDAppNetEstados = Convert.ToInt32(dttDatos.Rows[i]["IDEstado"].ToString());
                    objItem.NombreAppNetEstados = dttDatos.Rows[i]["NombreEstado"].ToString();
                    lstEmisores.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEmisores;
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de Emisores
        /// </summary>
        /// <param name="xID"></param>
        /// <param name="xNumero"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsAppNetTipoNumero> consultaTipoNumero()
        {
            List<clsAppNetTipoNumero> lstEmisores = new List<clsAppNetTipoNumero>();
            try
            {
                this.objControlador.setCommand("Consulta_TipoNumero");
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetTipoNumero objItem = new clsAppNetTipoNumero();
                    objItem.ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString());
                    objItem.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    lstEmisores.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEmisores;
        }


        /// <summary>
        /// Metodo utilizado para consultar listado de Emisores
        /// </summary>
        /// <param name="xID"></param>
        /// <param name="xNumero"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsAppNetEstadoProceso> consultaEstadoPorProceso(int xIdProceso)
        {
            List<clsAppNetEstadoProceso> lstEmisores = new List<clsAppNetEstadoProceso>();
            try
            {
                this.objControlador.setCommand("Consulta_EstadoProceso");
                this.objControlador.addNewParameter("@IDProceso", xIdProceso);
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsAppNetEstadoProceso objItem = new clsAppNetEstadoProceso();
                    objItem.ID = Convert.ToInt32(dttDatos.Rows[i]["ID"].ToString());
                    objItem.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    lstEmisores.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstEmisores;
        }


        /// <summary>
        /// Metodo utilizado para validar datos el emisor
        /// </summary>
        /// <returns></returns>
        private List<string> validaDatosEmisor(clsAppNetEmisor objEmisor)
        {
            List<string> lstMensaje = new List<string>();
            if (String.IsNullOrEmpty(objEmisor.Numero))
            {
                lstMensaje.Add("Debe ingresar un número de emisor.");
            }
            if (String.IsNullOrEmpty(objEmisor.Nombre))
            {
                lstMensaje.Add("Debe ingresar un nombre de emisor.");
            }
            return lstMensaje;
        }

        /// <summary>
        /// Guardar / Modificar emisor
        /// </summary>
        /// <param name="objEmisor"></param>
        /// <returns></returns>
        public clsResultadoJson GuardarModificarEmisor(clsAppNetEmisor objEmisor)
        {
            clsResultadoJson objResultado = new clsResultadoJson();
            try
            {
                List<string> lstMensaje = validaDatosEmisor(objEmisor);
                if (lstMensaje.Count == 0)
                {
                    this.objControlador.setCommand("GuardarEmisor");
                    this.objControlador.addNewParameter("@IDAppNetAdministrador", objEmisor.IDAppNetAdministrador);
                    this.objControlador.addNewParameter("@ID", objEmisor.ID);
                    this.objControlador.addNewParameter("@IDAppNetTipoNumero", objEmisor.IDAppNetTipoNumero);
                    this.objControlador.addNewParameter("@Numero", objEmisor.Numero);
                    this.objControlador.addNewParameter("@Nombre", objEmisor.Nombre);
                    this.objControlador.addNewParameter("@IDAppNetTipoEmisor", objEmisor.IDAppNetTipoEmisor);
                    this.objControlador.addNewParameter("@IDAppNetEstados", objEmisor.IDAppNetEstados);
                    long xIDEmisor = (long)this.objControlador.execScalar();
                    if (objEmisor.ID == 0 || objEmisor.ID == -1)
                    {
                        objResultado.MensajeProceso = "Se creo el emisor con Id: " + xIDEmisor.ToString();
                    }
                    else
                    {
                        objResultado.MensajeProceso = "Se modifico el emisor con Id: " + xIDEmisor.ToString();
                    }
                }
                else
                {
                    objResultado.MensajeProceso = lstMensaje[0];
                    objResultado.ResultadoProceso = false;
                }
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            return objResultado;
        }

       
    }
}