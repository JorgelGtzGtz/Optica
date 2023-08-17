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
    public interface IMaterialeRepository : IRepositoryBase<Materiale>
    {
    }

    public class MaterialeRepository : RepositoryBase<Materiale>, IMaterialeRepository
    {
        public MaterialeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
