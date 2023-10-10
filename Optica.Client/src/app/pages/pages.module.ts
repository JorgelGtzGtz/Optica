import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

// Modules
import { SharedModule } from '../shared/shared.module';
import { PagesRoutingModule } from './pages-routing.module';

// Directives
import { DatepickerToggleDirective } from '../directives/datepicker-toggle.directive';

// Routes
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
import { TipoLenteComponent } from './tipo-lente/tipo-lente.component';
import { ColorLenteComponent } from './color-lente/color-lente.component';
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
import { ContratosComponent } from './contratos/contratos.component';


@NgModule({
    declarations: [
        DashboardComponent,
        SucursalesComponent,
        AlmacenesComponent,
        UsuariosComponent,
        TiposUsuarioComponent,
        ZonasComponent,
        MarcasComponent,
        ModelosComponent,
        ColoresComponent,
        MaterialLenteComponent,
        TipoLenteComponent,
        ColorLenteComponent,
        TiposEntradaSalidaComponent,
        ProveedoresComponent,
        ClientesComponent,
        PacientesComponent,
        ProductosComponent,
        ComprasComponent,
        GenerarProductoKitComponent,
        DiagnosticosComponent,
        VentasComponent,
        DatepickerToggleDirective,
        EntradasComponent,
        SalidasComponent,
        InventarioComponent,
        ContratosComponent,
        // ClientesComponent,
        // ModelosComponent,
        // ProductosComponent,
        // TiposEntradaSalidaComponent,
        // ProveedoresComponent,
        // LineasProveedorComponent,
        // LineasProductoComponent,
        // SublineasProductoComponent,
        // UnidadMedidaComponent,
        // ExistenciasComponent,
        // MergeProductoComponent,
        // DescomposicionProductoComponent,
        // GeneracionProductokitComponent,
        // InventarioFisicoComponent,
        // TiposESComponent,
        // EntradasComponent,
        // SalidasComponent,
        // TraspasosComponent,
        // RecepcionTraspasosComponent,
        // ComodatosComponent,
        // ComodatoMultipleComponent,
        // RetirosComponent,
        // RemisionVentasComponent,
        // SolicitudAlmacenComponent,
        // OrdenComprasComponent,
        // ComprasComponent,
        // VentasComponent,
        // CotizacionesComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        PagesRoutingModule,
        NgSelectModule,
        NgOptionHighlightModule,
        NgbDatepickerModule
    ],
    exports: [
        DashboardComponent
    ]
})
export class PagesModule {}
