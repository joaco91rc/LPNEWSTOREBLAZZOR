using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class ClienteService
    {
        
        private readonly DL_Cliente _clienteDL;
        private readonly DL_ClienteNegocio _clienteNegocioDL;

        public ClienteService(DL_Cliente clienteDL, DL_ClienteNegocio clienteNegocioDL)
        {
            _clienteDL = clienteDL;
            _clienteNegocioDL = clienteNegocioDL;
        }

        public async Task<List<Cliente>> Listar()
        {
            return await _clienteDL.ListarClientes();
        }

        public async Task<(int idGenerado, string mensaje)> Registrar(Cliente cliente)
        {
            string mensaje = string.Empty;
            int id = await _clienteDL.Registrar(cliente, m => mensaje = m);
            return (id, mensaje);
        }


        public async Task<(bool exito, string mensaje)> Editar(Cliente cliente)
        {
            string mensaje = string.Empty;
            bool resultado = await _clienteDL.Editar(cliente, m => mensaje = m);
            return (resultado, mensaje);
        }

        public async Task<(bool exito, string mensaje)> Eliminar(int idCliente)
        {
            string mensaje = string.Empty;

            Cliente cliente = new Cliente { IdCliente = idCliente }; // Creamos el objeto requerido

            bool resultado = await _clienteDL.Eliminar(cliente, m => mensaje = m);

            return (resultado, mensaje);
        }

        public async Task<(bool exito, string mensaje)> AsignarClienteANegocio(int idCliente, int idNegocio)
        {
            return await _clienteNegocioDL.AsignarClienteANegocio(idCliente, idNegocio);
        }

        public async Task<List<Cliente>> ListarClientesPorNegocio(int idNegocio)
        {
            return await _clienteDL.ListarClientesPorNegocio(idNegocio);
        }


        }
}
