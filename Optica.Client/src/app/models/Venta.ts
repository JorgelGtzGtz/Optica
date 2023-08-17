import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class Venta {
  ID: number;
  Folio: string;
  Fecha: any;
  FechaCreacion: any;
  Descuento?: number;
  Subtotal?: number;
  Iva?: number;
  Total?: number;
  DescTotal?: number;
  Estatus: string;
  Observaciones: string;
  ID_Cliente: number;
  ID_Sucursal: number;
  ID_Almacen: number;
  ID_Vendedor: number;
  ID_MetodoPago: number;
  SelectedDate: NgbDateStruct;

  constructor() {
    this.ID = 0;
    this.Fecha = new Date();
    this.FechaCreacion = new Date();
    this.SelectedDate = {
        day: this.Fecha.getDate(),
        month: this.Fecha.getMonth() + 1,
        year: this.Fecha.getFullYear() };
  }
}
