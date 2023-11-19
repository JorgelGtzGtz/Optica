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
    public interface IPagosRepository : IRepositoryBase<Pago>
    {
    }

    public class PagosRepository : RepositoryBase<Pago>, IPagosRepository
    {
        public PagosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
