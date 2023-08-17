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
    public interface IProductosDetalleKitRepository : IRepositoryBase<ProductosDetalleKit>
    {
    }

    public class ProductosDetalleKitRepository : RepositoryBase<ProductosDetalleKit>, IProductosDetalleKitRepository
    {
        public ProductosDetalleKitRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
