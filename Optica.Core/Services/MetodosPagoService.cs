using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IMetodosPagoService
    {
        MetodosPago GetMetodoPago(int id);
        List<MetodosPago> GetMetodosPago();
        List<MetodosPago> GetMetodosPagoFiltro(string nombre = null);
        bool InsertUpdateMetodosPago(MetodosPago zona, out string Message);
        bool CancelarMetodoPago(int id);
    }

    public class MetodosPagoService : IMetodosPagoService
    {
        private readonly IMetodosPagoRepository _metodosPagoRepository;

        public MetodosPagoService(IMetodosPagoRepository metodosPagoRepository) {
            _metodosPagoRepository = metodosPagoRepository;
        }

        public MetodosPago GetMetodoPago(int id)
        {
            return _metodosPagoRepository.Get(id);
        }

        public List<MetodosPago> GetMetodosPago() {
            return _metodosPagoRepository.GetAll("MetodosPago").ToList();
        }

        public List<MetodosPago> GetMetodosPagoFiltro(string descripcion = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(descripcion))
            {
                filter += string.Format("Descripcion like '%{0}%' and estatus = 'true'", descripcion);
            }

            Sql query = new Sql(@"select * from MetodosPago " + (!string.IsNullOrEmpty(descripcion) ? filter : "") + "where estatus = 'true'");
            return _metodosPagoRepository.GetByFilter(query);
        }

        public bool CancelarMetodoPago(int id)
        {
            bool result = false;

            try
            {
                var metodoPago = _metodosPagoRepository.Get(id);
                metodoPago.Estatus = false;
                _metodosPagoRepository.InsertOrUpdate<int>(metodoPago);
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public bool InsertUpdateMetodosPago(MetodosPago zona, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _metodosPagoRepository.InsertOrUpdate<int>(zona);

                Message = "Metodo Pago guardada " + zona.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Metodo Pago no pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }
    }
}
