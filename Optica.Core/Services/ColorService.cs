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
    public interface IColorService
    {
        Colore GetColor(int id);
        List<Colore> GetColors();
        List<Colore> GetColorsFiltro(string nombre = null);
        bool InsertUpdateColor(Colore model, out string Message);
        bool EliminarColor(int id, out string Message);
    }

    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository) {
            _colorRepository = colorRepository;
        }

        public Colore GetColor(int id)
        {
            return _colorRepository.Get(id);
        }

        public List<Colore> GetColors() {
            return _colorRepository.GetAll("Colores").ToList();
        }

        public List<Colore> GetColorsFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Colores " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _colorRepository.GetByFilter(query);
        }

        public bool InsertUpdateColor(Colore model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _colorRepository.InsertOrUpdate<int>(model);

                Message = "Color guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Color no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarColor(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _colorRepository.Get(id);

                _colorRepository.Remove(model);

                Message = "Color eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Color no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
