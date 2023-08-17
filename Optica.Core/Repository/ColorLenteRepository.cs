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
    public interface IColorLenteRepository : IRepositoryBase<ColoresLente>
    {
    }

    public class ColorLenteRepository : RepositoryBase<ColoresLente>, IColorLenteRepository
    {
        public ColorLenteRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
