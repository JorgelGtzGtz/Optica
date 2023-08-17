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
    public interface IVentasService
    {
        Venta GetVenta(int id);
        List<VentasDetalle> GetVentaDetalles(int id);
        List<Venta> GetVentas();
        List<dynamic> GetVentasFiltro(string from, string to, int? idcliente, int? idsucursal, int? idvendedor, int? idproducto, string folio = null, string folioalt = null);
        bool InsertUpdateVenta(Venta VentaProducto, List<VentasDetalle> VentaProductoDetalles, int idUsuario, out string Message);
        bool CancelarVenta(int id, int idUsuario, out string Message);
    }

    public class VentasService : IVentasService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IVentaDetalleRepository _ventaDetalleRepository;
        private readonly ILogRepository _logRepository;
        

        public VentasService(IVentaRepository ventaRepository, IVentaDetalleRepository ventaDetalleRepository, ILogRepository logRepository) {
            _ventaRepository = ventaRepository;
            _ventaDetalleRepository = ventaDetalleRepository;
            _logRepository = logRepository;
        }

        public Venta GetVenta(int id) {
            return _ventaRepository.Get(id);
        }

        public List<VentasDetalle> GetVentaDetalles(int id)
        {
            Sql query = new Sql()
                .Select("*").From("VentasDetalle")
                .Where("ID_Venta = @0", id);
            return _ventaDetalleRepository.GetByFilter(query);
        }

        public List<Venta> GetVentas() {
            return _ventaRepository.GetAll("Ventas").ToList();
        }

        public List<dynamic> GetVentasFiltro(string from, string to, int? idcliente, int? idsucursal, int? idvendedor, int? idproducto, string folio = null, string folioalt = null)
        {
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(folio) || !string.IsNullOrEmpty(folioalt) || !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) || idcliente != null
                || idcliente != null || idsucursal != null || idvendedor != null || idproducto != null)
            {
                filter = " Where ";
            }

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                filter += string.Format("C.FechaFactura between '{0} 00:00:00' and '{1} 23:59:59' ", from, to);
            }
            if (idcliente != null)
            {
                filter += string.Format("{1} C.ID_Cliente = '{0}' ", idcliente, (filter.Length > 0 ? "and" : ""));
            }
            if (idsucursal != null)
            {
                filter += string.Format("{1} C.ID_Sucursal = '{0}' ", idsucursal, (filter.Length > 0 ? "and" : ""));
            }
            if (idvendedor != null)
            {
                filter += string.Format("{1} C.ID_Vendedor = '{0}' ", idvendedor, (filter.Length > 0 ? "and" : ""));
            }
            if (idproducto != null)
            {
                filter += string.Format("{1} D.ID_Producto = '{0}' ", idproducto, (filter.Length > 0 ? "and" : ""));
            }
            if (!string.IsNullOrEmpty(folio))
            {
                filter += string.Format("{1} C.ID like '%{0}%' ", folio, (filter.Length > 0 ? "and" : ""));
            }
            if (!string.IsNullOrEmpty(folioalt))
            {
                filter += string.Format("{1} C.Folio like '%{0}%' ", folioalt, (filter.Length > 0 ? "and" : ""));
            }
            Sql query = new Sql(@"select C.*, I.Nombre as Cliente, S.Nombre as Sucursal, U.Nombre as Vendedor from Ventas C 
                                    left join VentasDetalle D ON D.ID_Venta = C.ID
                                    inner join Clientes I on I.ID = C.ID_Cliente
                                    inner join Sucursales S on S.ID = C.ID_Sucursal
                                    inner join Usuarios U on U.ID = C.ID_Vendedor" + (filter.Length > 0 ? filter : ""));
            return _ventaRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateVenta(Venta VentaProducto, List<VentasDetalle> VentaProductoDetalles, int idUsuario,  out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {

                int id = _ventaRepository.InsertOrUpdate<int>(VentaProducto);
                VentaProducto.ID = id;
                List<VentasDetalle> _detallesActuales = GetVentaDetalles(id);

                // Eliminar detalles que no existen en los actuales
                foreach (var detalleNoExiste in _detallesActuales.Where(p => !VentaProductoDetalles.Any(p2 => p2.ID == p.ID)))
                {
                    _ventaDetalleRepository.Remove(detalleNoExiste);
                }

                //Insertar o Actualizar detalles existentes o nuevos
                foreach (var detalle in VentaProductoDetalles)
                {
                    if (detalle.ID <= 0)
                    {
                        detalle.ID_Venta = id;
                        _ventaDetalleRepository.InsertOrUpdate<int>(detalle);
                    }

                    _ventaDetalleRepository.InsertOrUpdate<int>(detalle);
                }

                Log log = new Log();
                log.Tipo = "Ventas";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                if (VentaProducto.ID <= 0)
                {
                    log.Accion = "Venta Nueva Guardada";
                }
                else
                {
                    log.Accion = VentaProducto.Estatus == "P" ? "Venta Procesada Actualizada" : "Venta Actualizada";
                }
                _logRepository.InsertOrUpdate<int>(log);

                Message = "Venta guardada con éxito";
                result = true;
            }
            catch (Exception ex)
            {
                Message = "Venta guardada No pudo ser guardada Error: " + ex.Message;
            }

            return result;
        }

        public bool CancelarVenta(int id, int idUsuario, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var venta = _ventaRepository.Get(id);
                List<VentasDetalle> detalles = GetVentaDetalles(id);

                // Eliminar detalles
                foreach (var detalle in detalles)
                {
                    _ventaDetalleRepository.Remove(detalle);
                }

                venta.Descuento = 0;
                venta.Subtotal = 0;
                venta.Iva = 0;
                venta.Total = 0;
                venta.DescTotal = 0;
                venta.Estatus = "C";

                _ventaRepository.InsertOrUpdate<int>(venta);

                Log log = new Log();
                log.Tipo = "Ventas";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                log.Accion = "Venta Cancelada";
                _logRepository.InsertOrUpdate<int>(log);

                Message = "Venta cancelada con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Venta No pudo ser cancelada Error: " + ex.Message;
            }
            return result;
        }
    }
}
