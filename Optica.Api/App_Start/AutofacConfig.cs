using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Optica.Core.Factories;
using Optica.Core.Repository;
using Optica.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Optica.Api.App_Start
{
    public class AutofacConfig
    {

        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<DbFactory>().As<IDbFactory>().AsImplementedInterfaces();
            builder.RegisterType<AccesosRepository>().As<IAccesosRepository>().AsImplementedInterfaces();
            builder.RegisterType<AccesosTipoUsuarioRepository>().As<IAccesosTipoUsuarioRepository>().AsImplementedInterfaces();
            builder.RegisterType<AlmacenRepository>().As<IAlmacenRepository>().AsImplementedInterfaces();
            builder.RegisterType<ClienteRepository>().As<IClienteRepository>().AsImplementedInterfaces();
            builder.RegisterType<ColorLenteRepository>().As<IColorLenteRepository>().AsImplementedInterfaces();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().AsImplementedInterfaces();
            builder.RegisterType<CompraDetalleRepository>().As<ICompraDetalleRepository>().AsImplementedInterfaces();
            builder.RegisterType<CompraRepository>().As<ICompraRepository>().AsImplementedInterfaces();
            builder.RegisterType<ContratosDetalleRepository>().As<IContratosDetalleRepository>().AsImplementedInterfaces();
            builder.RegisterType<ContratosRepository>().As<IContratosRepository>().AsImplementedInterfaces();
            builder.RegisterType<DestinoRepository>().As<IDestinoRepository>().AsImplementedInterfaces();
            builder.RegisterType<DiagnosticoRepository>().As<IDiagnosticoRepository>().AsImplementedInterfaces();
            builder.RegisterType<DocumentosCobranzaRepository>().As<IDocumentosCobranzaRepository>().AsImplementedInterfaces();
            builder.RegisterType<ExistenciasAlmacenRepository>().As<IExistenciasAlmacenRepository>().AsImplementedInterfaces();
            builder.RegisterType<GeneracionMicasRepository>().As<IGeneracionMicasRepository>().AsImplementedInterfaces();
            builder.RegisterType<HistorialVisitasRepository>().As<IHistorialVisitasRepository>().AsImplementedInterfaces();
            builder.RegisterType<KardexProductosRepository>().As<IKardexProductosRepository>().AsImplementedInterfaces();
            builder.RegisterType<KardexSeriesRepository>().As<IKardexSeriesRepository>().AsImplementedInterfaces();
            builder.RegisterType<MarcasRepository>().As<IMarcasRepository>().AsImplementedInterfaces();
            builder.RegisterType<MaterialeRepository>().As<IMaterialeRepository>().AsImplementedInterfaces();
            builder.RegisterType<ModelosRepository>().As<IModelosRepository>().AsImplementedInterfaces();
            builder.RegisterType<PacientesRepository>().As<IPacientesRepository>().AsImplementedInterfaces();
            builder.RegisterType<PagosRepository>().As<IPagosRepository>().AsImplementedInterfaces();
            builder.RegisterType<PagosDetalleRepository>().As<IPagosDetalleRepository>().AsImplementedInterfaces();
            builder.RegisterType<ProductoSeriesRepository>().As<IProductoSeriesRepository>().AsImplementedInterfaces();
            builder.RegisterType<ProductosKitRepository>().As<IProductosKitRepository>().AsImplementedInterfaces();
            builder.RegisterType<ProductosDetalleKitRepository>().As<IProductosDetalleKitRepository>().AsImplementedInterfaces();
            builder.RegisterType<ProductosRepository>().As<IProductosRepository>().AsImplementedInterfaces();
            builder.RegisterType<ProveedoresRepository>().As<IProveedoresRepository>().AsImplementedInterfaces();
            builder.RegisterType<ServiciosRepository>().As<IServiciosRepository>().AsImplementedInterfaces();
            builder.RegisterType<SucursalRepository>().As<ISucursalRepository>().AsImplementedInterfaces();
            builder.RegisterType<TiposEntradaSalidaRepository>().As<ITiposEntradaSalidaRepository>().AsImplementedInterfaces();
            builder.RegisterType<TiposLenteRepository>().As<ITiposLenteRepository>().AsImplementedInterfaces();
            builder.RegisterType<TipoUsuarioRepository>().As<ITipoUsuarioRepository>().AsImplementedInterfaces();
            builder.RegisterType<UsuarioRepository>().As<IUsuarioRepository>().AsImplementedInterfaces();
            builder.RegisterType<ZonasRepository>().As<IZonasRepository>().AsImplementedInterfaces();
            builder.RegisterType<LogRepository>().As<ILogRepository>().AsImplementedInterfaces();
            builder.RegisterType<OtrasEntradasSalidasRepository>().As<IOtrasEntradasSalidasRepository>().AsImplementedInterfaces();
            builder.RegisterType<OtrasEntradasSalidasDetallesRepository>().As<IOtrasEntradasSalidasDetallesRepository>().AsImplementedInterfaces();
            builder.RegisterType<MetodosPagoRepository>().As<IMetodosPagoRepository>().AsImplementedInterfaces();
            builder.RegisterType<VentaRepository>().As<IVentaRepository>().AsImplementedInterfaces();
            builder.RegisterType<VentaDetalleRepository>().As<IVentaDetalleRepository>().AsImplementedInterfaces();
            builder.RegisterType<ContratosRepository>().As<IContratosRepository>().AsImplementedInterfaces();
            builder.RegisterType<CorridaRepository>().As<ICorridaRepository>().AsImplementedInterfaces();
            builder.RegisterType<PagaresRepository>().As<IPagaresRepository>().AsImplementedInterfaces();

            builder.RegisterType<PagosService>().As<IPagosService>().AsImplementedInterfaces();
            builder.RegisterType<EntradaService>().As<IEntradaService>().AsImplementedInterfaces();
            builder.RegisterType<ContratoService>().As<IContratoService>().AsImplementedInterfaces();
            builder.RegisterType<AlmacenService>().As<IAlmacenService>().AsImplementedInterfaces();
            builder.RegisterType<ClienteService>().As<IClienteService>().AsImplementedInterfaces();
            builder.RegisterType<ColorLenteService>().As<IColorLenteService>().AsImplementedInterfaces();
            builder.RegisterType<ColorService>().As<IColorService>().AsImplementedInterfaces();
            builder.RegisterType<ComprasService>().As<IComprasService>().AsImplementedInterfaces();
            builder.RegisterType<ListaCombosService>().As<IListaCombosService>().AsImplementedInterfaces();
            builder.RegisterType<MarcasService>().As<IMarcasService>().AsImplementedInterfaces();
            builder.RegisterType<ModelosService>().As<IModelosService>().AsImplementedInterfaces();
            builder.RegisterType<PacienteService>().As<IPacienteService>().AsImplementedInterfaces();
            builder.RegisterType<ProductosService>().As<IProductosService>().AsImplementedInterfaces();
            builder.RegisterType<ProveedoresService>().As<IProveedoresService>().AsImplementedInterfaces();
            builder.RegisterType<SucursalService>().As<ISucursalService>().AsImplementedInterfaces();
            builder.RegisterType<TiposEntradaSalidaService>().As<ITiposEntradaSalidaService>().AsImplementedInterfaces();
            builder.RegisterType<TiposLenteService>().As<ITiposLenteService>().AsImplementedInterfaces();
            builder.RegisterType<TipoUsuarioService>().As<ITipoUsuarioService>().AsImplementedInterfaces();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>().AsImplementedInterfaces();
            builder.RegisterType<ZonasService>().As<IZonasService>().AsImplementedInterfaces();
            builder.RegisterType<MaterialeService>().As<IMaterialeService>().AsImplementedInterfaces();
            builder.RegisterType<KardexService>().As<IKardexService>().AsImplementedInterfaces();
            builder.RegisterType<DiagnosticosService>().As<IDiagnosticosService>().AsImplementedInterfaces();
            builder.RegisterType<MetodosPagoService>().As<IMetodosPagoService>().AsImplementedInterfaces();
            builder.RegisterType<VentasService>().As<IVentasService>().AsImplementedInterfaces();
            

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //.Where(t => t.Name.EndsWith("Repository"))
            //.AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //.Where(t => t.Name.EndsWith("Service"))
            //.AsImplementedInterfaces();

            Container = builder.Build();

            return Container;
        }
        //public static IContainer Container;

        //public static void Initialize()
        //{
        //    RegisterServices(new ContainerBuilder());
        //}

        //private static IContainer RegisterServices(ContainerBuilder builder)
        //{

        //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //    // Scan an assembly for components
        //    builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
        //    builder.RegisterAssemblyTypes(assemblies)
        //    .Where(t => t.Name.EndsWith("Repository"))
        //    .AsImplementedInterfaces();

        //    builder.RegisterAssemblyTypes(assemblies)
        //    .Where(t => t.Name.EndsWith("Service"))
        //    .AsImplementedInterfaces();

        //    //Set the dependency resolver to be Autofac.  
        //    Container = builder.Build();

        //    return Container;
        //}

        public static T Resolve<T>()
        {
            if (Container == null)
            {
                throw new Exception("AutofacConfig hasn't been Initialize!");
            }

            return Container.Resolve<T>(new Parameter[0]);
        }
    }
}