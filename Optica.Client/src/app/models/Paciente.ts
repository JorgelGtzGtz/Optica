import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class Paciente {
  ID: number;
  Clave: string;
  Nombre: string;
  FechaNacimiento: any;
  Sexo: string;
  Direccion: string;
  NumeroInt: string;
  NumeroExt: string;
  Colonia: string;
  Ciudad: string;
  Estado: string;
  CodigoPostal: string;
  Referencia: string;
  Telefono: string;
  Celular: string;
  eMail: string;
  Observaciones: string;
  FechaCreacion: any;
  DireccionCliente: boolean;
  Estatus?: boolean;
  ID_Cliente: number;
  SelectedDate: NgbDateStruct;
  
  constructor() {
    this.ID = 0;
    this.FechaNacimiento = new Date();
    this.SelectedDate = {
        day: this.FechaNacimiento.getDate(),
        month: this.FechaNacimiento.getMonth() + 1,
        year: this.FechaNacimiento.getFullYear() };
  }
}
