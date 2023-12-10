using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IContratoService
    {
        public int GetLastContrato();
        public Producto GetProductoSelected(int id);
        int InsertUpdateContrato(Contrato Contrato, List<corridaOriginal> corrida, List<DetalleContrato> detallesContrato, out string Message);
        public Paciente GetPacienteSelected(int id);
        public Contrato GetContrato(int id);
        public List<dynamic> GetAlmacenesDeSucursal(int id);

    }



    public class ContratoService : IContratoService
    {
        private readonly IContratosRepository _ContratosRepository;
        private readonly IContratosDetalleRepository _ContratosDetalleRepository;
        private readonly IProductosRepository _ProductosRepository;
        private readonly IPacientesRepository _PacientesRepository;
        private readonly ISucursalRepository _SucursalesRepository;
        private readonly ICorridaRepository _CorridaRepository;
        private readonly IPagaresRepository _PagaresRepository;
        private readonly IExistenciasAlmacenRepository _ExistenciasAlmacenRepository;
        private readonly IKardexProductosRepository _KardexProductosRepository;

        public ContratoService(IKardexProductosRepository KardexProductosRepository, IExistenciasAlmacenRepository ExistenciasAlmacenRepository, ICorridaRepository CorridaRepository, IPagaresRepository PagaresRepository, IContratosDetalleRepository ContratosDetalleRepository, ISucursalRepository SucursalesRepository, IPacientesRepository pacientesRepository, IContratosRepository ContratosRepository, IProductosRepository productosRepository)
        {
            _ContratosRepository = ContratosRepository;
            _ProductosRepository = productosRepository;
            _PacientesRepository = pacientesRepository;
            _SucursalesRepository = SucursalesRepository;
            _CorridaRepository = CorridaRepository;
            _PagaresRepository = PagaresRepository;
            _ContratosDetalleRepository = ContratosDetalleRepository;
            _ExistenciasAlmacenRepository = ExistenciasAlmacenRepository;
            _KardexProductosRepository = KardexProductosRepository;
        }

        public int InsertUpdateContrato(Contrato Contrato, List<corridaOriginal> corrida, List<DetalleContrato> detallesContrato, out string Message)
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
                foreach (var detalle in detallesContrato)
                {
                    KardexProducto kp = new KardexProducto();
                    detalle.contrato = result;
                    detalle.costoTotal = detalle.cantidad * detalle.costo;
                    var producto = _ProductosRepository.Get(detalle.producto);
                    var existencia = _ExistenciasAlmacenRepository.GetExistenciaAlmacen(detalle.producto, _Contrato.ID_Almacen);
                    kp.Fecha = _Contrato.Fecha;
                    kp.ID_Producto = detalle.producto;
                    kp.ID_Almacen = _Contrato.ID_Almacen;
                    kp.ID_EntradaSalida = 2;
                    kp.ID_TipoEntradaSalida = 3;
                    kp.ID_Movimiento = _Contrato.ID;
                    kp.Cantidad = detalle.cantidad;
                    kp.CantidadTotal = producto.Cantidad - kp.Cantidad;
                    kp.CantidadDisponibleTotal = (int)producto.Disponible - kp.Cantidad;
                    kp.CantidadTotalAlmacen = existencia.Cantidad - kp.Cantidad;
                    kp.CantidadDisponibleAlmacen = (int)existencia.Disponible - kp.Cantidad;
                    kp.Costo = detalle.costo;
                    kp.CostoPromedio = (decimal)producto.Costo;
                    kp.Observaciones = "";
                    kp.Referencia = "";
                    producto.Cantidad = kp.CantidadTotal;
                    producto.Disponible = kp.CantidadDisponibleTotal;
                    existencia.Disponible = kp.CantidadDisponibleAlmacen;
                    existencia.Cantidad = kp.CantidadTotalAlmacen;
                    _KardexProductosRepository.InsertOrUpdate<int>(kp);
                    _ContratosDetalleRepository.InsertOrUpdate<int>(detalle);
                    _ExistenciasAlmacenRepository.InsertOrUpdate<int>(existencia);
                    _ProductosRepository.InsertOrUpdate<int>(producto);
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

        public List<dynamic> GetAlmacenesDeSucursal(int id)
        {
            Sql query = new Sql(@"select * from almacenes where ID_Sucursal = @0", id);
            return _ContratosRepository.GetByDynamicFilter(query);
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
