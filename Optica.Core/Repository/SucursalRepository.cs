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
    public interface ISucursalRepository : IRepositoryBase<Sucursale>
    {
    }

    public class SucursalRepository : RepositoryBase<Sucursale>, ISucursalRepository
    {
        public SucursalRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
