using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Acceso_a_datos
{
    class FacturaDAO : IFacturaDAO
    {
        public int ObtenerProximoNroPresupuesto()
        {
            return HelperDAO.ObtenerInstancia().ProximoID("SP_PROXIMO_ID", "@next");
        }

        public DataTable ListarProductos()
        {
            return HelperDAO.ObtenerInstancia().ConsultaSQL("SP_CONSULTAR_PRODUCTOS");
        }

        public bool Crear(Factura oFactura)
        {
            return HelperDAO.ObtenerInstancia().Save(oFactura);
        }
    }
}
