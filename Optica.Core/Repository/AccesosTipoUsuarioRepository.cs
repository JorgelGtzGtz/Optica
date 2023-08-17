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
    public interface IAccesosTipoUsuarioRepository : IRepositoryBase<AccesosTipoUsuario>
    {
    }

    public class AccesosTipoUsuarioRepository : RepositoryBase<AccesosTipoUsuario>, IAccesosTipoUsuarioRepository
    {
        public AccesosTipoUsuarioRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
