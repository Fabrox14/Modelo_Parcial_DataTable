
using ParcialApp.Acceso_a_datos;
using ParcialApp.Dominio;
using ParcialApp.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialApp.Presentacion
{
    public partial class Frm_Alta : Form
    {
        private Factura oFactura;
        private GestorFactura gestor;

        public Frm_Alta()
        {
            InitializeComponent();

            oFactura = new Factura();
            gestor = new GestorFactura(new DAOFactory());
        }

        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            CargarCombo();
            consultarUltimoPresupuesto();
            // Valores por defecto
            dtpFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCliente.Text = "CONSUMIDOR FINAL";
            cboProducto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboForma.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void consultarUltimoPresupuesto()
        {
            lblNro.Text = "Presupuesto Nro: " + gestor.ProximoPresupuesto();
        }

        private void CargarCombo()
        {
            DataTable tabla = gestor.ObtenerProductos();

            // tabla.Rows[0]; // cada fila que tenga va a ser un DataRow

            cboProducto.DataSource = tabla;
            cboProducto.DisplayMember = tabla.Columns[1].ColumnName; // n_producto
            cboProducto.ValueMember = tabla.Columns[0].ColumnName; // id_producto 
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ExisteProductoEnGrilla(cboProducto.Text))
            {
                MessageBox.Show("Producto ya agregado como detalle", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult result = MessageBox.Show("Desea Agregar?", "Confirmación", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                DetalleFactura item = new DetalleFactura();
                item.Cantidad = (int)nudCantidad.Value;


                DataRowView oDataRow = (DataRowView)cboProducto.SelectedItem;



                // Producto:
                Producto oProducto = new Producto();
                oProducto.IdProducto = Int32.Parse(oDataRow[0].ToString());
                oProducto.Nombre = oDataRow[1].ToString();
                oProducto.Precio = Double.Parse(oDataRow[2].ToString());
                item.Producto = oProducto;

                oFactura.AgregarDetalle(item);

                // te voy a dar un array de objetos
                dgvDetalles.Rows.Add(new object[] { "", oProducto.Nombre, oProducto.Precio, item.Cantidad, item.CalcularSubtotal() });

                calcularTotales();
            }
        }

        private void calcularTotales()
        {
            double subTotal = oFactura.CalcularTotal();
            double total = subTotal;
            lblSubtotal.Text = "Subtotal: $" + subTotal.ToString();
            txtTotal.Text = total.ToString();
        }

        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["producto"].Value.Equals(text))
                    return true;
            }
            return false;
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5)
            {
                oFactura.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                calcularTotales();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos detalle!",
                "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCliente.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar un tipo de cliente", "Validaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCliente.Focus();
                return;
            }

            GuardarPresupuesto();
            Limpiar();
            consultarUltimoPresupuesto();
        }

        private void Limpiar()
        {
            dtpFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCliente.Text = "CONSUMIDOR FINAL";
            cboForma.SelectedIndex = -1;
            cboProducto.SelectedIndex = -1;
            nudCantidad.Value = 1;
            dgvDetalles.Rows.Clear();
            lblSubtotal.Text = "Subtotal: $";
            txtTotal.Text = "";
        }

        private void GuardarPresupuesto()
        {
            oFactura.Cliente = txtCliente.Text;
            oFactura.FormaPago = Convert.ToInt32(cboForma.SelectedIndex);
            oFactura.Fecha = Convert.ToDateTime(dtpFecha.Text);
            oFactura.Total = Convert.ToDouble(txtTotal.Text);
            oFactura.FacturaNro = Convert.ToInt32(txtTotal.Text);

            if (gestor.ConfirmarPresupuesto(oFactura))
            {
                MessageBox.Show("Presupuesto registrado", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // this.Dispose();
            }
            else
            {
                MessageBox.Show("ERROR. No se pudo registrar el presupuesto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
            else
            {
                return;
            }
        }
    }
}
