using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Factories;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Repository
{
    public interface IExistenciasAlmacenRepository : IRepositoryBase<ExistenciasAlmacen>
    {
        ExistenciasAlmacen GetExistenciaAlmacen(int productoid, int almacenid);
    }

    public class ExistenciasAlmacenRepository : RepositoryBase<ExistenciasAlmacen>, IExistenciasAlmacenRepository
    {
        public ExistenciasAlmacenRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public ExistenciasAlmacen GetExistenciaAlmacen(int productoid, int almacenid)
        {
            Sql query = new Sql(@"select * from ExistenciasAlmacen Where ID_Producto = @0 and ID_Almacen = @1", productoid, almacenid);

            return this.Context.SingleOrDefault<ExistenciasAlmacen>(query);
        }
    }
}
