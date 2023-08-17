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
    public interface IProductoSeriesRepository : IRepositoryBase<ProductoSeries>
    {
    }

    public class ProductoSeriesRepository : RepositoryBase<ProductoSeries>, IProductoSeriesRepository
    {
        public ProductoSeriesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
