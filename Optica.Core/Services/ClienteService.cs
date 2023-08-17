using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IClienteService
    {
        Cliente GetCliente(int id);
        List<Cliente> GetClientes();
        List<dynamic> GetClientesFiltro(string nombre = null);
        bool InsertUpdateCliente(Cliente cliente, out string Message);
        bool EliminarCliente(int id, out string Message);
    }

    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository) {
            _clienteRepository = clienteRepository;
        }


        public Cliente GetCliente(int id) {
            return _clienteRepository.Get(id);
        }

        public List<Cliente> GetClientes() {
            return _clienteRepository.GetAll("Clientes").ToList();
        }

        public List<dynamic> GetClientesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("c.Nombre like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select c.*,s.Nombre as 'Sucursal', z.Descripcion as 'Zona' from Clientes c
                                inner join Sucursales s on s.ID = c.ID_Sucursal
                                inner join Zonas z on z.ID = c.ID_Zona " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _clienteRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateCliente(Cliente cliente, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                if (cliente.FechaCreacion == null)
                    cliente.FechaCreacion = DateTime.Now;

                if (cliente.ID == 0)
                {
                   var id = _clienteRepository.InsertOrUpdate<int>(cliente);
                }
                else
                {
                    _clienteRepository.InsertOrUpdate<int>(cliente);
                }

                Message = "Cliente guardado " + cliente.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Cliente No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarCliente(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var cliente = _clienteRepository.Get(id);

                //Eliminar cliente
                _clienteRepository.Remove(cliente);

                Message = "Cliente eliminado " + cliente.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Cliente No pudo ser eliminado Error: " + ex.Message;
            }
            return result;
        }

    }
}
