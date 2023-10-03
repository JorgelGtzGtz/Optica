import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SucursalesComponent } from './sucursales/sucursales.component';
import { AlmacenesComponent } from './almacenes/almacenes.component';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { TiposUsuarioComponent } from './tipos-usuario/tipos-usuario.component';
import { ZonasComponent } from './zonas/zonas.component';
import { MarcasComponent } from './marcas/marcas.component';
import { ModelosComponent } from './modelos/modelos.component';
import { ColoresComponent } from './colores/colores.component';
import { MaterialLenteComponent } from './materiallente/materiallente.component';
import { ColorLenteComponent } from './color-lente/color-lente.component';
import { TipoLenteComponent } from './tipo-lente/tipo-lente.component';
import { TiposEntradaSalidaComponent } from './tipos-entrada-salida/tipos-entrada-salida.component';
import { ProveedoresComponent } from './proveedores/proveedores.component';
import { ClientesComponent } from './clientes/clientes.component';
import { PacientesComponent } from './pacientes/pacientes.component';
import { ProductosComponent } from './productos/productos.component';
import { ComprasComponent } from './compras/compras.component';
import { GenerarProductoKitComponent } from './generarproductokit/generarproductokit.component';
import { DiagnosticosComponent } from './diagnosticos/diagnosticos.component';
import { VentasComponent } from './ventas/ventas.component';
import { EntradasComponent } from './entradas/entradas.component';
import { SalidasComponent } from './salidas/salidas.component';
import { InventarioComponent } from './inventario/inventario.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'sucursales',
    component: SucursalesComponent
  },
  {
    path: 'almacenes',
    component: AlmacenesComponent
  },
  {
    path: 'usuarios',
    component: UsuariosComponent
  },
  {
    path: 'tiposusuario',
    component: TiposUsuarioComponent
  },
  {
    path: 'zonas',
    component: ZonasComponent
  },
  {
    path: 'marcas',
    component: MarcasComponent
  },
  {
    path: 'modelos',
    component: ModelosComponent
  },
  {
    path: 'colores',
    component: ColoresComponent
  },
  {
    path: 'materiales',
    component: MaterialLenteComponent
  },
  {
    path: 'tipolente',
    component: TipoLenteComponent
  },
  {
    path: 'colorlente',
    component: ColorLenteComponent
  },
  {
    path: 'tiposentradasalida',
    component: TiposEntradaSalidaComponent
  },
  {
    path: 'proveedor',
    component: ProveedoresComponent
  },
  {
    path: 'clientes',
    component: ClientesComponent
  },
  {
    path: 'pacientes',
    component: PacientesComponent
  },
  {
    path: 'productos',
    component: ProductosComponent
  },
  {
    path: 'compras',
    component: ComprasComponent
  },
  {
    path: 'generarkit',
    component: GenerarProductoKitComponent
  },
  {
    path: 'diagnostico',
    component: DiagnosticosComponent
  },
  {
    path: 'ventas',
    component: VentasComponent
  },
  {
     path: 'entradas',
     component: EntradasComponent
  },
  {
    path: 'salidas',
    component: SalidasComponent
  },
  {
    path: 'inventario',
    component: InventarioComponent
  },
  // {
  //   path: 'clientes',
  //   component: ClientesComponent
  // },
  // {
  //   path: 'modelos',
  //   component: ModelosComponent
  // },
  // {
  //   path: 'productos',
  //   component: ProductosComponent
  // },
  // {
  //   path: 'proveedores',
  //   component: ProveedoresComponent
  // },
  // {
  //   path: 'tiposentradasalida',
  //   component: TiposEntradaSalidaComponent
  // },
  
  // {
  //   path: 'cotizaciones',
  //   component: CotizacionesComponent
  // },
  // {
  //   path: 'ventas',
  //   component: VentasComponent
  // },
  // {
  //   path: 'compras',
  //   component: ComprasComponent
  // },
  // {
  //   path: 'ordencompra',
  //   component: OrdenComprasComponent
  // },
  // {
  //   path: 'solicitudalmacen',
  //   component: SolicitudAlmacenComponent
  // },
  // {
  //   path: 'comodatos',
  //   component: ComodatosComponent
  // },
  // {
  //   path: 'comodatomulti',
  //   component: ComodatoMultipleComponent
  // },
  // {
  //   path: 'retiros',
  //   component: RetirosComponent
  // },
  // {
  //   path: 'remisionventas',
  //   component: RemisionVentasComponent
  // },
  // {
  //   path: 'traspasos',
  //   component: TraspasosComponent
  // },
  // {
  //   path: 'recepciontraspaso',
  //   component: RecepcionTraspasosComponent
  // },
  // {
  //   path: 'tiposes',
  //   component: TiposESComponent
  // },
  // {
  //   path: 'inventariofisico',
  //   component: InventarioFisicoComponent
  // },
  // {
  //   path: 'generarkit',
  //   component: GeneracionProductokitComponent
  // },
  // {
  //   path: 'descproducto',
  //   component: DescomposicionProductoComponent
  // },
  // {
  //   path: 'mergeproducto',
  //   component: MergeProductoComponent
  // },
  // {
  //   path: 'existencias',
  //   component: ExistenciasComponent
  // },
  // {
  //   path: 'lineaproducto',
  //   component: LineasProductoComponent
  // },
  // {
  //   path: 'sublineaproducto',
  //   component: SublineasProductoComponent
  // },
  // {
  //   path: 'unidadmedida',
  //   component: UnidadMedidaComponent
  // },
  // {
  //   path: 'lineaproveedor',
  //   component: LineasProveedorComponent
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
