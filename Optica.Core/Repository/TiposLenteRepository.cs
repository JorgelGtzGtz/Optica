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
    public interface ITiposLenteRepository : IRepositoryBase<TiposLente>
    {
    }

    public class TiposLenteRepository : RepositoryBase<TiposLente>, ITiposLenteRepository
    {
        public TiposLenteRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
