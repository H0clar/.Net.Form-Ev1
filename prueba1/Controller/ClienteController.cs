using prueba1.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prueba1
{
    public class ClienteController
    {
        private List<Cliente> listaClientes = new List<Cliente>();

        public void AgregarCliente(Cliente cliente)
        {
            if (ClienteExiste(cliente.Rut))
            {
                throw new Exception("Cliente con este RUT ya existe.");
            }

            listaClientes.Add(cliente);
        }

        public List<Cliente> ObtenerClientes()
        {
            return listaClientes;
        }

        public void EliminarCliente(string rut)
        {
            Cliente cliente = BuscarClientePorRut(rut);

            if (cliente != null)
            {
                listaClientes.Remove(cliente);
            }
        }

        public void EditarCliente(string rut, Cliente clienteModificado)
        {
            Cliente clienteExistente = BuscarClientePorRut(rut);

            if (clienteExistente != null)
            {
                clienteExistente.Nombre = clienteModificado.Nombre;
                clienteExistente.EsEmpresa = clienteModificado.EsEmpresa;
                clienteExistente.Telefono = clienteModificado.Telefono;
                clienteExistente.Direccion = clienteModificado.Direccion;
                clienteExistente.FechaRegistro = clienteModificado.FechaRegistro;
                clienteExistente.CantidadFacturas = clienteModificado.CantidadFacturas;
                clienteExistente.NumeroUltimaFactura = clienteModificado.NumeroUltimaFactura;
                clienteExistente.MontoUltimaFactura = clienteModificado.MontoUltimaFactura;
            }
        }

        public Cliente BuscarClientePorRut(string rut)
        {
            return listaClientes.Find(c => c.Rut == rut);
        }

        private bool ClienteExiste(string rut)
        {
            return listaClientes.Exists(c => c.Rut == rut);
        }
    }
}
