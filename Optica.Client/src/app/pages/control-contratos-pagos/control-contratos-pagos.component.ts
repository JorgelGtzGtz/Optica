import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Contrato } from 'src/app/models/Contrato';
import { PagosService } from 'src/app/services/pagos/pagos.service';

@Component({
  selector: 'app-control-contratos-pagos',
  templateUrl: './control-contratos-pagos.component.html',
  styleUrls: ['./control-contratos-pagos.component.css']
})
export class ControlContratosPagosComponent implements OnInit {

  model: Contrato = new Contrato();
  idCliente: number;
  idContrato: number;
  estatus: number;
  contratoSelect: boolean = false;
  detalles: any[] = [];
  formaPago: any[] = [];
  clientes: any[] = [];
  contratos: any[] = [];
  corridaOriginal: any[] = [];
  estadoCuenta: any[] = [];
  pagos: any[] = [];
  detallePagos: any[] = [];
  frecuencia: any;
  dia: any;
  fecInicial: any;
  fec: any;
  fecContraEntrega: any;
  sucursal: any;


  constructor(private _pagosSerice: PagosService, private toastr: ToastrService) { }

  test() {
    this.contratoSelect = !this.contratoSelect
  }

  tabs: string[] = ['Corrida Original', 'Edo. Cuenta', 'Pagos', 'Detalle Pagos'];
  selectedTab: string = '';

  onTabSelected(tab: string) {
    this.selectedTab = tab;

    if (tab == "Corrida Original") {
      this._pagosSerice.getCorridaOriginal(this.idContrato)
        .subscribe(
          data => {
            this.corridaOriginal = data;
          }
        );
    }

    if (tab == "Edo. Cuenta") {
      this._pagosSerice.getEstadoCuenta(this.idContrato)
        .subscribe(
          data => {
            this.estadoCuenta = data;
          }
        );
    }

    if (tab == "Pagos") {
      this._pagosSerice.getPagosContrato(this.idContrato)
        .subscribe(
          data => {
            this.pagos = data;
          }
        );
    }

    if (tab == "Detalle Pagos") {
      this._pagosSerice.getDetallePagos(this.idContrato)
        .subscribe(
          data => {
            console.log(data)
            this.detallePagos = data;
          }
        );
    }
    // Aquí puedes agregar lógica adicional al cambiar de pestaña.
  }

  ngOnInit() {
    this.getCombos();
  }

  frecuencias = {
    1: "Mensual",
    2: "Quincenal",
    3: "Catorcenal",
    4: "Semanal"
  };

  dias = {
    1: "Lunes",
    2: "Martes",
    3: "Miercoles",
    4: "Jueves",
    5: "Viernes",
    6: "Sabado",
    7: "Domingo",
  }

  estados = {
    1: "Vigente",
    2: "Liquidado",
    3: "Cancelado"
  }

  setContrato() {
    this.fecInicial = "";
    this.fecContraEntrega = "";
    this.frecuencia = "";
    this.dia = "";
    this._pagosSerice.getContrato(this.idContrato)
      .subscribe(
        data => {
          this.contratoSelect = true;
          this.model = data;
          this.setFechas();
          this.setSucursal();
          this.frecuencia = this.frecuencias[this.model.frecuencia]
          this.dia = this.dias[this.model.DiaCobro]
        }
      );
  }

  setSucursal(){
    this._pagosSerice.getSucursal(this.model.ID_Sucursal)
    .subscribe(
      data=>{
        this.sucursal = data.Nombre
      }
    );
  }

  setFechas(){
    const fechaObj = new Date(this.model.fechaInicial);
    const dia = fechaObj.getDate();
    const mes = fechaObj.getMonth() + 1;
    const anio = fechaObj.getFullYear();
    this.fecInicial = `${dia < 10 ? '0' : ''}${dia}-${mes < 10 ? '0' : ''}${mes}-${anio}`;

    if (this.model.fechaEspEnt) {
      const fechaObj1 = new Date(this.model.fechaInicial);
      const dia1 = fechaObj1.getDate();
      const mes1 = fechaObj1.getMonth() + 1;
      const anio1 = fechaObj1.getFullYear();
      this.fecContraEntrega = `${dia1 < 10 ? '0' : ''}${dia1}-${mes1 < 10 ? '0' : ''}${mes1}-${anio1}`;
    }

    const fechaObj2 = new Date(this.model.fechaInicial);
    const dia2 = fechaObj2.getDate();
    const mes2 = fechaObj2.getMonth() + 1;
    const anio2 = fechaObj2.getFullYear();
    this.fec = `${dia2 < 10 ? '0' : ''}${dia2}-${mes2 < 10 ? '0' : ''}${mes2}-${anio2}`;

  }

  getEstadoDesc(status: any) {
    const res = this.estados[status];
    return res;
  }

  obtenerDescripcionFormaDePago(formaPagoID: number): string {
    const formaPago = this.formaPago.find(fp => fp.ID === formaPagoID);
    return formaPago ? formaPago.Descripcion : 'Forma de pago no definida';
  }

  getContratos() {
    this._pagosSerice.getContratos(this.idCliente)
      .subscribe(
        data => {
          this.contratos = data
        },
        error => this.toastr.error(error.message, 'Error!'));
  }

  getCombos() {
    this._pagosSerice.getCombos()
      .subscribe(
        data => {
          this.clientes = data.clientes
          this.formaPago = data.metodospago
        },
        error => this.toastr.error(error.message, 'Error!'));
  }

}
