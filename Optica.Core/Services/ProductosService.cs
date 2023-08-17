using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Entities.Dto;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IProductosService
    {
        Producto GetProducto(int id);
        List<Producto> GetProductos();
        List<Producto> GetProductosFiltro(string nombre = null);
        bool InsertUpdateProducto(Producto model, out string Message, List<ProductosDetalleKit> productoKits = null);
        bool EliminarProducto(int id, out string Message);
        List<ProductosDetalleKit> GetProductosKit(int id);
        List<dynamic> GetListaDetallesKit(int productoid, int almacenid, int cantidad);
        bool GenerarProductoKIT(GenerarKitDto model, int idUsuario, out string Message);
    }

    public class ProductosService : IProductosService
    {
        private readonly IProductosRepository _productosRepository;
        private readonly IProductosDetalleKitRepository _productosDetalleKitRepository;
        private readonly IExistenciasAlmacenRepository _existenciasAlmacenRepository;
        private readonly ILogRepository _logRepository;
        private readonly IOtrasEntradasSalidasRepository _otrasEntradasSalidasRepository;
        private readonly IOtrasEntradasSalidasDetallesRepository _otrasEntradasSalidasDetallesRepository;
        private readonly IKardexProductosRepository _kardexProductosRepository;

        public ProductosService(IProductosRepository productosRepository, IProductosDetalleKitRepository productosDetalleKitRepository, IExistenciasAlmacenRepository existenciasAlmacenRepository, 
            ILogRepository logRepository, IOtrasEntradasSalidasRepository otrasEntradasSalidasRepository, IOtrasEntradasSalidasDetallesRepository otrasEntradasSalidasDetallesRepository,
            IKardexProductosRepository kardexProductosRepository) {
            _productosRepository = productosRepository;
            _productosDetalleKitRepository = productosDetalleKitRepository;
            _existenciasAlmacenRepository = existenciasAlmacenRepository;
            _logRepository = logRepository;
            _otrasEntradasSalidasRepository = otrasEntradasSalidasRepository;
            _otrasEntradasSalidasDetallesRepository = otrasEntradasSalidasDetallesRepository;
            _kardexProductosRepository = kardexProductosRepository;
        }

        public Producto GetProducto(int id)
        {
            return _productosRepository.Get(id);
        }

        public List<Producto> GetProductos() {
            return _productosRepository.GetAll("Productos").ToList();
        }

        public List<Producto> GetProductosFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Descripcion like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Productos " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _productosRepository.GetByFilter(query);
        }

        public bool InsertUpdateProducto(Producto model, out string Message, List<ProductosDetalleKit> productoKits = null)
        {

            Message = string.Empty;
            bool result = false;
            try
            {
                var id = _productosRepository.InsertOrUpdate<int>(model);
                if (productoKits != null)
                {
                    foreach (var item in productoKits)
                    {
                        item.ID_Producto_Base = id;
                        _productosDetalleKitRepository.InsertOrUpdate<int>(item);
                    }
                }

                Message = "Producto guardada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Producto No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarProducto(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                Sql query = new Sql()
                .Select("*").From("ProductosDetalleKit")
                .Where("ID_Producto_Base = @0", id);
                var kits = _productosDetalleKitRepository.GetByFilter(query);
                foreach (var item in kits)
                {
                    _productosDetalleKitRepository.Remove(item);
                }

                var model = _productosRepository.Get(id);

                _productosRepository.Remove(model);

                Message = "Producto eliminada " + model.Descripcion + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Producto no pudo ser eliminada Error: " + ex.Message;
            }
            return result;
        }

        public List<ProductosDetalleKit> GetProductosKit(int id)
        {
            Sql query = new Sql(@"select * from ProductosDetalleKit Where ID_Producto_Base = @0", id);
            return _productosDetalleKitRepository.GetByFilter(query);
        }


        public bool GenerarProductoKIT(GenerarKitDto model, int idUsuario, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            var detalles = GetProductosKit(model.ID_Producto);
            foreach (var item in detalles)
            {
                var existencia = _existenciasAlmacenRepository.GetExistenciaAlmacen(item.ID_Producto, model.ID_Almacen);
                if (existencia != null)
                {
                    if (existencia.Cantidad < (item.Cantidad * model.Cantidad))
                    {
                        result = false;
                        Message = "Cantidad insuficiente de producto: " + item.Descripcion.ToString() + " - " + "Almacen: " + model.ID_Almacen.ToString();
                        break;
                    }
                }
                else
                {
                    result = false;
                    Message = "Cantidad insuficiente de producto: " + item.Descripcion.ToString() + " - " + "Almacen: " + model.ID_Almacen.ToString();
                    break;
                }
            }

            if (result)
            {
                result = GenerarEntradaSalidaKIT(model, idUsuario, out Message);
            }
            
            return result;
        }

        public List<dynamic> GetListaDetallesKit(int productoid, int almacenid, int cantidad)
        {
            var detalles = GetProductosKit(productoid);
            List<object> listkit = new List<object>();
            foreach (var item in detalles)
            {
                int existencia_disponible = 0;
                var producto = GetProducto(item.ID_Producto);
                var existencia = _existenciasAlmacenRepository.GetExistenciaAlmacen(item.ID_Producto, almacenid);
                if (existencia != null)
                {
                    existencia_disponible = existencia.Cantidad;
                }

                listkit.Add(new
                {
                    ID_Producto = item.ID_Producto,
                    Cantidad = cantidad,
                    CantidadTotal = cantidad * item.Cantidad,
                    Costo = producto.Costo,
                    Total = producto.Costo * (cantidad * item.Cantidad),
                    Clave = producto.Clave,
                    Descripcion = producto.Descripcion,
                    Existencia = existencia_disponible

                });
            }

            return listkit;
        }

        public bool GenerarEntradaSalidaKIT(GenerarKitDto model, int idUsuario, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            try
            {
                var producto = GetProducto(model.ID_Producto);
                // Generar entrada
                OtrasEntradasSalida otrasEntrada = new OtrasEntradasSalida();
                OtrasEntradasSalidasDetalle otrasEntradaDetalles = new OtrasEntradasSalidasDetalle();
                otrasEntrada.Fecha = model.Fecha;
                otrasEntrada.Referencia = string.Format("Generacion KIT Producto Base Clave: {0} Desc: {1} Folio: {2} Cantidad: {3}", producto.Clave, producto.Descripcion, producto.ID, model.Cantidad);
                otrasEntrada.Observaciones = string.Empty;
                otrasEntrada.ID_EntradaSalida = 1;
                otrasEntrada.ID_TipoEntradaSalida = 3;
                otrasEntrada.ID_Almacen = model.ID_Almacen;
                otrasEntrada.ID_Usuario = idUsuario;
                otrasEntrada.Estatus = "G";

                decimal costo = 0;
                var detalles = GetProductosKit(model.ID_Producto);
                foreach (var item in detalles)
                {
                    var productoKit = GetProducto(item.ID_Producto);
                    costo += productoKit?.Costo ?? 0;
                }

                otrasEntrada.Total = costo * model.Cantidad;
                int idEntrada = _otrasEntradasSalidasRepository.InsertOrUpdate<int>(otrasEntrada);

                otrasEntradaDetalles.Cantidad = model.Cantidad;
                otrasEntradaDetalles.Costo = costo;
                otrasEntradaDetalles.CostoTotal = otrasEntradaDetalles.Cantidad * otrasEntradaDetalles.Costo;
                otrasEntradaDetalles.ID_Producto = model.ID_Producto;
                otrasEntradaDetalles.ID_OtraEntradasSalidas = idEntrada;

                _otrasEntradasSalidasDetallesRepository.InsertOrUpdate<int>(otrasEntradaDetalles);

                Log log = new Log();
                log.Tipo = "Otras Entradas Salidas";
                log.ID_Referencia = idEntrada;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Entrada Nueva Guardada Por Generacion de Producto Kit";
                _logRepository.InsertOrUpdate<int>(log);

                // Generar Salida
                OtrasEntradasSalida otrasSalida = new OtrasEntradasSalida();
                List<OtrasEntradasSalidasDetalle> otrasSalidaDetalles = new List<OtrasEntradasSalidasDetalle>();

                otrasSalida.Fecha = model.Fecha;
                otrasSalida.Referencia = string.Format("Generacion KIT Producto Base Clave: {0} Desc: {1} Folio: {2} Cantidad: {3}", producto.Clave, producto.Descripcion, producto.ID, model.Cantidad);
                otrasSalida.Observaciones = string.Empty;
                otrasSalida.ID_EntradaSalida = 2;
                otrasSalida.ID_TipoEntradaSalida = 4;
                otrasSalida.ID_Almacen = model.ID_Almacen;
                otrasSalida.ID_Usuario = idUsuario;
                otrasSalida.Estatus = "G";

                foreach (var item in detalles)
                {
                    var productoKit = GetProducto(item.ID_Producto);
                    OtrasEntradasSalidasDetalle otraSalidaDetalle = new OtrasEntradasSalidasDetalle();
                    otraSalidaDetalle.Cantidad = item?.Cantidad ?? 0;
                    otraSalidaDetalle.Costo = productoKit?.Costo ?? 0;
                    otraSalidaDetalle.CostoTotal = otraSalidaDetalle.Cantidad * otraSalidaDetalle.Costo;
                    otraSalidaDetalle.ID_Producto = item.ID_Producto;
                    otrasSalidaDetalles.Add(otraSalidaDetalle);
                }
                otrasSalida.Total = otrasSalidaDetalles.Sum(x => x.CostoTotal);

                int idSalida = _otrasEntradasSalidasRepository.InsertOrUpdate<int>(otrasSalida);

                foreach (var salidasDetalle in otrasSalidaDetalles)
                {
                    salidasDetalle.ID_OtraEntradasSalidas = idSalida;
                    _otrasEntradasSalidasDetallesRepository.InsertOrUpdate<int>(salidasDetalle);
                }

                log = new Log();
                log.Tipo = "Otras Entradas Salidas";
                log.ID_Referencia = idSalida;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Salida Nueva Guardada Por Generacion de Producto Kit";
                _logRepository.InsertOrUpdate<int>(log);


                // Procesar entrada
                var resultEntrada = ProcesarEntradaKIT(idEntrada, idUsuario, out Message);

                // Procesar salida
                var resultSalida = ProcesarSalidaKIT(idSalida, idUsuario, out Message);

                if (resultEntrada && resultSalida)
                {
                    result = true;
                }
                else
                {
                    Message = String.Format("Error al procesar entrada/salida, favor de procesar manualmente");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Message = String.Format("Error al generar kit: " + ex.Message);
                result = false;
            }

            return result;
        }

        public bool ProcesarEntradaKIT(int id, int idUsuario, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            try
            {
                var model = _otrasEntradasSalidasRepository.Get(id);
                Sql query = new Sql().Select("*").From("OtrasEntradasSalidasDetalles").Where("ID_OtraEntradasSalidas = @0", id);
                var detalles = _otrasEntradasSalidasDetallesRepository.GetByFilter(query);

                if (model.Estatus == "P")
                {
                    throw new Exception("Entrada Folio: " + model.ID + " fue procesada previamente." );
                }

                _kardexProductosRepository.BeginTransaction();
                _productosRepository.BeginTransaction();
                _existenciasAlmacenRepository.BeginTransaction();
                _otrasEntradasSalidasRepository.BeginTransaction();
                foreach (var detalle in detalles)
                {
                    Sql queryProd = new Sql("SELECT * FROM [dbo].[Productos] with(nolock) where [ID] = @0", detalle.ID_Producto);
                    var producto = _productosRepository.Get(queryProd);

                    Sql queryExi = new Sql("SELECT * FROM [dbo].[ExistenciasAlmacen] with(nolock) where [ID_Almacen] = @0 and [ID_Producto] = @1", model.ID_Almacen, detalle.ID_Producto);
                    var existencia = _existenciasAlmacenRepository.Get(queryExi);

                    KardexProducto kardexProducto = new KardexProducto();
                    kardexProducto.Fecha = model.Fecha;
                    kardexProducto.Cantidad = detalle.Cantidad;
                    kardexProducto.CantidadTotal = (producto?.Cantidad ?? 0) + detalle.Cantidad;
                    kardexProducto.CantidadTotalAlmacen = existencia != null ? existencia.Cantidad + detalle.Cantidad : detalle.Cantidad;
                    kardexProducto.CantidadDisponibleTotal = (producto?.Disponible ?? 0) + detalle.Cantidad;
                    kardexProducto.CantidadDisponibleAlmacen = existencia != null ? existencia.Disponible + detalle.Cantidad : detalle.Cantidad;
                    kardexProducto.Costo = detalle.Costo;
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
                    kardexProducto.ID_Almacen = model.ID_Almacen;
                    kardexProducto.ID_TipoEntradaSalida = model.ID_TipoEntradaSalida;
                    kardexProducto.ID_EntradaSalida = model.ID_EntradaSalida;
                    kardexProducto.ID_Movimiento = model.ID;

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
                        existenciasAlmacen.ID_Almacen = model.ID_Almacen;
                        existenciasAlmacen.ID_Producto = detalle.ID_Producto;
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existenciasAlmacen);
                    }
                }

                model.Estatus = "P";

                _otrasEntradasSalidasRepository.InsertOrUpdate<int>(model);

                Log log = new Log();
                log.Tipo = "Otras Entradas Salidas";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Entrada Procesada Por Generacion de Producto Kit";
                _logRepository.InsertOrUpdate<int>(log);

                _kardexProductosRepository.CommitTransaction();
                _productosRepository.CommitTransaction();
                _existenciasAlmacenRepository.CommitTransaction();
                _otrasEntradasSalidasRepository.CommitTransaction();
                Message = "Entrada procesada con exito";
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }

            return result;
        }

        public bool ProcesarSalidaKIT(int id, int idUsuario, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            try
            {
                var model = _otrasEntradasSalidasRepository.Get(id);
                Sql query = new Sql().Select("*").From("OtrasEntradasSalidasDetalles").Where("ID_OtraEntradasSalidas = @0", id);
                var detalles = _otrasEntradasSalidasDetallesRepository.GetByFilter(query);

                if (model.Estatus == "P")
                {
                    throw new Exception("Salida Folio: " + model.ID + " fue procesada previamente.");
                }

                _kardexProductosRepository.BeginTransaction();
                _productosRepository.BeginTransaction();
                _existenciasAlmacenRepository.BeginTransaction();
                _otrasEntradasSalidasRepository.BeginTransaction();
                foreach (var detalle in detalles)
                {
                    Sql queryProd = new Sql("SELECT * FROM [dbo].[Productos] with(nolock) where [ID] = @0", detalle.ID_Producto);
                    var producto = _productosRepository.Get(queryProd);

                    Sql queryExi = new Sql("SELECT * FROM [dbo].[ExistenciasAlmacen] with(nolock) where [ID_Almacen] = @0 and [ID_Producto] = @1", model.ID_Almacen, detalle.ID_Producto);
                    var existencia = _existenciasAlmacenRepository.Get(queryExi);

                    KardexProducto kardexProducto = new KardexProducto();
                    kardexProducto.Fecha = model.Fecha;
                    kardexProducto.Cantidad = detalle.Cantidad;
                    kardexProducto.CantidadTotal = (producto?.Cantidad ?? 0) - detalle.Cantidad;
                    kardexProducto.CantidadTotalAlmacen = existencia != null ? existencia.Cantidad - detalle.Cantidad : 0 - detalle.Cantidad;
                    kardexProducto.CantidadDisponibleTotal = (producto?.Disponible ?? 0) - detalle.Cantidad;
                    kardexProducto.CantidadDisponibleAlmacen = existencia != null ? existencia.Disponible - detalle.Cantidad : 0 - detalle.Cantidad;
                    kardexProducto.Costo = detalle.Costo;
                    kardexProducto.CostoPromedio = kardexProducto.Costo;

                    kardexProducto.Referencia = string.Empty;
                    kardexProducto.Observaciones = string.Empty;

                    kardexProducto.ID_Producto = detalle.ID_Producto;
                    kardexProducto.ID_Almacen = model.ID_Almacen;
                    kardexProducto.ID_TipoEntradaSalida = model.ID_TipoEntradaSalida;
                    kardexProducto.ID_EntradaSalida = model.ID_EntradaSalida;
                    kardexProducto.ID_Movimiento = model.ID;

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
                        existenciasAlmacen.ID_Almacen = model.ID_Almacen;
                        existenciasAlmacen.ID_Producto = detalle.ID_Producto;
                        _existenciasAlmacenRepository.InsertOrUpdate<int>(existenciasAlmacen);
                    }
                }

                model.Estatus = "P";

                _otrasEntradasSalidasRepository.InsertOrUpdate<int>(model);

                Log log = new Log();
                log.Tipo = "Otras Entradas Salidas";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Salida Procesada Por Generacion de Producto Kit";
                _logRepository.InsertOrUpdate<int>(log);

                _kardexProductosRepository.CommitTransaction();
                _productosRepository.CommitTransaction();
                _existenciasAlmacenRepository.CommitTransaction();
                _otrasEntradasSalidasRepository.CommitTransaction();
                Message = "Salida procesada con exito";
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }

            return result;
        }
    }
}
