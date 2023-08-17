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
    public interface IDocumentosCobranzaRepository : IRepositoryBase<DocumentosCobranza>
    {
    }

    public class DocumentosCobranzaRepository : RepositoryBase<DocumentosCobranza>, IDocumentosCobranzaRepository
    {
        public DocumentosCobranzaRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
