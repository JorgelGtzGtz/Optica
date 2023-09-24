﻿using dbconnection;
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
    [RoutePrefix("api/Salida")]
    public class SalidaController : BaseApiController
    {
        private readonly IProductosService _productoService;
        private readonly IListaCombosService _listaCombosService;
        private readonly IEntradaService _otrasEntradasSalidasService;
        private readonly IOtrasEntradasSalidasDetallesRepository _otrasEntradasSalidasDetallesRepository;
        private readonly IKardexService _kardexService;

        public SalidaController(IKardexService kardexService, IOtrasEntradasSalidasDetallesRepository otrasEntradasSalidasDetallesRepository, IListaCombosService listaCombosService, IProductosService productoService, IEntradaService otrasEntradasSalidasService)
        {
            _listaCombosService = listaCombosService;
            _productoService = productoService;
            _otrasEntradasSalidasDetallesRepository = otrasEntradasSalidasDetallesRepository;
            _otrasEntradasSalidasService = otrasEntradasSalidasService;
            _kardexService = kardexService;
        }

        [HttpGet]
        [Route("Lista/{otraentradasalida?}/")]
        public async Task<HttpResponseMessage> GetSucurles(HttpRequestMessage request, string otraentradasalida = null)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var item = _otrasEntradasSalidasService.GetOtraEntradaSalidaFiltro(2);

                    response = request.CreateResponse(HttpStatusCode.OK, item);
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

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<HttpResponseMessage> Eliminar(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var result = _otrasEntradasSalidasService.EliminarEntrada(id, out message);
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

        [Route("GetSalida/{id:int=0}/")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCompra(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var model = _otrasEntradasSalidasService.GetEntrada(id);
                    var detalles = _otrasEntradasSalidasService.GetCompraDetalles(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { model, detalles });
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
        [Route("Procesar/{id}")]
        public async Task<HttpResponseMessage> Procesar(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var result = _kardexService.ProcesarSalida(id, UserLogged.UserID, out message);
                    if (result)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { ID = result });
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


        [HttpPost]
        [Route("Guardar")]
        public async Task<HttpResponseMessage> Guardar(HttpRequestMessage request, [FromBody] JObject data)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var entrada = data["data"].ToObject<OtrasEntradasSalida>();
                    var detalles = data["detalles"].ToObject<List<OtrasEntradasSalidasDetalle>>();
                    entrada.ID_Usuario = UserLogged.UserID;
                    var result = _otrasEntradasSalidasService.InsertUpdateEntrada(entrada, detalles, out message);
                    if (result != 0)
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

        [Route("GetProducto/{id:int=0}/")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProducto(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var model = _productoService.GetProducto(id);
                    var kit = _productoService.GetProductosKit(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { producto = model, kit = kit });
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
                    var almacenes = _listaCombosService.GetAlmacenesDeSucursal(UserLogged.SucursalID);
                    var tipoEntradaSalida = _listaCombosService.GetTiposEntradaSalidas("Salida");
                    response = request.CreateResponse(HttpStatusCode.OK, new { productos, almacenes, tipoEntradaSalida});
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