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
    public interface IMetodosPagoRepository : IRepositoryBase<MetodosPago>
    {
    }

    public class MetodosPagoRepository : RepositoryBase<MetodosPago>, IMetodosPagoRepository
    {
        public MetodosPagoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
