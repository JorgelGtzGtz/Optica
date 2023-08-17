export class VentaDetalle {
  ID: number;
  Clave: string;
  Descripcion: string;
  Cantidad: number;
  Costo: number;
  CostoTotal: number;
  Descuento: number;
  DescTotal: number;
  Precio: number;
  PrecioTotal: number;
  ID_Producto: number;
  ID_Venta: number;

  constructor() {
    this.ID = 0;
    this.Cantidad = 0;
    this.Costo = 0;
    this.CostoTotal = 0;
    this.Descuento = 0;
    this.DescTotal = 0;
    this.Precio = 0;
    this.PrecioTotal = 0;
  }
}
