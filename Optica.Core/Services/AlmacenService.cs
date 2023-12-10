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
    public interface IAlmacenService
    {
        Almacene GetAlmacen(int id);
        List<Almacene> GetAlmacenes();
        List<dynamic> GetAlmacenesFiltro(string nombre = null);
        bool InsertUpdateAlmacen(Almacene Almacene, out string Message);
        bool EliminarAlmacen(int id, out string Message);
    }

    public class AlmacenService : IAlmacenService
    {
        private readonly IAlmacenRepository _almaceneRepository;

        public AlmacenService(IAlmacenRepository almaceneRepository) {
            _almaceneRepository = almaceneRepository;
        }

        public Almacene GetAlmacen(int id) {
            return _almaceneRepository.Get(id);
        }

        public List<Almacene> GetAlmacenes() {
            return _almaceneRepository.GetAll("Almacenes").ToList();
        }

        

        public List<dynamic> GetAlmacenesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("p.Nombre like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select p.[ID], p.[Nombre] as Nombre, p.[Prefijo], s.Nombre as Sucursal from [dbo].[Almacenes] p
                                  inner join [dbo].[Sucursales] s on s.ID = p.ID_Sucursal" + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _almaceneRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateAlmacen(Almacene Almacene, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _almaceneRepository.InsertOrUpdate<int>(Almacene);

                Message = "Almacen guardado " + Almacene.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Almacen No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarAlmacen(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var Almacene = _almaceneRepository.Get(id);

                _almaceneRepository.Remove(Almacene);

                Message = "Almacen eliminado " + Almacene.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Almacen No pudo ser eliminado Error: " + ex.Message;
            }
            return result;
        }
    }
}
