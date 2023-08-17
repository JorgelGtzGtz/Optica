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
    public interface IAlmacenRepository : IRepositoryBase<Almacene>
    {
        List<dynamic> GetByDynamicFilter(Sql sql);
    }

    public class AlmacenRepository : RepositoryBase<Almacene>, IAlmacenRepository
    {
        public AlmacenRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<dynamic> GetByDynamicFilter(Sql sql)
        {
            return this.Context.Fetch<dynamic>(sql);
        }
    }
}
