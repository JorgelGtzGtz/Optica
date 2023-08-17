using Optica.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Optica.Api.Controllers
{
    [RoutePrefix("api/Combos")]
    public class ListaCombosController : BaseApiController
    {
        private readonly IListaCombosService _listaCombosService;

        public ListaCombosController(IListaCombosService listaCombosService)
        {
            _listaCombosService = listaCombosService;
        }

        [HttpGet]
        [Route("Sucursales")]
        public async Task<HttpResponseMessage> GetSucursales(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var item = _listaCombosService.GetSucursales();
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

        [HttpGet]
        [Route("TiposUsuarios")]
        public async Task<HttpResponseMessage> GetTiposUsuarios(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var item = _listaCombosService.GetTipoUsuarios();
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
    }
}