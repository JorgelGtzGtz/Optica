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
    public interface ILogRepository : IRepositoryBase<Log>
    {
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
