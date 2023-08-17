import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class Cliente {
  ID: number;
  Clave: string;
  Nombre: string;
  FechaNacimiento: Date;
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
  Email: string;
  Rfc: string;
  DireccionCredencialFrente: string;
  DireccionCredencialAtras: string;
  ImagenCredencialFrente: string;
  ImagenCredencialAtras: string;
  ImagenCliente: string;
  ImagenCasa: string;
  Observaciones: string;
  FechaCreacion: any;
  Estatus?: boolean;
  ID_Sucursal: number;
  ID_Zona: number;
  ID_Usuario: number;
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
