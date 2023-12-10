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
    public interface IPagaresRepository : IRepositoryBase<pagare>
    {
        public List<dynamic> GetByDynamicFilter(Sql sql);
        public List<pagare> GetPagares(Sql query);
        public List<corridaOriginal> GetCorridaOriginala(Sql query);
        public List<Pago> GetPagos(Sql sql);
    }

    public class PagaresRepository : RepositoryBase<pagare>, IPagaresRepository
    {
        public PagaresRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public List<dynamic> GetByDynamicFilter(Sql sql)
        {
            return this.Context.Fetch<dynamic>(sql);
        }

        public List<pagare> GetPagares(Sql sql)
        {
            return this.Context.Fetch<pagare>(sql);
        }

        public List<Pago> GetPagos(Sql sql)
        {
            return this.Context.Fetch<Pago>(sql);
        }

        public List<corridaOriginal> GetCorridaOriginala(Sql sql)
        {
            return this.Context.Fetch<corridaOriginal>(sql);
        }

    }
}
