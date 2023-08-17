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
    public interface IOtrasEntradasSalidasDetallesRepository : IRepositoryBase<OtrasEntradasSalidasDetalle>
    {
    }

    public class OtrasEntradasSalidasDetallesRepository : RepositoryBase<OtrasEntradasSalidasDetalle>, IOtrasEntradasSalidasDetallesRepository
    {
        public OtrasEntradasSalidasDetallesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
