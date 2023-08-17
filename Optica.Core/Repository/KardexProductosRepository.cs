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
    public interface IKardexProductosRepository : IRepositoryBase<KardexProducto>
    {
    }

    public class KardexProductosRepository : RepositoryBase<KardexProducto>, IKardexProductosRepository
    {
        public KardexProductosRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
