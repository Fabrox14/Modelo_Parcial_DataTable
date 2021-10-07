using ParcialApp.Acceso_a_datos;
using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Servicios
{
    class GestorFactura
    {
        private IFacturaDAO dao;

        public GestorFactura(AbstractDAOFactory factory)
        {
            dao = factory.CrearFacturaDAO();
        }





        public int ProximoPresupuesto()
        {
            return dao.ObtenerProximoNroPresupuesto();
        }

        public DataTable ObtenerProductos()
        {
            return dao.ListarProductos();
        }

        public bool ConfirmarPresupuesto(Factura oFactura)
        {
            return dao.Crear(oFactura);
        }
    }
}
