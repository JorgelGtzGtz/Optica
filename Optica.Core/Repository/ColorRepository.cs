﻿using dbconnection;
using Optica.Core.Entities;
using Optica.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Repository
{
    public interface IColorRepository : IRepositoryBase<Colore>
    {
    }

    public class ColorRepository : RepositoryBase<Colore>, IColorRepository
    {
        public ColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
