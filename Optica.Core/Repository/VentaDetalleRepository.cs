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
    public interface IVentaDetalleRepository : IRepositoryBase<VentasDetalle>
    {
    }

    public class VentaDetalleRepository : RepositoryBase<VentasDetalle>, IVentaDetalleRepository
    {
        public VentaDetalleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
