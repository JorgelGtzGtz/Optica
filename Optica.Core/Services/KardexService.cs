using dbconnection;
using Optica.Core.Config;
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
    public interface IKardexService
    {
        bool ProcesarCompra(int id, int idUsuario, out string Message);
        bool ProcesarVenta(int id, int idUsuario, out string Message);
    }

    public class KardexService : IKardexService
    {
        private readonly ICompraRepository _compraProductoRepository;
        private readonly ICompraDetalleRepository _compraProductoDetalleRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IVentaDetalleRepository _ventaDetalleRepository;

        private readonly IKardexProductosRepository _kardexProductosRepository;
        private readonly IExistenciasAlmacenRepository _existenciasAlmacenRepository;
        private readonly IProductosRepository _productosRepository;

        private readonly ILogRepository _logRepository;

        

        public KardexService(ICompraRepository compraProductoRepository, ICompraDetalleRepository compraProductoDetalleRepository, ILogRepository logRepository,
            IKardexProductosRepository kardexProductosRepository, IExistenciasAlmacenRepository existenciasAlmacenRepository, IProductosRepository productosRepository,
            IVentaRepository ventaRepository, IVentaDetalleRepository ventaDetalleRepository) {
            _compraProductoRepository = compraProductoRepository;
            _compraProductoDetalleRepository = compraProductoDetalleRepository;
            _kardexProductosRepository = kardexProductosRepository;
            _existenciasAlmacenRepository = existenciasAlmacenRepository;
            _productosRepository = productosRepository;
            _logRepository = logRepository;

            _ventaRepository = ventaRepository;
            _ventaDetalleRepository = ventaDetalleRepository;
        }

        public bool ProcesarCompra(int id, int idUsuario, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var compra = _compraProductoRepository.Get(id);
                Sql query = new Sql().Select("*").From("ComprasDetalles").Where("ID_Compra = @0", id);
                List<ComprasDetalle> detalles = _compraProductoDetalleRepository.GetByFilter(query);

                
                _kardexProductosRepository.BeginTransaction();
                _productosRepository.BeginTransaction();
                _existenciasAlmacenRepository.BeginTransaction();
                _compraProductoRepository.BeginTransaction();
                foreach (var detalle in detalles)
                {
                    Sql queryProd = new Sql("SELECT * FROM [dbo].[Productos] with(nolock) where [ID] = @0", detalle.ID_Producto);
                    var producto = _productosRepository.Get(queryProd);

                    Sql queryExi = new Sql("SELECT * FROM [dbo].[ExistenciasAlmacen] with(nolock) where [ID_Almacen] = @0 and [ID_Producto] = @1", compra.ID_Almacen, detalle.ID_Producto);
                    var existencia = _existenciasAlmacenRepository.Get(queryExi);

                    KardexProducto kardexProducto = new KardexProducto();
                    kardexProducto.Fecha = compra.FechaFactura;
                    kardexProducto.Cantidad = detalle.Cantidad;
                    kardexProducto.CantidadTotal = (producto?.Cantidad ?? 0) + detalle.Cantidad;
                    kardexProducto.CantidadTotalAlmacen = existencia != null ? existencia.Cantidad + detalle.Cantidad : detalle.Cantidad;
                    kardexProducto.CantidadDisponibleTotal = (producto?.Disponible ?? 0) + detalle.Cantidad;
                    kardexProducto.CantidadDisponibleAlmacen = existencia != null ? existencia.Disponible + detalle.Cantidad : detalle.Cantidad;
                    kardexProducto.Costo = detalle.CostoConDescuento;
                    if (producto.Costo <= 0 || kardexProducto.CantidadTotal > 0)
                    {
                        kardexProducto.CostoPromedio = ((producto?.Cantidad ?? 0 * producto?.Costo ?? 0) + (kardexProducto.Cantidad * kardexProducto.Costo))
                                                            / (producto?.Cantidad ?? 0 + kardexProducto.Cantidad);
                    }
                    else if (kardexProducto.CantidadTotal <= 0 && producto.Costo > 0)
                    {
                        kardexProducto.CostoPromedio = producto?.Costo ?? 0;
                    }
                    else
                    {
                        kardexProducto.CostoPromedio = kardexProducto.Costo;
                    }

                    kardexProducto.Referencia = string.Empty;
                    kardexProducto.Observaciones = string.Empty;

                    kardexProducto.ID_Producto = detalle.ID_Producto;
                    kardexProducto.ID_Almacen = compra.ID_Almacen;
                    kardexProducto.ID_TipoEntradaSalida = 1;
                    kardexProducto.ID_EntradaSalida = 1;
                    kardexProducto.ID_Movimiento = compra.ID;

                    _kardexProductosRepository.InsertOrUpdate<int>(kardexProducto);

                    producto.Cantidad = kardexProducto.CantidadTotal;
                    producto.Disponible = kardexProducto.CantidadDisponibleTotal;
                    producto.Costo = kardexProducto.CostoPromedio;

                    _productosRepository.InsertOrUpdate<int>(producto);

                    if (existencia != null)
                    {
                        existencia.Cantidad = kardexProducto.CantidadTotalAlmacen;
                        existencia.Disponible = kardexProducto?.CantidadDisponibleAlmacen ?? 0;
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existencia);
                    }
                    else
                    {
                        ExistenciasAlmacen existenciasAlmacen = new ExistenciasAlmacen();
                        existenciasAlmacen.Cantidad = kardexProducto.CantidadTotalAlmacen;
                        existenciasAlmacen.Disponible = kardexProducto?.CantidadDisponibleAlmacen ?? 0;
                        existenciasAlmacen.ID_Almacen = compra.ID_Almacen;
                        existenciasAlmacen.ID_Producto = detalle.ID_Producto; 
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existenciasAlmacen);
                    }
                }

                compra.Estatus = "P";

                _compraProductoRepository.InsertOrUpdate<int>(compra);

                Log log = new Log();
                log.Tipo = "Compras";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Compra Procesada";
                _logRepository.InsertOrUpdate<int>(log);

                _kardexProductosRepository.CommitTransaction();
                _productosRepository.CommitTransaction();
                _existenciasAlmacenRepository.CommitTransaction();
                _compraProductoRepository.CommitTransaction();
                Message = "Compra procesada con exito";
                result = true;
            }
            catch (Exception ex)
            {
                _kardexProductosRepository.RollBackTransaction();
                _productosRepository.RollBackTransaction();
                _existenciasAlmacenRepository.RollBackTransaction();
                _compraProductoRepository.RollBackTransaction();
                Message = "Compra No pudo ser procesada Error: " + ex.Message;
            }
            return result;
        }

        public bool ProcesarVenta(int id, int idUsuario, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var modelo = _ventaRepository.Get(id);
                Sql query = new Sql().Select("*").From("VentasDetalle").Where("ID_Venta = @0", id);
                List<VentasDetalle> detalles = _ventaDetalleRepository.GetByFilter(query);


                _kardexProductosRepository.BeginTransaction();
                _productosRepository.BeginTransaction();
                _existenciasAlmacenRepository.BeginTransaction();
                _ventaRepository.BeginTransaction();
                foreach (var detalle in detalles)
                {
                    Sql queryProd = new Sql("SELECT * FROM [dbo].[Productos] with(nolock) where [ID] = @0", detalle.ID_Producto);
                    var producto = _productosRepository.Get(queryProd);

                    Sql queryExi = new Sql("SELECT * FROM [dbo].[ExistenciasAlmacen] with(nolock) where [ID_Almacen] = @0 and [ID_Producto] = @1", modelo.ID_Almacen, detalle.ID_Producto);
                    var existencia = _existenciasAlmacenRepository.Get(queryExi);

                    KardexProducto kardexProducto = new KardexProducto();
                    kardexProducto.Fecha = modelo.Fecha;
                    kardexProducto.Cantidad = detalle?.Cantidad ?? 0;
                    kardexProducto.CantidadTotal = (producto?.Cantidad ?? 0) - detalle?.Cantidad ?? 0;
                    kardexProducto.CantidadTotalAlmacen = existencia != null ? existencia.Cantidad - detalle?.Cantidad ?? 0 : 0 - detalle?.Cantidad ?? 0;
                    kardexProducto.CantidadDisponibleTotal = (producto?.Disponible ?? 0) - detalle?.Cantidad ?? 0;
                    kardexProducto.CantidadDisponibleAlmacen = existencia != null ? existencia.Disponible - detalle?.Cantidad ?? 0 : 0 - detalle?.Cantidad ?? 0;
                    kardexProducto.Costo = producto?.Costo ?? 0;
                    kardexProducto.CostoPromedio = producto?.Costo ?? 0;

                    kardexProducto.Referencia = string.Empty;
                    kardexProducto.Observaciones = string.Empty;

                    kardexProducto.ID_Producto = detalle.ID_Producto;
                    kardexProducto.ID_Almacen = modelo.ID_Almacen;
                    kardexProducto.ID_TipoEntradaSalida = 2;
                    kardexProducto.ID_EntradaSalida = 2;
                    kardexProducto.ID_Movimiento = modelo.ID;

                    _kardexProductosRepository.InsertOrUpdate<int>(kardexProducto);

                    producto.Cantidad = kardexProducto.CantidadTotal;
                    producto.Disponible = kardexProducto.CantidadDisponibleTotal;

                    _productosRepository.InsertOrUpdate<int>(producto);

                    if (existencia != null)
                    {
                        existencia.Cantidad = kardexProducto.CantidadTotalAlmacen;
                        existencia.Disponible = kardexProducto?.CantidadDisponibleAlmacen ?? 0;
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existencia);
                    }
                    else
                    {
                        ExistenciasAlmacen existenciasAlmacen = new ExistenciasAlmacen();
                        existenciasAlmacen.Cantidad = kardexProducto.CantidadTotalAlmacen;
                        existenciasAlmacen.Disponible = kardexProducto?.CantidadDisponibleAlmacen ?? 0;
                        existenciasAlmacen.ID_Almacen = modelo.ID_Almacen;
                        existenciasAlmacen.ID_Producto = detalle.ID_Producto;
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existenciasAlmacen);
                    }
                }

                modelo.Estatus = "P";

                _ventaRepository.InsertOrUpdate<int>(modelo);

                Log log = new Log();
                log.Tipo = "Ventas";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Venta Procesada";
                _logRepository.InsertOrUpdate<int>(log);

                _kardexProductosRepository.CommitTransaction();
                _productosRepository.CommitTransaction();
                _existenciasAlmacenRepository.CommitTransaction();
                _ventaRepository.CommitTransaction();
                Message = "Venta procesada con exito";
                result = true;
            }
            catch (Exception ex)
            {
                _kardexProductosRepository.RollBackTransaction();
                _productosRepository.RollBackTransaction();
                _existenciasAlmacenRepository.RollBackTransaction();
                _ventaRepository.RollBackTransaction();
                Message = "Compra No pudo ser procesada Error: " + ex.Message;
            }
            return result;
        }
    }
}
