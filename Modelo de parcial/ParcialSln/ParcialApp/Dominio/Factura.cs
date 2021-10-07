using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcialApp.Dominio
{
    class Factura
    {
        public int FacturaNro { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public double Total { get; set; }
        public int FormaPago { get; set; }
        public DateTime FechaBaja { get; set; }
        public List<DetalleFactura> Detalles { get; }

        public Factura()
        {
            // Generar la relacion 1 a muchos
            Detalles = new List<DetalleFactura>();
        }

        // Funcion para agregar los detalles del mismo presupuesto a una lista
        // pq un presupuesto tiene muchos detalles
        public void AgregarDetalle(DetalleFactura detalle)
        {
            Detalles.Add(detalle);
        }

        public void QuitarDetalle(int nro)
        {
            Detalles.RemoveAt(nro);
        }

        public double CalcularTotal()
        {
            double total = 0;

            foreach (DetalleFactura item in Detalles)
            {
                total += item.CalcularSubtotal();
            }

            return total;
        }

        //public bool Confirmar()
        //{
        //  paso al DAO
        //}

        //public bool Actualizar()
        //{
        //    paso al DAO            
        //}

        public double calcularTotalDesc(double total, double descuento)
        {
            return total - ((descuento * total) / 100);
        }

        public string GetFechaBajaFormato()
        {
            string aux = FechaBaja.ToString("dd/MM/yyyy");
            return aux.Equals("01/01/0001") ? "" : aux;
        }
    }
}
