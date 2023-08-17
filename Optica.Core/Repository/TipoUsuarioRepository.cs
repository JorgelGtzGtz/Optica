using dbconnection;
using Optica.Core.Factories;

namespace Optica.Core.Repository
{
    public interface ITipoUsuarioRepository : IRepositoryBase<TiposUsuario>
    {
    }

    public class TipoUsuarioRepository : RepositoryBase<TiposUsuario>, ITipoUsuarioRepository
    {
        public TipoUsuarioRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
