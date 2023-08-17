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
    public interface IProveedoresRepository : IRepositoryBase<Proveedore>
    {
    }

    public class ProveedoresRepository : RepositoryBase<Proveedore>, IProveedoresRepository
    {
        public ProveedoresRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
