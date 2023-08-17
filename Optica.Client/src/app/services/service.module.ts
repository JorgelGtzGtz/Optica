import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import {
  UsersService,
  LoginGuardGuard,
  TiposUsuarioService,
  SucursalesService,
  AlmacenesService,
  ZonasService,
  MarcasService,
  ModelosService,
  ColoresService,
  MaterialLenteService,
  TipoLenteService,
  ColorLenteService,
  TipoEntradaSalidaService,
  ProveedoresService,
  ClientesService,
  PacientesService,
  ProductosService,
  ComprasService,
  GenerarProductoKitService,
  DiagnosticosService,
  VentasService

 } from './service.index';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    UsersService,
    LoginGuardGuard,
    TiposUsuarioService,
    SucursalesService,
    AlmacenesService,
    ZonasService,
    MarcasService,
    ModelosService,
    ColoresService,
    MaterialLenteService,
    TipoLenteService,
    ColorLenteService,
    TipoEntradaSalidaService,
    ProveedoresService,
    ClientesService,
    PacientesService,
    ProductosService,
    ComprasService,
    GenerarProductoKitService,
    DiagnosticosService,
    VentasService

  ]
})
export class ServiceModule { }
