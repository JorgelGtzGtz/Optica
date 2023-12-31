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
    public interface IUsuarioService
    {
        Usuario GetUsuario(int id);
        Usuario GetUsuario(string usr);
        Usuario GetUsuario(string usr, string password);
        List<Usuario> GetUsuarios();
        List<dynamic> GetUsuarioesFiltro(string nombre = null);
        bool InsertUpdateUsuario(Usuario Usuario, out string Message);
        bool EliminarUsuario(int id, out string Message);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAccesosTipoUsuarioRepository _accesosTipoUsuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IAccesosTipoUsuarioRepository accesosTipoUsuarioRepository) {
            _usuarioRepository = usuarioRepository;
            _accesosTipoUsuarioRepository = accesosTipoUsuarioRepository;
        }

        public Usuario GetUsuario(int id) {
            return _usuarioRepository.Get(id);
        }

        public Usuario GetUsuario(string usr, string password)
        {
            return _usuarioRepository.GetUsuario(usr, password);
        }

        public Usuario GetUsuario(string usr)
        {
            return _usuarioRepository.GetUsuario(usr);
        }

        public List<Usuario> GetUsuarios() {
            return _usuarioRepository.GetAll("Usuarios").ToList();
        }

        public List<dynamic> GetUsuarioesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("p.Nombre like '%{0}%' or p.Usuario like '%{0}%'", nombre);
            }

            Sql query = new Sql(@"select p.*, pt.Nombre as NombreTipo, s.Nombre as NombreSucursal from  [dbo].[Usuarios] p
                                  inner join [dbo].[TiposUsuario] pt on pt.ID = p.ID_TipoUsuario
                                  inner join [dbo].[Sucursales] s on s.ID = p.ID_Sucursal " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _usuarioRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateUsuario(Usuario usuario, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                _usuarioRepository.InsertOrUpdate<int>(usuario);

                Message = "Usuario guardado " + usuario.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Usuario No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarUsuario(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var usuario = _usuarioRepository.Get(id);

                _usuarioRepository.Remove(usuario);

                Message = "Usuario eliminado " + usuario.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Usuario No pudo ser eliminado Error: " + ex.Message;
            }
            return result;
        }

        public List<AccesosTipoUsuario> GetPermisosUsuario(int id)
        {
            Sql query = new Sql()
                .Select("*").From("AccesosTipoUsuario")
                .Where("ID_TipoUsuario = @0", id);

            return _accesosTipoUsuarioRepository.GetByFilter(query);
        }
    }
}
