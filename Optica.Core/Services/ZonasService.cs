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
    public interface IZonasService
    {
        Zona GetZona(int id);
        List<Zona> GetZonas();
        List<Zona> GetZonasFiltro(string nombre = null);
        bool InsertUpdateZona(Zona zona, out string Message);
        bool EliminarZona(int id, out string Message);
    }

    public class ZonasService : IZonasService
    {
        private readonly IZonasRepository _zonasRepository;

        public ZonasService(IZonasRepository zonasRepository) {
            _zonasRepository = zonasRepository;
        }

        public Zona GetZona(int id)
        {
            return _zonasRepository.Get(id);
        }

        public List<Zona> GetZonas() {
            return _zonasRepository.GetAll("Zonas").ToList();
        }

        public List<Zona> GetZonasFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Zonas " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _zonasRepository.GetByFilter(query);
        }

        public bool InsertUpdateZona(Zona zona, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _zonasRepository.InsertOrUpdate<int>(zona);

                Message = "Zona guardada " + zona.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Zona no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarZona(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var zona = _zonasRepository.Get(id);

                //Eliminar Sucursal
                _zonasRepository.Remove(zona);

                Message = "Zona eliminada " + zona.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Zona no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }
    }
}
