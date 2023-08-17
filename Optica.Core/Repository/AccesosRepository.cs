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
    public interface IAccesosRepository : IRepositoryBase<Acceso>
    {
    }

    public class AccesosRepository : RepositoryBase<Acceso>, IAccesosRepository
    {
        public AccesosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
