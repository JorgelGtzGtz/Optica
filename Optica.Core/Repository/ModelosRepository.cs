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
    public interface IModelosRepository : IRepositoryBase<Modelo>
    {
    }

    public class ModelosRepository : RepositoryBase<Modelo>, IModelosRepository
    {
        public ModelosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
