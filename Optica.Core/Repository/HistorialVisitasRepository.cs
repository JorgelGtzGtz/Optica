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
    public interface IHistorialVisitasRepository : IRepositoryBase<HistorialVisita>
    {
    }

    public class HistorialVisitasRepository : RepositoryBase<HistorialVisita>, IHistorialVisitasRepository
    {
        public HistorialVisitasRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
