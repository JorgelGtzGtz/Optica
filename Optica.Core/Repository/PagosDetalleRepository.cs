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
    public interface IPagosDetalleRepository : IRepositoryBase<PagosDetalle>
    {
    }

    public class PagosDetalleRepository : RepositoryBase<PagosDetalle>, IPagosDetalleRepository
    {
        public PagosDetalleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
