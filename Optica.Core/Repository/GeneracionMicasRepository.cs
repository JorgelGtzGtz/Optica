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
    public interface IGeneracionMicasRepository : IRepositoryBase<GeneracionMica>
    {
    }

    public class GeneracionMicasRepository : RepositoryBase<GeneracionMica>, IGeneracionMicasRepository
    {
        public GeneracionMicasRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
