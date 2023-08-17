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
    public interface ISucursalService
    {
        Sucursale GetSucursal(int id);
        List<Sucursale> GetSucursales();
        List<Sucursale> GetSucursalesFiltro(string nombre = null);
        bool InsertUpdateSucursal(Sucursale sucursal, out string Message);
        bool EliminarSucursal(int id, out string Message);
    }

    public class SucursalService : ISucursalService
    {
        private readonly ISucursalRepository _sucursalRepository;

        public SucursalService(ISucursalRepository sucursalRepository) {
            _sucursalRepository = sucursalRepository;
        }

        public Sucursale GetSucursal(int id)
        {
            return _sucursalRepository.Get(id);
        }

        public List<Sucursale> GetSucursales() {
            return _sucursalRepository.GetAll("Sucursales").ToList();
        }

        public List<Sucursale> GetSucursalesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Nombre like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Sucursales " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _sucursalRepository.GetByFilter(query);
        }

        public bool InsertUpdateSucursal(Sucursale sucursal, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                var exist = _sucursalRepository.Get(sucursal.ID);

                if (exist != null)
                {
                    _sucursalRepository.InsertOrUpdate<int>(sucursal);
                }
                else
                {
                    int id = _sucursalRepository.InsertOrUpdate<int>(sucursal);
                }
                Message = "Sucursal guardada " + sucursal.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Sucursal No pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarSucursal(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var sucursal = _sucursalRepository.Get(id);

                _sucursalRepository.Remove(sucursal);

                Message = "Sucursal eliminada " + sucursal.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Sucursal No pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
