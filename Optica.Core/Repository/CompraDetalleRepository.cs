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
    public interface ICompraDetalleRepository : IRepositoryBase<ComprasDetalle>
    {
    }

    public class CompraDetalleRepository : RepositoryBase<ComprasDetalle>, ICompraDetalleRepository
    {
        public CompraDetalleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
