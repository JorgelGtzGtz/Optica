using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Factories;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Repository
{
    public interface ICorridaRepository : IRepositoryBase<corridaOriginal>
    {
        public List<dynamic> GetByDynamicFilter(Sql sql);
    }

    public class CorridaRepository : RepositoryBase<corridaOriginal>, ICorridaRepository
    {
        public CorridaRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public List<dynamic> GetByDynamicFilter(Sql sql)
        {
            return this.Context.Fetch<dynamic>(sql);
        }
    }
}
