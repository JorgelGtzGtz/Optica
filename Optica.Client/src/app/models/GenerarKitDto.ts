import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class GenerarKitDto {
  Cantidad: number;
  Fecha: any;
  Observaciones: string;
  ID_Producto: number;
  ID_Almacen: number;
  SelectedDate: NgbDateStruct;

  constructor() {
    this.Fecha = new Date();
    this.SelectedDate = {
        day: this.Fecha.getDate(),
        month: this.Fecha.getMonth() + 1,
        year: this.Fecha.getFullYear() };
  }
}
