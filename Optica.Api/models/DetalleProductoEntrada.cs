using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Api.Models
{
    public class DetalleProductoEntrada
    {
        public int ID_Producto { get; set; }
        public int ID_OtrasEntradasSalidas { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
        public decimal CostoTotal { get; set; }

    }
}