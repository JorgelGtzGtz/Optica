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
    public interface IProveedoresService
    {
        Proveedore GetProveedor(int id);
        List<Proveedore> GetProveedores();
        List<Proveedore> GetProveedoresFiltro(string nombre = null);
        bool InsertUpdateProveedor(Proveedore cliente, out string Message);
        bool EliminarProveedor(int id, out string Message);
    }

    public class ProveedoresService : IProveedoresService
    {
        private readonly IProveedoresRepository _proveedoresRepository;

        public ProveedoresService(IProveedoresRepository proveedoresRepository) {
            _proveedoresRepository = proveedoresRepository;
        }


        public Proveedore GetProveedor(int id) {
            return _proveedoresRepository.Get(id);
        }

        public List<Proveedore> GetProveedores() {
            return _proveedoresRepository.GetAll("Proveedores").ToList();
        }

        public List<Proveedore> GetProveedoresFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Nombre like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Proveedores " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _proveedoresRepository.GetByFilter(query);
        }

        public bool InsertUpdateProveedor(Proveedore model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                if (model.FechaCreacion == null)
                    model.FechaCreacion = DateTime.Now;

                if (model.ID == 0)
                {
                   var id = _proveedoresRepository.InsertOrUpdate<int>(model);
                }
                else
                {
                    _proveedoresRepository.InsertOrUpdate<int>(model);
                }

                Message = "Proveedor guardado " + model.NombreComercial + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Proveedor No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarProveedor(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _proveedoresRepository.Get(id);

                _proveedoresRepository.Remove(model);

                Message = "Proveedor eliminado " + model.NombreComercial + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Proveedor no pudo ser eliminado Error: " + ex.Message;
            }
            return result;
        }

    }
}
