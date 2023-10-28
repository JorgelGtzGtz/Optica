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

        public PagosController(IListaCombosService listaCombosService, IPagosService pagosService)
        {
            _listaCombosService = listaCombosService;
            _pagosService = pagosService;
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