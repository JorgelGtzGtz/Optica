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
    public interface IMaterialeService
    {
        Materiale GetMateriale(int id);
        List<Materiale> GetMateriales();
        List<Materiale> GetMaterialesFiltro(string nombre = null);
        bool InsertUpdateMateriale(Materiale Materiale, out string Message);
        bool EliminarMateriale(int id, out string Message);
    }

    public class MaterialeService : IMaterialeService
    {
        private readonly IMaterialeRepository _MaterialeRepository;

        public MaterialeService(IMaterialeRepository MaterialeRepository) {
            _MaterialeRepository = MaterialeRepository;
        }

        public Materiale GetMateriale(int id)
        {
            return _MaterialeRepository.Get(id);
        }

        public List<Materiale> GetMateriales() {
            return _MaterialeRepository.GetAll("Materiale").ToList();
        }

        public List<Materiale> GetMaterialesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Materiales " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _MaterialeRepository.GetByFilter(query);
        }

        public bool InsertUpdateMateriale(Materiale Materiale, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _MaterialeRepository.InsertOrUpdate<int>(Materiale);

                Message = "Materiale de Lente guardada " + Materiale.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Materiale no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarMateriale(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _MaterialeRepository.Get(id);

                _MaterialeRepository.Remove(model);

                Message = "Materiale de Lente eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Material de Lente no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
