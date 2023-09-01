using prueba1.Api;
using prueba1.Modelo;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace prueba1
{
    public partial class Form1 : Form
    {
        private ClienteController clienteController = new ClienteController();
        private Cliente clienteEditando = null;

        public Form1()
        {
            InitializeComponent();
            InicializarTabla();
            MostrarValorDolar();
            MostrarFecha();
            
        }

        private void InicializarTabla()
        {
            // codigo para configurar la tabla

        }

        private void MostrarValorDolar()
        {
            try
            {
                string valorDolar = ConsumoAPI.ObtenerValorMoneda("dolar");
                txtValor.Text = valorDolar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el valor del dólar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarFecha()
        {
            try
            {
                string fecha = ConsumoAPI.ObtenerFecha();

                if (DateTime.TryParse(fecha, out DateTime parsedDate))
                {
                    txtFecha.Text = parsedDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    MessageBox.Show("Fecha obtenida no válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la fecha: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (clienteEditando != null)
                {
                    Cliente clienteModificado = CrearClienteDesdeFormulario();
                    clienteController.EditarCliente(clienteEditando.Rut, clienteModificado);

                    RefrescarTabla();
                    LimpiarFormulario();
                    clienteEditando = null;
                }
                else
                {
                    Cliente nuevoCliente = CrearClienteDesdeFormulario();
                    clienteController.AgregarCliente(nuevoCliente);
                    RefrescarTabla();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string rutClienteAEliminar = ObtenerRutSeleccionado();

                if (!string.IsNullOrEmpty(rutClienteAEliminar))
                {
                    clienteController.EliminarCliente(rutClienteAEliminar);
                    RefrescarTabla();
                }
                else
                {
                    MessageBox.Show("Selecciona un cliente de la tabla para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tblInfo.SelectedRows.Count > 0)
                {
                    string rutClienteAEditar = ObtenerRutSeleccionado();

                    if (!string.IsNullOrEmpty(rutClienteAEditar))
                    {
                        clienteEditando = clienteController.BuscarClientePorRut(rutClienteAEditar);
                        CargarClienteEnFormulario(clienteEditando);
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona una fila para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            clienteEditando = null;
        }

        private string ObtenerRutSeleccionado()
        {
            if (tblInfo.SelectedRows.Count > 0)
            {
                DataGridViewRow filaSeleccionada = tblInfo.SelectedRows[0];
                return filaSeleccionada.Cells["Rut"].Value.ToString();
            }

            return null;
        }

        private Cliente CrearClienteDesdeFormulario()
        {
            if (!int.TryParse(txtFacturas.Text.Trim(), out int cantidadFacturas))
            {
                MessageBox.Show("Cantidad de facturas no válida. Debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Cantidad de facturas no válida.");
            }

            if (!int.TryParse(txtNUltimaFactura.Text.Trim(), out int numeroUltimaFactura))
            {
                MessageBox.Show("Número de última factura no válido. Debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Número de última factura no válido.");
            }

            if (!int.TryParse(txtMontoUFactura.Text.Trim(), out int montoUltimaFactura))
            {
                MessageBox.Show("Monto de última factura no válido. Debe ser un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Monto de última factura no válido.");
            }

            return new Cliente
            {
                Rut = txtRut.Text,
                Nombre = txtNombre.Text,
                EsEmpresa = chkActivo.Checked,
                Telefono = txtTelefono.Text,
                Direccion = txtDireccion.Text,
                FechaRegistro = dateTimePicker1.Value.Date,
                CantidadFacturas = cantidadFacturas,
                NumeroUltimaFactura = numeroUltimaFactura,
                MontoUltimaFactura = montoUltimaFactura
            };
        }

        private void CargarClienteEnFormulario(Cliente cliente)
        {
            txtRut.Text = cliente.Rut;
            txtNombre.Text = cliente.Nombre;
            chkActivo.Checked = cliente.EsEmpresa;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;
            dateTimePicker1.Value = cliente.FechaRegistro;
            txtFacturas.Text = cliente.CantidadFacturas.ToString();
            txtNUltimaFactura.Text = cliente.NumeroUltimaFactura.ToString();
            txtMontoUFactura.Text = cliente.MontoUltimaFactura.ToString();
        }

        private void RefrescarTabla()
        {
            tblInfo.DataSource = null;
            tblInfo.DataSource = clienteController.ObtenerClientes();
        }

        private void LimpiarFormulario()
        {
            txtRut.Text = string.Empty;
            txtNombre.Text = string.Empty;
            chkActivo.Checked = false;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            txtFacturas.Text = string.Empty;
            txtNUltimaFactura.Text = string.Empty;
            txtMontoUFactura.Text = string.Empty;
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void txtFecha_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }



   
}
