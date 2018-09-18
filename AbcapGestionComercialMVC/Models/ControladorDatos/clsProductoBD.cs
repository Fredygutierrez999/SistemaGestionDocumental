using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AbcapGestionComercialMVC.Models.Clases.Productos;
using AbcapGestionComercialMVC.Models.Clases;

namespace AbcapGestionComercialMVC.Models.ControladorDatos
{
    public class clsProductoBD : clsConfgiruacionBD
    {
        public clsProductoBD() { }

        /// <summary>
        /// COnsulta listado de productos desiguales entre proyeccion y produccion
        /// </summary>
        /// <param name="xCodigo"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsProductosSincronizacion> consultaListadoProductosParaSincronizar(string xCodigo, string xNombre) {
            List<clsProductosSincronizacion> lstProductos = new List<clsProductosSincronizacion>();
            try
            {
                this.objControlador.setCommand("BuscaProductosParaCargar");
                this.objControlador.addNewParameter("@Codigo", xCodigo);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++) {
                    clsProductosSincronizacion objProd = new clsProductosSincronizacion();
                    objProd.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                    objProd.Codigo = dttDatos.Rows[i]["Codigo"].ToString();
                    objProd.Revision = dttDatos.Rows[i]["Revision"].ToString();
                    objProd.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    objProd.IDClasificacion = Convert.ToInt32(dttDatos.Rows[i]["IDClasificacion"].ToString());
                    objProd.NombreClasificacion = dttDatos.Rows[i]["NombreClasificacion"].ToString();
                    objProd.IDPresentacion = Convert.ToInt32(dttDatos.Rows[i]["IDPresentacion"].ToString());
                    objProd.NombrePresentacion = dttDatos.Rows[i]["NombrePresentacion"].ToString();
                    objProd.IDTipoProducto = Convert.ToInt32(dttDatos.Rows[i]["IDTiposProductos"].ToString());
                    objProd.NombreTipoProducto = dttDatos.Rows[i]["NombreTipoPresentacion"].ToString();
                    lstProductos.Add(objProd);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex) {
                cargarErro(ex);
            }
            return lstProductos;
        }

        /// <summary>
        /// COnsulta listado de productos en la proyeccion
        /// </summary>
        /// <param name="xCodigo"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsProductosSincronizacion> consultaListadoProductos(string xCodigo, string xNombre)
        {
            List<clsProductosSincronizacion> lstProductos = new List<clsProductosSincronizacion>();
            try
            {
                this.objControlador.setCommand("BuscaProductos");
                this.objControlador.addNewParameter("@Codigo", xCodigo);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsProductosSincronizacion objProd = new clsProductosSincronizacion();
                    objProd.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                    objProd.Codigo = dttDatos.Rows[i]["Codigo"].ToString();
                    objProd.Revision = dttDatos.Rows[i]["Revision"].ToString();
                    objProd.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    objProd.IDClasificacion = Convert.ToInt32(dttDatos.Rows[i]["IDClasificacion"].ToString());
                    objProd.NombreClasificacion = dttDatos.Rows[i]["NombreClasificacion"].ToString();
                    objProd.IDPresentacion = Convert.ToInt32(dttDatos.Rows[i]["IDPresentacion"].ToString());
                    objProd.NombrePresentacion = dttDatos.Rows[i]["NombrePresentacion"].ToString();
                    objProd.IDTipoProducto = Convert.ToInt32(dttDatos.Rows[i]["IDTiposProductos"].ToString());
                    objProd.NombreTipoProducto = dttDatos.Rows[i]["NombreTipoPresentacion"].ToString();
                    lstProductos.Add(objProd);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstProductos;
        }


        /// <summary>
        /// COnsulta listado de productos en la proyeccion
        /// </summary>
        /// <param name="xCodigo"></param>
        /// <param name="xNombre"></param>
        /// <returns></returns>
        public List<clsProductosSincronizacion> consultaListadoProductosSinCliente(string xCodigo, string xNombre)
        {
            List<clsProductosSincronizacion> lstProductos = new List<clsProductosSincronizacion>();
            try
            {
                this.objControlador.setCommand("BuscaProductosSinAsignarCliente");
                this.objControlador.addNewParameter("@Codigo", xCodigo);
                this.objControlador.addNewParameter("@Nombre", xNombre);
                DataTable dttDatos = this.objControlador.execTableResult();
                for (int i = 0; i < dttDatos.Rows.Count; i++)
                {
                    clsProductosSincronizacion objProd = new clsProductosSincronizacion();
                    objProd.ID = Convert.ToInt64(dttDatos.Rows[i]["ID"].ToString());
                    objProd.Codigo = dttDatos.Rows[i]["Codigo"].ToString();
                    objProd.Revision = dttDatos.Rows[i]["Revision"].ToString();
                    objProd.Nombre = dttDatos.Rows[i]["Nombre"].ToString();
                    objProd.IDClasificacion = Convert.ToInt32(dttDatos.Rows[i]["IDClasificacion"].ToString());
                    objProd.NombreClasificacion = dttDatos.Rows[i]["NombreClasificacion"].ToString();
                    objProd.IDPresentacion = Convert.ToInt32(dttDatos.Rows[i]["IDPresentacion"].ToString());
                    objProd.NombrePresentacion = dttDatos.Rows[i]["NombrePresentacion"].ToString();
                    objProd.IDTipoProducto = Convert.ToInt32(dttDatos.Rows[i]["IDTiposProductos"].ToString());
                    objProd.NombreTipoProducto = dttDatos.Rows[i]["NombreTipoPresentacion"].ToString();
                    lstProductos.Add(objProd);
                }
                this.objControlador.closeConnection();
            }
            catch (Exception ex)
            {
                cargarErro(ex);
            }
            return lstProductos;
        }

        /// <summary>
        /// Metodo utilizado para importar productos desde produccion
        /// </summary>
        /// <param name="xIDProducto"></param>
        /// <returns></returns>
        public clsResultadoJson ImportarProductoProyeccion(int xIDProducto) {
            clsResultadoJson objResultado = new clsResultadoJson();

            try
            {
                this.objControlador.setCommand("SincronizaProductoDesdeProduccion");
                this.objControlador.addNewParameter("@IDProductoCaja", xIDProducto);
                this.objControlador.execNoResults();
            }
            catch (Exception ex) {
                objResultado.cargarErro(ex);
            }
            this.objControlador.closeConnection();
            return objResultado;
        }


        /// <summary>
        /// Metodo utilizado para importar productos desde produccion
        /// </summary>
        /// <param name="xIDProducto"></param>
        /// <returns></returns>
        public clsResultadoJson ActualizarProductoProyeccion(int xIDProducto)
        {
            clsResultadoJson objResultado = new clsResultadoJson();

            try
            {
                this.objControlador.setCommand("ActualizarProductoDesdeProduccion");
                this.objControlador.addNewParameter("@IDProductoCaja", xIDProducto);
                this.objControlador.execNoResults();
            }
            catch (Exception ex)
            {
                objResultado.cargarErro(ex);
            }
            this.objControlador.closeConnection();
            return objResultado;
        }

    }
}