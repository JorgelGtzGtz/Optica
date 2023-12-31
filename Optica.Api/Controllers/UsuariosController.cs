﻿using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Entities.Dto;
using Optica.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Optica.Api.Controllers
{
    [RoutePrefix("api/Usuario")]
    public class UsuariosController : BaseApiController
    {
        private readonly IUsuarioService _usuarioservice;
        private readonly IListaCombosService _listaCombosService;
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public UsuariosController(IUsuarioService usuarioservice, IListaCombosService listaCombosService, ITipoUsuarioService tipoUsuarioService)
        {
            _usuarioservice = usuarioservice;
            _listaCombosService = listaCombosService;
            _tipoUsuarioService = tipoUsuarioService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> Authenticate(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    if (UserLogged != null)
                    {
                        var op = _usuarioservice.GetUsuario(UserLogged.UserName, UserLogged.Password);
                        var accesos = _tipoUsuarioService.GetTipoUsuarioAccesos(op.ID_TipoUsuario);
                        User = Thread.CurrentPrincipal;
                        response = request.CreateResponse(HttpStatusCode.OK, new { usuario = op, accesos = accesos });
                    }
                    else
                    {
                        message = "Usuario o contraseña invalidas, Intente de nuevo.";
                        response = request.CreateResponse(HttpStatusCode.NotFound, new { Status = "ERROR", message = message, Sucess = false });
                    }

                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        Status = "ERROR",
                        Message = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }

        [HttpGet]
        [Route("Lista/{usuario?}/")]
        public async Task<HttpResponseMessage> GetUsuarios(HttpRequestMessage request, string usuario = null)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var item = _usuarioservice.GetUsuarioesFiltro(usuario);
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

        [Route("GetUsuario/{id:int=0}/")]
        [HttpGet]
        public async Task<HttpResponseMessage> getUsuario(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var usuario = _usuarioservice.GetUsuario(id);

                    response = request.CreateResponse(HttpStatusCode.OK, usuario);
                }
                catch (Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                    new
                    {
                        error = "ERROR",
                        exception = ex.Message
                    });
                }

                return await Task.FromResult(response);
            });
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<HttpResponseMessage> Guardar(HttpRequestMessage request, Usuario model)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string message = String.Empty;
                try
                {
                    var result = _usuarioservice.InsertUpdateUsuario(model, out message);
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
                    var result = _usuarioservice.EliminarUsuario(id, out message);
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