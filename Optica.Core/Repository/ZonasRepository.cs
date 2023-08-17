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
    public interface IZonasRepository : IRepositoryBase<Zona>
    {
    }

    public class ZonasRepository : RepositoryBase<Zona>, IZonasRepository
    {
        public ZonasRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
