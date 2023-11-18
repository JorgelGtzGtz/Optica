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
    [RoutePrefix("api/Contrato")]
    public class ContratoController : BaseApiController
    {
        private readonly IListaCombosService _listaCombosService;
        private readonly IContratoService _contratosService;

        public ContratoController(IListaCombosService listaCombosService, IContratoService contratosService)
        {
            _listaCombosService = listaCombosService;
            _contratosService = contratosService;
        }


        [Route("GetLastContrato")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetLastContrato(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var result = _contratosService.GetLastContrato();
                    response = request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("Pacientes/{id}")]
        public async Task<HttpResponseMessage> GetCombos(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var pacientes = _listaCombosService.GetPacientesDeCliente(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { pacientes });
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

        [Route("Diagnosticos/{id}")]
        public async Task<HttpResponseMessage> GetDiagnosticos(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var diagnosticos = _listaCombosService.GetDiagnosticoDePaciente(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { diagnosticos });
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

        [Route("Producto/{id}")]
        public async Task<HttpResponseMessage> GetProductoSelected(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var producto = _contratosService.GetProductoSelected(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { producto });
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

        [Route("Paciente/{id}")]
        public async Task<HttpResponseMessage> GetPaciente(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var paciente = _contratosService.GetPacienteSelected(id);
                    response = request.CreateResponse(HttpStatusCode.OK, new { paciente });
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
                    var sucursales = _listaCombosService.GetSucursales();
                    var clientes = _listaCombosService.GetClientes();
                    response = request.CreateResponse(HttpStatusCode.OK, new { productos, sucursales, clientes });
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
                    var result = _contratosService.InsertUpdateContrato(contrato, detalles, out message);
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