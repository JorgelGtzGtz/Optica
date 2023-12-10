using dbconnection;
using Newtonsoft.Json.Linq;
using Optica.Core.Entities;
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
    [RoutePrefix("api/Inventario")]
    public class InventarioController : BaseApiController
    {
        private readonly IProductosService _productoService;
        private readonly IListaCombosService _listaCombosService;
        private readonly IKardexService _kardexService;

        public InventarioController(IProductosService productoService, IListaCombosService listaCombosService, IKardexService kardexService)
        {
            _productoService = productoService;
            _listaCombosService = listaCombosService;
            _kardexService = kardexService;

        }

        [HttpPost]
        [Route("Lista")]
        public async Task<HttpResponseMessage> ListaDetallesKit(HttpRequestMessage request, [FromBody] JObject data)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    int idproducto = data["id"].ToObject<int>();

                    var item = _productoService.GetProductosInventario(idproducto);

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
                    response = request.CreateResponse(HttpStatusCode.OK, new { producto = model});
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

        [Route("GetKardexProducto/{producto:int=0}/{almacen:int=0}")]
        public async Task<HttpResponseMessage> GetKardexProducto(HttpRequestMessage request, int producto, int almacen)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var movimientos = _kardexService.GetKardexProducto(producto, almacen);

                    response = request.CreateResponse(HttpStatusCode.OK, movimientos);
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
        [Route("Combos")]
        public async Task<HttpResponseMessage> GetListasCombos(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    
                    var productos = _listaCombosService.GetProductos();
                    var almacenes = _listaCombosService.GetAlmacenesDeSucursal(UserLogged.SucursalID);
                    response = request.CreateResponse(HttpStatusCode.OK, new { productos, almacenes });
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