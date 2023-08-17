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
    public interface IServiciosRepository : IRepositoryBase<Servicio>
    {
    }

    public class ServiciosRepository : RepositoryBase<Servicio>, IServiciosRepository
    {
        public ServiciosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
