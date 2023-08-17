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
    public interface IColorLenteService
    {
        ColoresLente GetColorLente(int id);
        List<ColoresLente> GetColorLentes();
        List<ColoresLente> GetColorLentesFiltro(string nombre = null);
        bool InsertUpdateColorLente(ColoresLente model, out string Message);
        bool EliminarColorLente(int id, out string Message);
    }

    public class ColorLenteService : IColorLenteService
    {
        private readonly IColorLenteRepository _colorLenteRepository;

        public ColorLenteService(IColorLenteRepository colorLenteRepository) {
            _colorLenteRepository = colorLenteRepository;
        }

        public ColoresLente GetColorLente(int id)
        {
            return _colorLenteRepository.Get(id);
        }

        public List<ColoresLente> GetColorLentes() {
            return _colorLenteRepository.GetAll("ColoresLente").ToList();
        }

        public List<ColoresLente> GetColorLentesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from ColoresLente " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _colorLenteRepository.GetByFilter(query);
        }

        public bool InsertUpdateColorLente(ColoresLente model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _colorLenteRepository.InsertOrUpdate<int>(model);

                Message = "Color Lente guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Color Lente no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarColorLente(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _colorLenteRepository.Get(id);

                _colorLenteRepository.Remove(model);

                Message = "Color Lente eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Color Lente no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
