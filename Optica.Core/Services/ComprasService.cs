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
    public interface IComprasService
    {
        Compra GetCompra(int id);
        List<ComprasDetalle> GetCompraDetalles(int id);
        List<Compra> GetCompraProductos();
        List<dynamic> GetComprasFiltro(string from, string to, int? idproveedor, string folio = null, string factura = null);
        bool InsertUpdateCompra(Compra compraProducto, List<ComprasDetalle> compraProductoDetalles, int idUsuario, out string Message);
        bool CancelarCompra(int id, int idUsuario, out string Message);
    }

    public class ComprasService : IComprasService
    {
        private readonly ICompraRepository _compraProductoRepository;
        private readonly ICompraDetalleRepository _compraProductoDetalleRepository;
        private readonly ILogRepository _logRepository;
        

        public ComprasService(ICompraRepository compraProductoRepository, ICompraDetalleRepository compraProductoDetalleRepository, ILogRepository logRepository) {
            _compraProductoRepository = compraProductoRepository;
            _compraProductoDetalleRepository = compraProductoDetalleRepository;
            _logRepository = logRepository;
        }

        public Compra GetCompra(int id) {
            return _compraProductoRepository.Get(id);
        }

        public List<ComprasDetalle> GetCompraDetalles(int id)
        {
            Sql query = new Sql()
                .Select("*").From("ComprasDetalles")
                .Where("ID_Compra = @0", id);
            return _compraProductoDetalleRepository.GetByFilter(query);
        }

        public List<Compra> GetCompraProductos() {
            return _compraProductoRepository.GetAll("Compras").ToList();
        }

        public List<dynamic> GetComprasFiltro(string from, string to, int? idproveedor, string folio = null, string factura = null)
        {
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(folio) || !string.IsNullOrEmpty(factura) || !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) || idproveedor != null)
            {
                filter = " Where ";
            }

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                filter += string.Format("C.FechaFactura between '{0} 00:00:00' and '{1} 23:59:59' ", from, to);
            }
            if (idproveedor != null)
            {
                filter += string.Format("{1} C.ID_Proveedor = '{0}' ", idproveedor, (filter.Length > 0 ? "and" : ""));
            }
            if (!string.IsNullOrEmpty(folio))
            {
                filter += string.Format("{1} C.ID like '%{0}%' ", folio, (filter.Length > 0 ? "and" : ""));
            }
            if (!string.IsNullOrEmpty(factura))
            {
                filter += string.Format("{1} C.ClaveFactura like '%{0}%' ", factura, (filter.Length > 0 ? "and" : ""));
            }
            Sql query = new Sql(@"select C.*, S.Nombre as Sucursal, P.NombreComercial as Proveedor from Compras C 
                                    inner join Almacenes A on A.ID = C.ID_Almacen
                                    inner join Sucursales S on S.ID = A.ID_Sucursal
                                    inner join Proveedores P on P.ID = C.ID_Proveedor" + (filter.Length > 0 ? filter : ""));
            return _compraProductoRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateCompra(Compra compraProducto, List<ComprasDetalle> compraProductoDetalles, int idUsuario,  out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {

                int id = _compraProductoRepository.InsertOrUpdate<int>(compraProducto);
                List<ComprasDetalle> _detallesActuales = GetCompraDetalles(id);

                // Eliminar detalles que no existen en los actuales
                foreach (var detalleNoExiste in _detallesActuales.Where(p => !compraProductoDetalles.Any(p2 => p2.ID == p.ID)))
                {
                    _compraProductoDetalleRepository.Remove(detalleNoExiste);
                }

                //Insertar o Actualizar detalles existentes o nuevos
                foreach (var detalle in compraProductoDetalles)
                {
                    if (detalle.ID <= 0)
                    {
                        detalle.ID_Compra = id;
                        _compraProductoDetalleRepository.InsertOrUpdate<int>(detalle);
                    }

                    _compraProductoDetalleRepository.InsertOrUpdate<int>(detalle);
                }

                Log log = new Log();
                log.Tipo = "Compras";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                if (compraProducto.ID <= 0)
                {
                    log.Accion = "Compra Nueva Guardada";
                }
                else
                {
                    log.Accion = compraProducto.Estatus == "P" ? "Compra Procesada Actualizada" : "Compra Actualizada";
                }
                _logRepository.InsertOrUpdate<int>(log);

                Message = "Compra guardada con éxito";
                result = true;
            }
            catch (Exception ex)
            {
                Message = "Compra guardada No pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool CancelarCompra(int id, int idUsuario, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var compra = _compraProductoRepository.Get(id);
                List<ComprasDetalle> detalles = GetCompraDetalles(id);

                // Eliminar detalles
                foreach (var detalle in detalles)
                {
                    _compraProductoDetalleRepository.Remove(detalle);
                }

                compra.Subtotal = 0;
                compra.Iva = 0;
                compra.Total = 0;
                compra.Estatus = "C";

                _compraProductoRepository.InsertOrUpdate<int>(compra);

                Log log = new Log();
                log.Tipo = "Compras";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Compra Cancelada";
                _logRepository.InsertOrUpdate<int>(log);

                Message = "Compra cancelada con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Compra No pudo ser cancelada Error: " + ex.Message;
            }
            return result;
        }
    }
}
