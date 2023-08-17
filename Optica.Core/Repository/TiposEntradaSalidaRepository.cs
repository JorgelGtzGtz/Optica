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
    public interface ITiposEntradaSalidaRepository : IRepositoryBase<TiposEntradaSalida>
    {
    }

    public class TiposEntradaSalidaRepository : RepositoryBase<TiposEntradaSalida>, ITiposEntradaSalidaRepository
    {
        public TiposEntradaSalidaRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
