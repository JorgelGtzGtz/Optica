




















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `dbconnection`
//     Provider:               `System.Data.SqlClient`
//     Connection String:      `Data Source=JORGE-PC;Initial Catalog=OpticaV2;Integrated Security=false;user ID=sa;password=**zapped**;Connection Timeout=0`
//     Schema:                 ``
//     Include Views:          `False`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace dbconnection
{

	public partial class dbconnectionDB : Database
	{
		public dbconnectionDB() 
			: base("dbconnection")
		{
			CommonConstruct();
		}

		public dbconnectionDB(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			dbconnectionDB GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static dbconnectionDB GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new dbconnectionDB();
        }

		[ThreadStatic] static dbconnectionDB _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

		public class Record<T> where T:new()
		{
			public static dbconnectionDB repo { get { return dbconnectionDB.GetInstance(); } }
			public bool IsNew() { return repo.IsNew(this); }
			public object Insert() { return repo.Insert(this); }

			public void Save() { repo.Save(this); }
			public int Update() { return repo.Update(this); }

			public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
			public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
			public static int Update(Sql sql) { return repo.Update<T>(sql); }
			public int Delete() { return repo.Delete(this); }
			public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
			public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
			public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
			public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
			public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
			public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
			public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
			public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
			public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
			public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
			public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
			public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
			public static T Single(Sql sql) { return repo.Single<T>(sql); }
			public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
			public static T First(Sql sql) { return repo.First<T>(sql); }
			public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
			public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
			public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
			public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
			public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
			public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
			public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
			public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
			public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
			public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

		}

	}
	



    

	[TableName("dbo.Accesos")]



	[PrimaryKey("ID", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class Acceso : dbconnectionDB.Record<Acceso>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.AccesosTipoUsuario")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class AccesosTipoUsuario : dbconnectionDB.Record<AccesosTipoUsuario>  
    {



		[Column] public int ID { get; set; }





		[Column] public int ID_Acceso { get; set; }





		[Column] public int ID_TipoUsuario { get; set; }



	}

    

	[TableName("dbo.Almacenes")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Almacene : dbconnectionDB.Record<Almacene>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public string Prefijo { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Sucursal { get; set; }



	}

    

	[TableName("dbo.Clientes")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Cliente : dbconnectionDB.Record<Cliente>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public DateTime FechaNacimiento { get; set; }





		[Column] public string Direccion { get; set; }





		[Column] public string NumeroInt { get; set; }





		[Column] public string NumeroExt { get; set; }





		[Column] public string Colonia { get; set; }





		[Column] public string Ciudad { get; set; }





		[Column] public string Estado { get; set; }





		[Column] public string CodigoPostal { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Telefono { get; set; }





		[Column] public string Celular { get; set; }





		[Column] public string eMail { get; set; }





		[Column] public string Rfc { get; set; }





		[Column] public string DireccionCredencialFrente { get; set; }





		[Column] public string DireccionCredencialAtras { get; set; }





		[Column] public string ImagenCredencialFrente { get; set; }





		[Column] public string ImagenCredencialAtras { get; set; }





		[Column] public string ImagenCliente { get; set; }





		[Column] public string ImagenCasa { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public DateTime FechaCreacion { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Sucursal { get; set; }





		[Column] public int ID_Zona { get; set; }





		[Column] public int ID_Usuario { get; set; }



	}

    

	[TableName("dbo.Colores")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Colore : dbconnectionDB.Record<Colore>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.ColoresLente")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ColoresLente : dbconnectionDB.Record<ColoresLente>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Compras")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Compra : dbconnectionDB.Record<Compra>  
    {



		[Column] public int ID { get; set; }





		[Column] public string ClaveFactura { get; set; }





		[Column] public DateTime FechaFactura { get; set; }





		[Column] public decimal Subtotal { get; set; }





		[Column] public decimal Iva { get; set; }





		[Column] public decimal Total { get; set; }





		[Column] public decimal Descuento { get; set; }





		[Column] public decimal PorcentajeDescuento { get; set; }





		[Column] public string Estatus { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public int ID_Almacen { get; set; }





		[Column] public int ID_Proveedor { get; set; }



	}

    

	[TableName("dbo.ComprasDetalles")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ComprasDetalle : dbconnectionDB.Record<ComprasDetalle>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public int Cantidad { get; set; }





		[Column] public decimal Costo { get; set; }





		[Column] public decimal CostoSinDescuento { get; set; }





		[Column] public decimal CostoConDescuento { get; set; }





		[Column] public decimal Total { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Compra { get; set; }



	}

    

	[TableName("dbo.Contratos")]



	[PrimaryKey("ID", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class Contrato : dbconnectionDB.Record<Contrato>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string ClaveContrato { get; set; }





		[Column] public DateTime? Fecha { get; set; }





		[Column] public int? Plazo { get; set; }





		[Column] public int? DiaCobro { get; set; }





		[Column] public string TipoCobro { get; set; }





		[Column] public decimal? Total { get; set; }





		[Column] public decimal? Restante { get; set; }





		[Column] public decimal? Abonos { get; set; }





		[Column] public decimal? Descuento { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public int ID_Cliente { get; set; }





		[Column] public int ID_UsuarioCobrador { get; set; }





		[Column] public int ID_UsuarioCreacion { get; set; }





		[Column] public int ID_Sucursal { get; set; }



	}

    

	[TableName("dbo.ContratosDetalle")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ContratosDetalle : dbconnectionDB.Record<ContratosDetalle>  
    {



		[Column] public int ID { get; set; }





		[Column] public string ClaveContrato { get; set; }





		[Column] public decimal? Costo { get; set; }





		[Column] public decimal? Precio { get; set; }





		[Column] public decimal? TotalAbono { get; set; }





		[Column] public decimal? Descuento { get; set; }





		[Column] public decimal? Restante { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Paciente { get; set; }





		[Column] public int ID_Diagnostico { get; set; }





		[Column] public int ID_Contrato { get; set; }



	}

    

	[TableName("dbo.Destinos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Destino : dbconnectionDB.Record<Destino>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Diagnosticos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Diagnostico : dbconnectionDB.Record<Diagnostico>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public string RxOD { get; set; }





		[Column] public string RxOI { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public int ID_TipoLente { get; set; }





		[Column] public int ID_Material { get; set; }





		[Column] public int ID_ColorLente { get; set; }





		[Column] public int ID_Paciente { get; set; }





		[Column] public int ID_Optometrista { get; set; }





		[Column] public int ID_Sucursal { get; set; }





		[Column] public string OD_esfera { get; set; }





		[Column] public string OI_esfera { get; set; }





		[Column] public string OD_cilindro { get; set; }





		[Column] public string IO_cilindro { get; set; }





		[Column] public string OD_eje { get; set; }





		[Column] public string OI_eje { get; set; }





		[Column] public string OD_adicion { get; set; }





		[Column] public string OI_adicion { get; set; }





		[Column] public string Dist_interpupilar { get; set; }





		[Column] public string Altura_oblea { get; set; }



	}

    

	[TableName("dbo.DocumentosCobranza")]



	[PrimaryKey("ID", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class DocumentosCobranza : dbconnectionDB.Record<DocumentosCobranza>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public DateTime? FechaPromesa { get; set; }





		[Column] public DateTime? FechaPago { get; set; }





		[Column] public decimal? Importe { get; set; }





		[Column] public decimal? Resta { get; set; }





		[Column] public string EstatusDoc { get; set; }



	}

    

	[TableName("dbo.ExistenciasAlmacen")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ExistenciasAlmacen : dbconnectionDB.Record<ExistenciasAlmacen>  
    {



		[Column] public int ID { get; set; }





		[Column] public int Disponible { get; set; }





		[Column] public int Cantidad { get; set; }





		[Column] public int ID_Almacen { get; set; }





		[Column] public int ID_Producto { get; set; }



	}

    

	[TableName("dbo.GeneracionMicas")]




	[ExplicitColumns]

    public partial class GeneracionMica : dbconnectionDB.Record<GeneracionMica>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public decimal Costo { get; set; }





		[Column] public decimal Precio { get; set; }





		[Column] public string NombreMica { get; set; }





		[Column] public int ID_TipoLente { get; set; }





		[Column] public int ID_Material { get; set; }





		[Column] public int ID_ColorLente { get; set; }





		[Column] public int ID_Paciente { get; set; }



	}

    

	[TableName("dbo.HistorialVisitas")]




	[ExplicitColumns]

    public partial class HistorialVisita : dbconnectionDB.Record<HistorialVisita>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public decimal Importe { get; set; }





		[Column] public string Atendio { get; set; }





		[Column] public string ResultadoVisita { get; set; }





		[Column] public int ID_Contrato { get; set; }





		[Column] public int ID_UsuarioCobrador { get; set; }





		[Column] public int ID_Sucursal { get; set; }



	}

    

	[TableName("dbo.KardexProductos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class KardexProducto : dbconnectionDB.Record<KardexProducto>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public int Cantidad { get; set; }





		[Column] public int CantidadTotal { get; set; }





		[Column] public int? CantidadDisponibleTotal { get; set; }





		[Column] public int CantidadTotalAlmacen { get; set; }





		[Column] public int? CantidadDisponibleAlmacen { get; set; }





		[Column] public decimal Costo { get; set; }





		[Column] public decimal CostoPromedio { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public int ID_EntradaSalida { get; set; }





		[Column] public int ID_TipoEntradaSalida { get; set; }





		[Column] public int ID_Movimiento { get; set; }





		[Column] public int ID_Almacen { get; set; }





		[Column] public int ID_Producto { get; set; }



	}

    

	[TableName("dbo.KardexSeries")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class KardexSeries : dbconnectionDB.Record<KardexSeries>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Folio { get; set; }





		[Column] public string Serie { get; set; }





		[Column] public string SerieTipo { get; set; }





		[Column] public int ID_Destino { get; set; }





		[Column] public int ID_DestinoParticular { get; set; }





		[Column] public int ID_EntradaSalida { get; set; }





		[Column] public int ID_TipoEntradaSalida { get; set; }





		[Column] public int ID_Sucursal { get; set; }



	}

    

	[TableName("dbo.Log")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Log : dbconnectionDB.Record<Log>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Tipo { get; set; }





		[Column] public string Accion { get; set; }





		[Column] public DateTime? Fecha { get; set; }





		[Column] public int? ID_Referencia { get; set; }





		[Column] public int? ID_Usuario { get; set; }



	}

    

	[TableName("dbo.Marcas")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Marca : dbconnectionDB.Record<Marca>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Materiales")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Materiale : dbconnectionDB.Record<Materiale>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public string Tipo { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.MetodosPago")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class MetodosPago : dbconnectionDB.Record<MetodosPago>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Modelos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Modelo : dbconnectionDB.Record<Modelo>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Marca { get; set; }



	}

    

	[TableName("dbo.OtrasEntradasSalidas")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class OtrasEntradasSalida : dbconnectionDB.Record<OtrasEntradasSalida>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public decimal? Total { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public string Estatus { get; set; }





		[Column] public int ID_TipoEntradaSalida { get; set; }





		[Column] public int ID_EntradaSalida { get; set; }





		[Column] public int ID_Almacen { get; set; }





		[Column] public int ID_Usuario { get; set; }



	}

    

	[TableName("dbo.OtrasEntradasSalidasDetalles")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class OtrasEntradasSalidasDetalle : dbconnectionDB.Record<OtrasEntradasSalidasDetalle>  
    {



		[Column] public int ID { get; set; }





		[Column] public int Cantidad { get; set; }





		[Column] public decimal Costo { get; set; }





		[Column] public decimal CostoTotal { get; set; }





		[Column] public int ID_OtraEntradasSalidas { get; set; }





		[Column] public int ID_Producto { get; set; }



	}

    

	[TableName("dbo.Pacientes")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Paciente : dbconnectionDB.Record<Paciente>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public DateTime FechaNacimiento { get; set; }





		[Column] public string Sexo { get; set; }





		[Column] public string Direccion { get; set; }





		[Column] public string NumeroInt { get; set; }





		[Column] public string NumeroExt { get; set; }





		[Column] public string Colonia { get; set; }





		[Column] public string Ciudad { get; set; }





		[Column] public string Estado { get; set; }





		[Column] public string CodigoPostal { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Telefono { get; set; }





		[Column] public string Celular { get; set; }





		[Column] public string eMail { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public DateTime FechaCreacion { get; set; }





		[Column] public bool DireccionCliente { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Cliente { get; set; }



	}

    

	[TableName("dbo.Pagos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Pago : dbconnectionDB.Record<Pago>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public decimal Importe { get; set; }





		[Column] public string Lugar { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public bool Estatus { get; set; }





		[Column] public int ID_UsuarioCobrador { get; set; }





		[Column] public int ID_Contrato { get; set; }



	}

    

	[TableName("dbo.PagosDetalle")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class PagosDetalle : dbconnectionDB.Record<PagosDetalle>  
    {



		[Column] public int ID { get; set; }





		[Column] public decimal Importe { get; set; }





		[Column] public int ID_Contrato { get; set; }





		[Column] public int ID_Pago { get; set; }



	}

    

	[TableName("dbo.Productos")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Producto : dbconnectionDB.Record<Producto>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public int? Cantidad { get; set; }





		[Column] public decimal? Costo { get; set; }





		[Column] public decimal? Precio { get; set; }





		[Column] public decimal? PrecioPP { get; set; }





		[Column] public decimal? PrecioCred { get; set; }





		[Column] public decimal? Iva { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public bool? Serie { get; set; }





		[Column] public bool? Inventariable { get; set; }





		[Column] public string Tipo { get; set; }





		[Column] public bool? Kit { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int? ID_Marca { get; set; }





		[Column] public int? ID_Modelo { get; set; }





		[Column] public int? ID_Color { get; set; }





		[Column] public int? ID_TipoLente { get; set; }





		[Column] public int? ID_MaterialLente { get; set; }





		[Column] public int? ID_ColorLente { get; set; }





		[Column] public string MicaOjo { get; set; }





		[Column] public string MicaEsfera { get; set; }





		[Column] public string MicaAdicion { get; set; }





		[Column] public string Imagen { get; set; }





		[Column] public int? Disponible { get; set; }





		[Column] public decimal? UltimoCosto { get; set; }





		[Column] public string MicaCilindro { get; set; }



	}

    

	[TableName("dbo.ProductosDetalleKit")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ProductosDetalleKit : dbconnectionDB.Record<ProductosDetalleKit>  
    {



		[Column] public int ID { get; set; }





		[Column] public int? Cantidad { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Producto_Base { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public string Clave { get; set; }



	}

    

	[TableName("dbo.ProductoSeries")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class ProductoSeries : dbconnectionDB.Record<ProductoSeries>  
    {



		[Column] public int ID { get; set; }





		[Column] public string SerieProducto { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Almacen { get; set; }



	}

    

	[TableName("dbo.ProductosKit")]




	[ExplicitColumns]

    public partial class ProductosKit : dbconnectionDB.Record<ProductosKit>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Serie { get; set; }





		[Column] public int? Cantidad { get; set; }





		[Column] public decimal? Costo { get; set; }





		[Column] public decimal? Precio { get; set; }





		[Column] public int ID_Producto { get; set; }



	}

    

	[TableName("dbo.Proveedores")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Proveedore : dbconnectionDB.Record<Proveedore>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string NombreComercial { get; set; }





		[Column] public string RazonSocial { get; set; }





		[Column] public string Contacto { get; set; }





		[Column] public string Direccion { get; set; }





		[Column] public string NumeroInt { get; set; }





		[Column] public string NumeroExt { get; set; }





		[Column] public string Colonia { get; set; }





		[Column] public string Ciudad { get; set; }





		[Column] public string Estado { get; set; }





		[Column] public string CodigoPostal { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Telefono { get; set; }





		[Column] public string Celular { get; set; }





		[Column] public string eMail { get; set; }





		[Column] public string Rfc { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public DateTime FechaCreacion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Servicios")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Servicio : dbconnectionDB.Record<Servicio>  
    {



		[Column] public int ID { get; set; }





		[Column] public DateTime? FechaInicio { get; set; }





		[Column] public DateTime? FechaRecoleccion { get; set; }





		[Column] public DateTime? FechaPromesa { get; set; }





		[Column] public DateTime? FechaFinalizacion { get; set; }





		[Column] public bool? Tipo { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public string DescProd { get; set; }





		[Column] public string ObservacionesCliente { get; set; }





		[Column] public string ObservacionesResultado { get; set; }





		[Column] public decimal? Costo { get; set; }





		[Column] public int ID_Cliente { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Contrato { get; set; }





		[Column] public int ID_Sucursal { get; set; }



	}

    

	[TableName("dbo.Sucursales")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Sucursale : dbconnectionDB.Record<Sucursale>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public string Direccion { get; set; }





		[Column] public string NumeroInt { get; set; }





		[Column] public string NumeroExt { get; set; }





		[Column] public string Colonia { get; set; }





		[Column] public string Ciudad { get; set; }





		[Column] public string Estado { get; set; }





		[Column] public string CodigoPostal { get; set; }





		[Column] public string Rfc { get; set; }





		[Column] public string Telefono { get; set; }





		[Column] public string Fax { get; set; }





		[Column] public string Abreviacion { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int? ID_Vendedor { get; set; }



	}

    

	[TableName("dbo.TiposEntradaSalida")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class TiposEntradaSalida : dbconnectionDB.Record<TiposEntradaSalida>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Tipo { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.TiposLente")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class TiposLente : dbconnectionDB.Record<TiposLente>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.TiposUsuario")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class TiposUsuario : dbconnectionDB.Record<TiposUsuario>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Nombre { get; set; }





		[Column] public bool? Estatus { get; set; }



	}

    

	[TableName("dbo.Usuarios")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Usuario : dbconnectionDB.Record<Usuario>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Nombre { get; set; }





		[Column("Usuario")] public string _Usuario { get; set; }





		[Column] public string Contrasena { get; set; }





		[Column] public string Direccion { get; set; }





		[Column] public string NumeroInt { get; set; }





		[Column] public string NumeroExt { get; set; }





		[Column] public string Colonia { get; set; }





		[Column] public string Ciudad { get; set; }





		[Column] public string Estado { get; set; }





		[Column] public string CodigoPostal { get; set; }





		[Column] public string Referencia { get; set; }





		[Column] public string Telefono { get; set; }





		[Column] public string Celular { get; set; }





		[Column] public string eMail { get; set; }





		[Column] public bool SuperAdmin { get; set; }





		[Column] public bool? Estatus { get; set; }





		[Column] public int ID_TipoUsuario { get; set; }





		[Column] public int ID_Sucursal { get; set; }





		[Column] public bool? Optometrista { get; set; }



	}

    

	[TableName("dbo.Ventas")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Venta : dbconnectionDB.Record<Venta>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Folio { get; set; }





		[Column] public DateTime Fecha { get; set; }





		[Column] public DateTime FechaCreacion { get; set; }





		[Column] public decimal? Descuento { get; set; }





		[Column] public decimal? Subtotal { get; set; }





		[Column] public decimal? Iva { get; set; }





		[Column] public decimal? Total { get; set; }





		[Column] public decimal? DescTotal { get; set; }





		[Column] public string Estatus { get; set; }





		[Column] public string Observaciones { get; set; }





		[Column] public int ID_Cliente { get; set; }





		[Column] public int ID_Sucursal { get; set; }





		[Column] public int ID_Almacen { get; set; }





		[Column] public int ID_Vendedor { get; set; }





		[Column] public int ID_MetodoPago { get; set; }



	}

    

	[TableName("dbo.VentasDetalle")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class VentasDetalle : dbconnectionDB.Record<VentasDetalle>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Clave { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public int? Cantidad { get; set; }





		[Column] public decimal? Costo { get; set; }





		[Column] public decimal? CostoTotal { get; set; }





		[Column] public decimal? Descuento { get; set; }





		[Column] public decimal? DescTotal { get; set; }





		[Column] public decimal? Precio { get; set; }





		[Column] public decimal? PrecioTotal { get; set; }





		[Column] public int ID_Producto { get; set; }





		[Column] public int ID_Venta { get; set; }



	}

    

	[TableName("dbo.Zonas")]



	[PrimaryKey("ID")]




	[ExplicitColumns]

    public partial class Zona : dbconnectionDB.Record<Zona>  
    {



		[Column] public int ID { get; set; }





		[Column] public string Descripcion { get; set; }





		[Column] public bool? Estatus { get; set; }



	}


}
