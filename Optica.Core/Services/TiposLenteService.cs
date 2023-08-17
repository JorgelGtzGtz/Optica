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
    public interface ITiposLenteService
    {
        TiposLente GetTiposLente(int id);
        List<TiposLente> GetTiposLentes();
        List<TiposLente> GetTiposLentesFiltro(string nombre = null);
        bool InsertUpdateTiposLente(TiposLente model, out string Message);
        bool EliminarTiposLente(int id, out string Message);
    }

    public class TiposLenteService : ITiposLenteService
    {
        private readonly ITiposLenteRepository _tiposLenteRepository;

        public TiposLenteService(ITiposLenteRepository tiposLenteRepository) {
            _tiposLenteRepository = tiposLenteRepository;
        }

        public TiposLente GetTiposLente(int id)
        {
            return _tiposLenteRepository.Get(id);
        }

        public List<TiposLente> GetTiposLentes() {
            return _tiposLenteRepository.GetAll("TiposLente").ToList();
        }

        public List<TiposLente> GetTiposLentesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from TiposLente " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _tiposLenteRepository.GetByFilter(query);
        }

        public bool InsertUpdateTiposLente(TiposLente model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _tiposLenteRepository.InsertOrUpdate<int>(model);

                Message = "TiposLente guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "TiposLente no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarTiposLente(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _tiposLenteRepository.Get(id);

                _tiposLenteRepository.Remove(model);

                Message = "TiposLente eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "TiposLente no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
