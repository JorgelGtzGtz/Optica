using dbconnection;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using Optica.Api.Models;
using Optica.Core.Entities.Dto;
using Optica.Core.Repository;
using Optica.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Optica.Api.Controllers
{
    [RoutePrefix("api/Pagos")]
    public class PagosController : BaseApiController
    {
        private readonly IListaCombosService _listaCombosService;
        private readonly IPagosService _pagosService;

        public PagosController(IPagosService PagosService, IListaCombosService listaCombosService)
        {
            _listaCombosService = listaCombosService;
            _pagosService = PagosService;
        }

        [Route("Combos")]
        public async Task<HttpResponseMessage> GetCombos(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var productos = _listaCombosService.GetProductos();
                    var sucursales = _listaCombosService.GetSucursales();
                    var clientes = _listaCombosService.GetClientes();
                    var metodospago = _listaCombosService.GetMetodosPago();
                    response = request.CreateResponse(HttpStatusCode.OK, new { productos, sucursales, clientes, metodospago});
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }


        [HttpPost]
        [Route("PagoContrato")]
        public async Task<HttpResponseMessage> PagoContrato(HttpRequestMessage request, [FromBody] JObject data)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var entrada = data["data"].ToObject<Pago>();
                    var detalles = data["detalles"].ToObject<List<pagare>>();
                    var result = _pagosService.InsertUpdatePago(entrada, detalles, out message);
                    if (result)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        new
                        {
                            error = "ERROR",
                            message = message
                        });
                    }

                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }

        [Route("GetPagos/{id:int=0}/{fecha?}")]
        public async Task<HttpResponseMessage> GetPagos(HttpRequestMessage request, int id, DateTime fecha)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var pagos = _pagosService.GetPagos(id, fecha);
                   
                    response = request.CreateResponse(HttpStatusCode.OK, pagos);
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }
        [HttpGet]
        [Route("AplicarImporte/{importe?}/{fecha?}/{id:int=0}")]
        public async Task<HttpResponseMessage> AplicarImporte(HttpRequestMessage request, decimal importe, DateTime fecha, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var pagos = _pagosService.AplicarImporte(importe, fecha, id);

                    response = request.CreateResponse(HttpStatusCode.OK, pagos);
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }


        [Route("GetContratos/{id:int=0}")]
        public async Task<HttpResponseMessage> GetContratos(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var contratos = _listaCombosService.GetContratosDeUsuario(id);

                    response = request.CreateResponse(HttpStatusCode.OK, contratos);
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }

        [Route("Guardar")]
        public async Task<HttpResponseMessage> Guardar(HttpRequestMessage request, [FromBody] JObject data)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var contrato = data["data"].ToObject<Contrato>();
                    var detalles = data["detalles"].ToObject<List<corridaOriginal>>();
                    //var result = _contratosService.InsertUpdateContrato(contrato, detalles, out message);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }
    }

}