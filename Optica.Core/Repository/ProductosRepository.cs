using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Repository
{
    public interface IProductosRepository : IRepositoryBase<Producto>
    {
    }

    public class ProductosRepository : RepositoryBase<Producto>, IProductosRepository
    {
        public ProductosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
