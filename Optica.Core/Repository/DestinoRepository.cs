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
    public interface IDestinoRepository : IRepositoryBase<Destino>
    {
    }

    public class DestinoRepository : RepositoryBase<Destino>, IDestinoRepository
    {
        public DestinoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
