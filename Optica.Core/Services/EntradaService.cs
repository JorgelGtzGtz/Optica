﻿using dbconnection;
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
        int InsertUpdateEntrada(OtrasEntradasSalida EntradaSalida, List<OtrasEntradasSalidasDetalle> detalles, out string Message);
        List<dynamic> GetOtraEntradaSalidaFiltro(string from, string to, int? movimiento, int? almacen = null, string status = null);
        bool EliminarEntrada(int id, out string Message);
        bool CancelarEntradaSalida(int id, int usuario, out string Message);
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

        public List<dynamic> GetOtraEntradaSalidaFiltro(string from, string to, int? movimiento, int? almacen, string status = null)
        {
            string filter = string.Empty;

            if (movimiento != null || almacen != null || !string.IsNullOrEmpty(status) || !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                filter = " where ";
            }

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                filter += string.Format("OES.Fecha between '{0} 00:00:00' and '{1} 23:59:59' ", from, to);
            }
            if (movimiento != null)
            {
                filter += string.Format("{1} OES.ID_TipoEntradaSalida = '{0}' ", movimiento, (filter.Length > 0 ? "and" : ""));
            }
            if (almacen != null)
            {
                filter += string.Format(" {1} OES.ID_Almacen = {0}", almacen, (filter.Length > 0 ? "aNd" : ""));
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status != "null")
                filter += string.Format(" and OES.Estatus like '{0}'", status.Substring(0, 1), (filter.Length > 0 ? "and" : ""));
            }
            Sql query = new Sql(@"select OES.*, tes.Descripcion as Movimiento from otrasentradassalidas OES
                                    inner join tiposentradasalida tes on OES.ID_TipoEntradaSalida = tes.ID " + (filter.Length > 0 ? filter : "")+
                                    "and OES.ID_EntradaSalida = 1");
                                    
            return _otrasEntradasSalidasRepository.GetByDynamicFilter(query);
        }

        public int InsertUpdateEntrada(OtrasEntradasSalida EntradaSalida, List<OtrasEntradasSalidasDetalle> detalles, out string Message) 
        {

            Message = string.Empty;
            int result = 0;
            try
            {
                result = _otrasEntradasSalidasRepository.InsertOrUpdate<int>(EntradaSalida);
                var _EntradaSalida = _otrasEntradasSalidasRepository.Get(result);
                decimal costo = 0, total = 0;
                foreach (var item in detalles)
                {
                    if (result != 0)
                    {
                        OtrasEntradasSalidasDetalle otrasEntradaDetalles = new OtrasEntradasSalidasDetalle();

                        otrasEntradaDetalles.Cantidad = item.Cantidad;
                        otrasEntradaDetalles.Costo = item.Costo;
                        otrasEntradaDetalles.CostoTotal = item.Cantidad * item.Costo;
                        otrasEntradaDetalles.ID_Producto = item.ID_Producto;
                        otrasEntradaDetalles.ID_OtraEntradasSalidas = result;
                        total += otrasEntradaDetalles.CostoTotal;
                        _otrasEntradasSalidasDetallesRepository.InsertOrUpdate<int>(otrasEntradaDetalles);
                    }
                }
                _EntradaSalida.Total = total;
                result = _otrasEntradasSalidasRepository.InsertOrUpdate<int>(_EntradaSalida);

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

        public bool CancelarEntradaSalida(int id, int idUsuario, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var entrada = _otrasEntradasSalidasRepository.Get(id);
                List<OtrasEntradasSalidasDetalle> detalles = GetEntradasDetalles(id);

                foreach (var detalle in detalles)
                {
                    _otrasEntradasSalidasDetallesRepository.Remove(detalle);
                }
               
                entrada.Estatus = "C";
                entrada.Total = 0;

                _otrasEntradasSalidasRepository.InsertOrUpdate<int>(entrada);

                Message = "Entrada cancelada con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Entrada No pudo ser cancelada Error: " + ex.Message;
            }
            return result;
        }

        public List<OtrasEntradasSalidasDetalle> GetEntradasDetalles(int id)
        {
            Sql query = new Sql(@"select * from otrasentradassalidasdetalles where ID_OtraEntradasSalidas = " + id);
            return _otrasEntradasSalidasDetallesRepository.GetByFilter(query);
        }

    }
}
