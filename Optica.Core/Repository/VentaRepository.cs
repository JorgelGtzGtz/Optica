﻿using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Factories;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Repository
{
    public interface IVentaRepository : IRepositoryBase<Venta>
    {
        List<dynamic> GetByDynamicFilter(Sql sql);
    }

    public class VentaRepository : RepositoryBase<Venta>, IVentaRepository
    {
        public VentaRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<dynamic> GetByDynamicFilter(Sql sql)
        {
            return this.Context.Fetch<dynamic>(sql);
        }
    }
}
