import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class Diagnostico {
  ID: number;
  Fecha: any;
  RxOD: string;
  RxOI: string;
  Observaciones: string;
  ID_TipoLente: number;
  ID_Material: number;
  ID_ColorLente: number;
  ID_Paciente: number;
  ID_Optometrista: number;
  ID_Sucursal: number;
  OD_esfera: string;
  OI_esfera: string;
  OD_cilindro: string;
  IO_cilindro: string;
  OD_eje: string;
  OI_eje: string;
  OD_adicion: string;
  OI_adicion: string;
  Dist_interpupilar: string;
  Altura_oblea: string;
  SelectedDate: NgbDateStruct;

  constructor() {
    this.ID = 0;
    this.Fecha = new Date();
    this.RxOD = '';
    this.RxOI = '';
    this.SelectedDate = {
        day: this.Fecha.getDate(),
        month: this.Fecha.getMonth() + 1,
        year: this.Fecha.getFullYear() };
  }
}
