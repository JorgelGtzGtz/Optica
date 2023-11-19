using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Optica.Core.Services
{
    public interface IPagosService
    {
        public List<pagare> GetPagos(int idContrato, DateTime fecha);
        public decimal? GetImporteSugerido(List<pagare> pagares, DateTime fecha);
        public List<pagare> AplicarImporte(decimal importe, DateTime fecha, int idContrato);
        public bool InsertUpdatePago(Pago Pago, List<pagare> Pagares, out string Message);
    }

    public class PagosService : IPagosService
    {
        private readonly IPagaresRepository _pagaresRepository;
        private readonly IContratosRepository _ContratosRepository;
        private readonly ISucursalRepository _SucursalesRepository;
        private readonly IPagosRepository _PagosRepository;
        private readonly IPagosDetalleRepository _PagosDetalleRepository;


        public PagosService(IPagosDetalleRepository PagosDetalleRepository, IPagosRepository PagosRepository, IPagaresRepository pagaresRepository, ISucursalRepository SucursalesRepository, IContratosRepository contratosRepository)
        {
            _pagaresRepository = pagaresRepository;
            _ContratosRepository = contratosRepository;
            _SucursalesRepository = SucursalesRepository;
            _PagosRepository = PagosRepository;
            _PagosDetalleRepository = PagosDetalleRepository;
        }

        public List<pagare> GetPagos(int idContrato, DateTime fecha)
        {
            List<pagare> reult = new List<pagare>();
            Contrato contrato = _ContratosRepository.Get(idContrato);
            Sucursale sucursal = _SucursalesRepository.Get(contrato.ID_Sucursal);
            Sql query = new Sql(@"select * from pagares where claveContrato = @0 and status = 1;", idContrato);
            List<pagare> pagares = _pagaresRepository.GetPagares(query);
            foreach (var pagare in pagares)
            {
                if (pagare.fecha <= fecha)
                {
                    if (pagare.fecha == fecha)
                    {
                        reult.Add(pagare);

                    }
                    else
                    {
                        pagare.gastoCobranza = sucursal.ImpCobranza;
                        pagare.importeTotal += pagare.gastoCobranza;
                        pagare.resta += pagare.gastoCobranza;
                        reult.Add(pagare);
                    }
                }
            }
            return reult;
        }

        public decimal? GetImporteSugerido(List<pagare> pagares, DateTime fecha)
        {
            decimal? result = 0;
            foreach (var pagare in pagares)
            {
                if (pagare.fecha < fecha)
                {
                    result += pagare.resta + pagare.gastoCobranza;
                }else if (pagare.fecha >= fecha)
                {
                    result += pagare.resta;
                }
            }
            return result;
        }

        public List<pagare> AplicarImporte(decimal importe, DateTime fecha, int idContrato)
        {
            List<pagare> result = new List<pagare>();
            Contrato contrato = _ContratosRepository.Get(idContrato);
            Sucursale sucursal = _SucursalesRepository.Get(contrato.ID_Sucursal);
            Sql query = new Sql(@"select * from pagares where claveContrato = @0;", idContrato);
            List<pagare> pagares = _pagaresRepository.GetPagares(query);
            decimal impTemporal = importe;
            decimal nuevoImporte = 0;
            decimal impDetalle = 0;
            decimal impPagare = 0;
            decimal impGasto = 0;
            foreach (var pagare in pagares)
            {
                if (impTemporal > 0)
                {
                    if (pagare.Status == 1)
                    {
                        nuevoImporte = pagare.resta;
                        //Si se paga la fecha o se adelanta una fecha
                        if (pagare.fecha >= fecha)
                        {
                            if (impTemporal <= nuevoImporte)
                            {
                                pagare.resta = pagare.resta - impTemporal;
                                impTemporal -= impTemporal;
                            }
                            else
                            {
                                pagare.resta = pagare.resta - nuevoImporte;
                                impTemporal -= nuevoImporte;
                            }
                            result.Add(pagare);
                        }
                        //Si la fecha es atrasada
                        else
                        {
                            pagare.gastoCobranza = sucursal.ImpCobranza;
                            pagare.importeTotal = pagare.importeOriginal + sucursal.ImpCobranza;
                            pagare.resta += sucursal.ImpCobranza;
                            nuevoImporte = pagare.resta;
                            if (impTemporal <= pagare.resta)
                            {
                                pagare.resta = pagare.resta - impTemporal;
                                impTemporal -= impTemporal;
                            }
                            else
                            {
                                pagare.resta -= pagare.resta;
                                impTemporal -= nuevoImporte;
                            }
                            result.Add(pagare);

                        }
                    }
                }
            }

            return result;
        }

        public bool InsertUpdatePago(Pago Pago, List<pagare> Pxagares, out string Message)
        {
            try
            {

                Contrato contrato = _ContratosRepository.Get(Pago.ID_Contrato);
                Sucursale sucursal = _SucursalesRepository.Get(contrato.ID_Sucursal);
                Sql query = new Sql(@"select * from pagares where claveContrato = @0 and status = 1;", contrato.ID);
                List<pagare> Pagares = _pagaresRepository.GetPagares(query);
                Pago.ID_Sucursal = contrato.ID_Sucursal;
                Pago.ID = _PagosRepository.InsertOrUpdate<int>(Pago);
                decimal impTemporal = Pago.Importe;
                decimal nuevoImporte = 0;
                decimal impDetalle = 0;
                decimal impPagare = 0;
                decimal impGasto = 0;

                foreach (var pagare in Pagares)
                {
                    if (impTemporal > 0)
                    {
                        if (pagare.Status == 1)
                        {
                            nuevoImporte = pagare.resta;
                            //Si se paga la fecha o se adelanta una fecha
                            if (pagare.fecha >= Pago.Fecha)
                            {
                                if (impTemporal <= nuevoImporte)
                                {
                                    impDetalle = impTemporal;
                                }
                                else
                                {
                                    impDetalle = nuevoImporte;
                                    impTemporal -= nuevoImporte;
                                }
                                impPagare = impDetalle;
                                impGasto = 0;
                                contrato.Abonos = contrato.Abonos + impDetalle;
                                contrato.Restante = contrato.Restante - impDetalle;
                                pagare.importePagado = pagare.importePagado + impDetalle;
                                pagare.resta = pagare.resta - impDetalle;
                                pagare.fechaUltimoPago = Pago.Fecha;
                                if (pagare.resta < 1)
                                {
                                    pagare.Status = 2;
                                }
                                PagosDetalle detalle = new PagosDetalle();
                                detalle.ID_Pago = Pago.ID;
                                detalle.ID_Contrato = pagare.claveContrato;
                                detalle.ID_Pagare = pagare.ID;
                                detalle.Importe = impDetalle;
                                detalle.importePagare = impPagare;
                                detalle.importeGasto = impGasto;
                                detalle.Status = true;
                                _PagosDetalleRepository.InsertOrUpdate<int>(detalle);
                            }
                            //Si la fecha es atrasada
                            else
                            {
                                if (pagare.gastoCobranza == 0)
                                {
                                    nuevoImporte = pagare.resta + sucursal.ImpCobranza;
                                    pagare.gastoCobranza = sucursal.ImpCobranza;
                                    pagare.importeTotal = pagare.importeOriginal + pagare.gastoCobranza;
                                    contrato.Restante += pagare.gastoCobranza;
                                }
                                else
                                {
                                    nuevoImporte = pagare.resta;
                                }
                                //Si el importe del pago es igual o menor al importe del pagare
                                if (impTemporal <= nuevoImporte)
                                {
                                    impDetalle = impTemporal;
                                    pagare.importePagado += impDetalle;
                                    pagare.resta -= impDetalle;
                                    if (pagare.importePagado >= pagare.importeOriginal)
                                    {
                                        impGasto = impDetalle;
                                    }
                                    else
                                    {
                                        if ((impTemporal + pagare.importePagado) <= pagare.importeOriginal)
                                        {
                                            impGasto = 0;
                                            impPagare = impDetalle;
                                        }
                                        else
                                        {
                                            impPagare = pagare.importeOriginal;
                                            impGasto = impTemporal + pagare.importePagado - impPagare;
                                        }
                                    }
                                }
                                else
                                {
                                    impDetalle = nuevoImporte;
                                    if (nuevoImporte <= pagare.gastoCobranza)
                                    {
                                        impGasto = nuevoImporte;
                                    }
                                    else
                                    {
                                        impPagare = pagare.importeOriginal + pagare.gastoCobranza;
                                        impGasto = pagare.gastoCobranza;
                                    }
                                    pagare.resta -= impDetalle;
                                    impTemporal -= nuevoImporte;
                                }
                                contrato.Abonos += impDetalle;
                                contrato.Restante -= impDetalle;
                                pagare.fechaUltimoPago = Pago.Fecha;
                                if (pagare.resta < 1)
                                {
                                    pagare.Status = 2;
                                }
                                PagosDetalle detalle = new PagosDetalle();
                                detalle.ID_Pago = Pago.ID;
                                detalle.ID_Contrato = pagare.claveContrato;
                                detalle.ID_Pagare = pagare.ID;
                                detalle.Importe = impDetalle;
                                detalle.importePagare = impPagare;
                                detalle.importeGasto = impGasto;
                                detalle.Status = true;
                                _PagosDetalleRepository.InsertOrUpdate<int>(detalle);
                            }
                        }
                    }
                    _pagaresRepository.InsertOrUpdate<int>(pagare);
                    _ContratosRepository.InsertOrUpdate<int>(contrato);
                }


                Message = "Pago guardado con exito";
            }
            catch (Exception ex)
            {
                Message = "Pago No pudo ser guardado Error: " + ex.Message;
            }

            return true;
        }


    }
}
