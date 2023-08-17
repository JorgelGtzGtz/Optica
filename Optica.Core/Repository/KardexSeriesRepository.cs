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
    public interface IKardexSeriesRepository : IRepositoryBase<KardexSeries>
    {
    }

    public class KardexSeriesRepository : RepositoryBase<KardexSeries>, IKardexSeriesRepository
    {
        public KardexSeriesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
