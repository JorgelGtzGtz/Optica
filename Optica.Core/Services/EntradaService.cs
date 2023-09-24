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
    public interface IEntradaService
    {
        int InsertUpdateEntrada(OtrasEntradasSalida Entrada, out string Message);
        List<dynamic> GetOtraEntradaSalidaFiltro();
        bool EliminarEntrada(int id, out string Message);
        public OtrasEntradasSalida GetEntrada(int id);
        public List<dynamic> GetCompraDetalles(int id);


    }

    public class EntradaService : IEntradaService
    {
        
        private readonly IOtrasEntradasSalidasRepository _otrasEntradasSalidasRepository;
        private readonly IOtrasEntradasSalidasDetallesRepository _otrasEntradasSalidasDetallesRepository;

        public EntradaService(IOtrasEntradasSalidasRepository otrasEntradasSalidasRepository, IOtrasEntradasSalidasDetallesRepository otrasEntradasSalidasDetallesRepository = null)
        {
            _otrasEntradasSalidasRepository = otrasEntradasSalidasRepository;
            _otrasEntradasSalidasDetallesRepository = otrasEntradasSalidasDetallesRepository;
        }

        public List<dynamic> GetOtraEntradaSalidaFiltro()
        {

            Sql query = new Sql(@"select oes.*, tes.Descripcion as Movimiento from otrasentradassalidas oes inner join tiposentradasalida tes on oes.ID_TipoEntradaSalida = tes.ID;");
            return _otrasEntradasSalidasRepository.GetByDynamicFilter(query);
        }

        public int InsertUpdateEntrada(OtrasEntradasSalida Entrada, out string Message) 
        {

            Message = string.Empty;
            int result = 0;
            try
            {
                result = _otrasEntradasSalidasRepository.InsertOrUpdate<int>(Entrada);
                Message = "Entrada guardada con exito";
            }
            catch (Exception ex)
            {

                Message = "Entrada No pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public OtrasEntradasSalida GetEntrada(int id)
        {
            return _otrasEntradasSalidasRepository.Get(id);
        }

        public List<dynamic> GetCompraDetalles(int id)
        {

            Sql query = new Sql(@"SELECT productos.Descripcion, otrasentradassalidasdetalles.*
                FROM otrasentradassalidasdetalles
                INNER JOIN productos ON otrasentradassalidasdetalles.ID_Producto = productos.ID
                WHERE otrasentradassalidasdetalles.ID_OtraEntradasSalidas = @id;", new { id });
            return _otrasEntradasSalidasRepository.GetByDynamicFilter(query);
        }

       

        public bool EliminarEntrada(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var entrada = _otrasEntradasSalidasRepository.Get(id);
                Sql query = new Sql(@"select * from otrasentradassalidasdetalles where ID_OtraEntradasSalidas = "+id);
                List<OtrasEntradasSalidasDetalle> a = _otrasEntradasSalidasDetallesRepository.GetByFilter(query);

                foreach (var detalle in a)
                {
                    _otrasEntradasSalidasDetallesRepository.Remove(detalle);
                }
                _otrasEntradasSalidasRepository.Remove(entrada);

                Message = "Entrada eliminada con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Entrada No pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }

    }
}
