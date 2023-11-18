using dbconnection;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IListaCombosService
    {
        List<Sucursale> GetSucursales();
        List<Sucursale> GetSucursalesUsuario(int id);
        List<Zona> GetZonas();
        List<Usuario> GetUsuariosCobrador();
        List<Usuario> GetUsuariosCobradorPorSucursal(int id);
        List<Usuario> GetUsuariosVendedor();
        List<Usuario> GetUsuariosVendedorPorSucursal(int id);
        List<Usuario> GetUsuariosOptometrista();
        List<Cliente> GetClientes();
        List<Marca> GetMarcas();
        List<Modelo> GetModelos();
        List<Colore> GetColores();
        List<TiposLente> GetTiposLente();
        List<TiposUsuario> GetTipoUsuarios();
        List<Materiale> GetMateriale();
        List<ColoresLente> GetColoresLente();
        List<Producto> GetProductos();
        List<Producto> GetProductosKit();
        List<Proveedore> GetProveedores();
        List<Almacene> GetAlmacenes();
        List<MetodosPago> GetMetodosPago();
        List<Contrato> GetContratos();
        List<Almacene> GetAlmacenesDeSucursal(int id);
        List<Paciente> GetPacientes();
        List<TiposEntradaSalida> GetTiposEntradaSalidas(string tipo = "Entrada");
        List<Paciente> GetPacientesDeCliente(int id);
        List<Diagnostico> GetDiagnosticoDePaciente(int id);
    }

    public class ListaCombosService : IListaCombosService
    {
        private readonly ISucursalRepository _sucursalRepository;
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private readonly IZonasRepository _zonasRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMarcasRepository _marcasRepository;
        private readonly IModelosRepository _modelosRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IContratosRepository _contratosRepository;
        private readonly ITiposLenteRepository _tiposLenteRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;
        private readonly IMaterialeRepository _MaterialeRepository;
        private readonly IColorLenteRepository _colorLenteRepository;
        private readonly IProductosRepository _productosRepository;
        private readonly IProveedoresRepository _proveedoresRepository;
        private readonly IAlmacenRepository _almacenRepository;
        private readonly IPacientesRepository _pacientesRepository;
        private readonly IMetodosPagoRepository _metodosPagoRepository;
        private readonly ITiposEntradaSalidaRepository _tipoEntradaSalidaRepository;

        public ListaCombosService(IContratosRepository contratosRepository, IDiagnosticoRepository diagnosticoRepository, ITiposEntradaSalidaRepository tipoEntradaSalidaRepository, ISucursalRepository sucursalRepository, IZonasRepository zonasRepository, IUsuarioRepository usuarioRepository, 
            IClienteRepository clienteRepository, IMarcasRepository marcasRepository, IModelosRepository modelosRepository, IColorRepository colorRepository,
            ITiposLenteRepository tiposLenteRepository, ITipoUsuarioRepository tipoUsuarioRepository, IMaterialeRepository MaterialeRepository,
            IColorLenteRepository colorLenteRepository, IProductosRepository productosRepository, IProveedoresRepository proveedoresRepository,
            IAlmacenRepository almacenRepository, IPacientesRepository pacientesRepository, IMetodosPagoRepository metodosPagoRepository)
        {
            _sucursalRepository = sucursalRepository;
            _zonasRepository = zonasRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _marcasRepository = marcasRepository;
            _modelosRepository = modelosRepository;
            _colorRepository = colorRepository;
            _tiposLenteRepository = tiposLenteRepository;
            _tipoUsuarioRepository = tipoUsuarioRepository;
            _MaterialeRepository = MaterialeRepository;
            _colorLenteRepository = colorLenteRepository;
            _productosRepository = productosRepository;
            _proveedoresRepository = proveedoresRepository;
            _almacenRepository = almacenRepository;
            _pacientesRepository = pacientesRepository;
            _metodosPagoRepository = metodosPagoRepository;
            _tipoEntradaSalidaRepository = tipoEntradaSalidaRepository;
            _diagnosticoRepository = diagnosticoRepository;
            _contratosRepository = contratosRepository;
        }

        public List<Sucursale> GetSucursales()
        {
            Sql query = new Sql()
                .Select("*").From("Sucursales");
            return _sucursalRepository.GetByFilter(query);
        }

        public List<TiposEntradaSalida> GetTiposEntradaSalidas(string tipo = "Entrada")
        {
            Sql query = new Sql("SELECT * FROM [dbo].[tiposentradasalida] with(nolock) where [tipo] = @0", tipo);
            return _tipoEntradaSalidaRepository.GetByFilter(query);
        }

        public List<Sucursale> GetSucursalesUsuario(int id)
        {
            Sql query = new Sql(@"select * from [dbo].[Sucursales] 
                                  Where ID = @0 ", id);
            return _sucursalRepository.GetByFilter(query);
        }

        public List<Zona> GetZonas()
        {
            Sql query = new Sql()
                .Select("*").From("Zonas");
            return _zonasRepository.GetByFilter(query);
        }

        public List<Usuario> GetUsuariosCobrador()
        {
            Sql query = new Sql(@"select p.* from  [dbo].[Usuarios] p
                                  inner join [dbo].[TiposUsuario] pt on pt.ID = p.ID_TipoUsuario
                                  Where UPPER(pt.Nombre) = 'COBRADOR'");
            return _usuarioRepository.GetByFilter(query);
        }

        public List<Usuario> GetUsuariosVendedor()
        {
            Sql query = new Sql(@"select p.* from  [dbo].[Usuarios] p
                                  inner join [dbo].[TiposUsuario] pt on pt.ID = p.ID_TipoUsuario
                                  Where UPPER(pt.Nombre) = 'VENDEDOR'");
            return _usuarioRepository.GetByFilter(query);
        }

        public List<Usuario> GetUsuariosOptometrista()
        {
            Sql query = new Sql(@"select p.* from  [dbo].[Usuarios] p
                                  Where p.Optometrista = 1");
            return _usuarioRepository.GetByFilter(query);
        }

        public List<Usuario> GetUsuariosCobradorPorSucursal(int id)
        {
            Sql query = new Sql(@"select p.* from  [dbo].[Usuarios] p
                                  inner join [dbo].[TiposUsuario] pt on pt.ID = p.ID_TipoUsuario
                                  Where p.ID_Sucursal = @0 and UPPER(pt.Nombre) = 'COBRADOR'", id);
            return _usuarioRepository.GetByFilter(query);
        }

        public List<Usuario> GetUsuariosVendedorPorSucursal(int id)
        {
            Sql query = new Sql(@"select p.* from  [dbo].[Usuarios] p
                                  inner join [dbo].[TiposUsuario] pt on pt.ID = p.ID_TipoUsuario
                                  Where p.ID_Sucursal = @0 and UPPER(pt.Nombre) = 'VENDEDOR'", id);
            return _usuarioRepository.GetByFilter(query);
        }

        public List<Cliente> GetClientes()
        {
            Sql query = new Sql()
                .Select("*").From("Clientes");
            return _clienteRepository.GetByFilter(query);
        }

        public List<Marca> GetMarcas()
        {
            Sql query = new Sql()
                .Select("*").From("Marcas");
            return _marcasRepository.GetByFilter(query);
        }

        public List<Modelo> GetModelos()
        {
            Sql query = new Sql()
                .Select("*").From("Modelos");
            return _modelosRepository.GetByFilter(query);
        }

        public List<Colore> GetColores()
        {
            Sql query = new Sql()
                .Select("*").From("Colores");
            return _colorRepository.GetByFilter(query);
        }

        public List<TiposLente> GetTiposLente()
        {
            Sql query = new Sql()
                .Select("*").From("TiposLente");
            return _tiposLenteRepository.GetByFilter(query);
        }

        public List<TiposUsuario> GetTipoUsuarios()
        {
            Sql query = new Sql()
                .Select("*").From("TiposUsuario");
            return _tipoUsuarioRepository.GetByFilter(query);
        }

        public List<Materiale> GetMateriale()
        {
            Sql query = new Sql()
                .Select("*").From("Materiales");
            return _MaterialeRepository.GetByFilter(query);
        }

        public List<ColoresLente> GetColoresLente()
        {
            Sql query = new Sql()
                .Select("*").From("ColoresLente");
            return _colorLenteRepository.GetByFilter(query);
        }

        public List<Producto> GetProductos()
        {
            Sql query = new Sql()
                .Select("*").From("Productos");
            return _productosRepository.GetByFilter(query);
        }

        public List<Producto> GetProductosKit()
        {
            Sql query = new Sql()
                .Select("*").From("Productos Where Kit = 1");
            return _productosRepository.GetByFilter(query);
        }

        public List<Paciente> GetPacientesDeCliente(int id)
        {
            Sql query = new Sql(@"select * from [dbo].[Pacientes]
                                  Where ID_Cliente = @0 ", id);
            return _pacientesRepository.GetByFilter(query);
        }

        public List<Diagnostico> GetDiagnosticoDePaciente(int id)
        {
            Sql query = new Sql(@"select * from [dbo].[diagnosticos]
                                  Where ID_Paciente = @0 ", id);
            return _diagnosticoRepository.GetByFilter(query);
        }

        public List<Proveedore> GetProveedores()
        {
            Sql query = new Sql()
                .Select("*").From("Proveedores");
            return _proveedoresRepository.GetByFilter(query);
        }

        public List<Almacene> GetAlmacenes()
        {
            Sql query = new Sql()
                .Select("*").From("Almacenes");
            return _almacenRepository.GetByFilter(query);
        }

        public List<Almacene> GetAlmacenesDeSucursal(int id)
        {
            Sql query = new Sql(@"select * from [dbo].[Almacenes]
                                  Where ID_Sucursal = @0 ", id);
            return _almacenRepository.GetByFilter(query);
        }

        public List<Paciente> GetPacientes()
        {
            Sql query = new Sql()
                .Select("*").From("Pacientes");
            return _pacientesRepository.GetByFilter(query);
        }

        public List<Contrato> GetContratos()
        {
            Sql query = new Sql()
                .Select("*").From("Contratos").Where("Estatus = 1");
            return _contratosRepository.GetByFilter(query);
        }

        public List<MetodosPago> GetMetodosPago()
        {
            Sql query = new Sql()
                .Select("*").From("MetodosPago").Where("estatus = 'true'");
            return _metodosPagoRepository.GetByFilter(query);
        }
    }
}
