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
    public interface IContratosRepository : IRepositoryBase<Contrato>
    {
    }

    public class ContratosRepository : RepositoryBase<Contrato>, IContratosRepository
    {
        public ContratosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
