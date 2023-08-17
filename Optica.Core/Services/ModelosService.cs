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
    public interface IModelosService
    {
        Modelo GetModelo(int id);
        List<Modelo> GetModelos();
        List<Modelo> GetModelosFiltro(string nombre = null);
        bool InsertUpdateModelo(Modelo model, out string Message);
        bool EliminarModelo(int id, out string Message);
    }

    public class ModelosService : IModelosService
    {
        private readonly IModelosRepository _modelosRepository;

        public ModelosService(IModelosRepository modelosRepository) {
            _modelosRepository = modelosRepository;
        }

        public Modelo GetModelo(int id)
        {
            return _modelosRepository.Get(id);
        }

        public List<Modelo> GetModelos() {
            return _modelosRepository.GetAll("Modelos").ToList();
        }

        public List<Modelo> GetModelosFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Modelos " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _modelosRepository.GetByFilter(query);
        }

        public bool InsertUpdateModelo(Modelo model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _modelosRepository.InsertOrUpdate<int>(model);

                Message = "Modelo guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Modelo no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarModelo(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _modelosRepository.Get(id);

                _modelosRepository.Remove(model);

                Message = "Modelo eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Modelo no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
