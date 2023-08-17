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
    public interface IMarcasService
    {
        Marca GetMarca(int id);
        List<Marca> GetMarcas();
        List<Marca> GetMarcasFiltro(string nombre = null);
        bool InsertUpdateMarca(Marca marca, out string Message);
        bool EliminarMarca(int id, out string Message);
    }

    public class MarcasService : IMarcasService
    {
        private readonly IMarcasRepository _marcasRepository;

        public MarcasService(IMarcasRepository marcasRepository) {
            _marcasRepository = marcasRepository;
        }

        public Marca GetMarca(int id)
        {
            return _marcasRepository.Get(id);
        }

        public List<Marca> GetMarcas() {
            return _marcasRepository.GetAll("Marcas").ToList();
        }

        public List<Marca> GetMarcasFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Marcas " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _marcasRepository.GetByFilter(query);
        }

        public bool InsertUpdateMarca(Marca marca, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _marcasRepository.InsertOrUpdate<int>(marca);

                Message = "Marca guardada " + marca.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Marca no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarMarca(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _marcasRepository.Get(id);

                //Eliminar Sucursal
                _marcasRepository.Remove(model);

                Message = "Marca eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Marca no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
