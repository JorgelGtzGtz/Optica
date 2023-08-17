using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Entities.Dto
{
    public class GenerarKitDto
    {
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public int ID_Producto { get; set; }
        public int ID_Almacen { get; set; }
    }
}
