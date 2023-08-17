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
    public interface ITiposEntradaSalidaService
    {
        TiposEntradaSalida GetTiposEntradaSalida(int id);
        List<TiposEntradaSalida> GetTiposEntradaSalidas();
        List<TiposEntradaSalida> GetTiposEntradaSalidasFiltro(string nombre = null);
        bool InsertUpdateTiposEntradaSalida(TiposEntradaSalida model, out string Message);
        bool EliminarTiposEntradaSalida(int id, out string Message);
    }

    public class TiposEntradaSalidaService : ITiposEntradaSalidaService
    {
        private readonly ITiposEntradaSalidaRepository _tiposEntradaSalidaRepository;

        public TiposEntradaSalidaService(ITiposEntradaSalidaRepository tiposEntradaSalidaRepository) {
            _tiposEntradaSalidaRepository = tiposEntradaSalidaRepository;
        }

        public TiposEntradaSalida GetTiposEntradaSalida(int id)
        {
            return _tiposEntradaSalidaRepository.Get(id);
        }

        public List<TiposEntradaSalida> GetTiposEntradaSalidas() {
            return _tiposEntradaSalidaRepository.GetAll("TiposEntradaSalida").ToList();
        }

        public List<TiposEntradaSalida> GetTiposEntradaSalidasFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from TiposEntradaSalida " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _tiposEntradaSalidaRepository.GetByFilter(query);
        }

        public bool InsertUpdateTiposEntradaSalida(TiposEntradaSalida model, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _tiposEntradaSalidaRepository.InsertOrUpdate<int>(model);

                Message = "Tipo Entrada/Salida guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Tipo Entrada/Salida no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarTiposEntradaSalida(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var model = _tiposEntradaSalidaRepository.Get(id);

                _tiposEntradaSalidaRepository.Remove(model);

                Message = "Tipo Entrada/Salida eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Tipo Entrada/Salida no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
