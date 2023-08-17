export class Usuario {
  ID: number;
  Nombre: string;
  _Usuario: string;
  Contrasena: string;
  Celular: string;
  eMail: string;
  Status: string;
  ID_TipoUsuario: number;
  ID_Sucursal: number;
  SuperAdmin?: boolean;
  Optometrista?: boolean;
}
