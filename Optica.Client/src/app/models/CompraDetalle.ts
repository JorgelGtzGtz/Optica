export class CompraDetalle {
  ID: number;
  Clave: string;
  Descripcion: string;
  Cantidad: number;
  Costo: number;
  CostoSinDescuento: number;
  CostoConDescuento: number;
  Total: number;
  ID_Producto: number;
  ID_Compra: number;

  constructor() {
    this.ID = 0;
    this.Cantidad = 0;
    this.Costo = 0;
  }
}
