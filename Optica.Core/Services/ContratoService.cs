using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IContratoService
    {
        public int GetLastContrato();
        public Producto GetProductoSelected(int id);
        int InsertUpdateContrato(Contrato Contrato, List<corridaOriginal> corrida, out string Message);
        public Paciente GetPacienteSelected(int id);
        public Contrato GetContrato(int id);
    }

    public class ContratoService : IContratoService
    {
        private readonly IContratosRepository _ContratosRepository;
        private readonly IProductosRepository _ProductosRepository;
        private readonly IPacientesRepository _PacientesRepository;
        private readonly ISucursalRepository _SucursalesRepository;
        private readonly ICorridaRepository _CorridaRepository;
        private readonly IPagaresRepository _PagaresRepository;

        public ContratoService(ICorridaRepository CorridaRepository, IPagaresRepository PagaresRepository, ISucursalRepository SucursalesRepository, IPacientesRepository pacientesRepository, IContratosRepository ContratosRepository, IProductosRepository productosRepository)
        {
            _ContratosRepository = ContratosRepository;
            _ProductosRepository = productosRepository;
            _PacientesRepository = pacientesRepository;
            _SucursalesRepository = SucursalesRepository;
            _CorridaRepository = CorridaRepository;
            _PagaresRepository = PagaresRepository;
        }

        public int InsertUpdateContrato(Contrato Contrato, List<corridaOriginal> corrida, out string Message)
        {
            Message = string.Empty;
            int result = 0;
            try
            {
                result = _ContratosRepository.InsertOrUpdate<int>(Contrato);
                Contrato _Contrato = _ContratosRepository.Get(result);
                Sucursale _Sucursal = _SucursalesRepository.Get(_Contrato.ID_Sucursal);
                _Contrato.ClaveContrato = _Sucursal.Abreviacion +" "+ _Contrato.ID;
                result = _ContratosRepository.InsertOrUpdate<int>(_Contrato);
                foreach (var item in corrida)
                {
                    pagare npagare = new pagare();
                    npagare.claveContrato = result;
                    npagare.fecha = item.fecha;
                    npagare.Descripcion = item.Descripcion;
                    npagare.importeOriginal = item.importe;
                    npagare.gastoCobranza = 0;
                    npagare.importeTotal = item.importe;
                    npagare.importePagado = 0;
                    npagare.resta = item.importe;
                    npagare.Saldo = item.Saldo;
                    npagare.Status = 1;
                    item.claveContrato = result;
                    _PagaresRepository.InsertOrUpdate<int>(npagare);
                    _CorridaRepository.InsertOrUpdate<int>(item);
                }

                Message = "Contrato guardado con exito";
            }
            catch (Exception ex)
            {

                Message = "Contrato No pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public int GetLastContrato()
        {
            Sql query = new Sql(@"select top 1 ID from contratos order by ID DESC;");
            var res = _ContratosRepository.GetByDynamicFilter(query);
            return res[0].ID;
        }

        public Contrato GetContrato(int id)
        {
            return _ContratosRepository.Get(id);
        }

        public Producto GetProductoSelected(int id)
        {
            return _ProductosRepository.Get(id);
        }

        public Paciente GetPacienteSelected(int id)
        {
            return _PacientesRepository.Get(id);
        }

    }
}
