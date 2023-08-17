import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class Compra {
  ID: number;
  ClaveFactura: string;
  FechaFactura: any;
  Subtotal: number;
  Iva: number;
  Total: number;
  Descuento: number;
  PorcentajeDescuento: number;
  Estatus: string;
  Observaciones: string;
  ID_Almacen: number;
  ID_Proveedor: number;
  SelectedDate: NgbDateStruct;

  constructor() {
    this.ID = 0;
    this.PorcentajeDescuento = 0;
    this.FechaFactura = new Date();
    this.SelectedDate = {
        day: this.FechaFactura.getDate(),
        month: this.FechaFactura.getMonth() + 1,
        year: this.FechaFactura.getFullYear() };
  }
}
