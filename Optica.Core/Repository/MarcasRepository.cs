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
    public interface IMarcasRepository : IRepositoryBase<Marca>
    {
    }

    public class MarcasRepository : RepositoryBase<Marca>, IMarcasRepository
    {
        public MarcasRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
