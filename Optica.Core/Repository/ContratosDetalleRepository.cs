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
    public interface IContratosDetalleRepository : IRepositoryBase<DetalleContrato>
    {
    }

    public class ContratosDetalleRepository : RepositoryBase<DetalleContrato>, IContratosDetalleRepository
    {
        public ContratosDetalleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
