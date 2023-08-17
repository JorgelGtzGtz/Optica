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
    public interface IContratosDetalleRepository : IRepositoryBase<ContratosDetalle>
    {
    }

    public class ContratosDetalleRepository : RepositoryBase<ContratosDetalle>, IContratosDetalleRepository
    {
        public ContratosDetalleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
