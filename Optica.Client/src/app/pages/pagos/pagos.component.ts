import { Component, OnInit, TemplateRef } from '@angular/core';
import { error } from 'console';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Contrato } from 'src/app/models/Contrato';
import { Pago } from 'src/app/models/Pago';
import { PagosService } from 'src/app/services/pagos/pagos.service';

@Component({
  selector: 'app-pagos',
  templateUrl: './pagos.component.html',
  styleUrls: ['./pagos.component.css']
})
export class PagosComponent implements OnInit {
  modalRef: BsModalRef;
  config = {
    class: 'modal-lg',
    backdrop: true,
    ignoreBackdropClick: true,
  };

  status: any[] = ["Vigentes", "Cancelados", "Todos"];
  clientes: any[] = [];
  formasPago: any[] = [];
  contratos: any[] = [];
  pagos: any[] = [];
  model: Pago;
  liquidarContratoDisabled: boolean = false;
  importesugerido: any;
  fechaSelect: boolean = false;
  importeAplicado: boolean = false;
  constructor(private modalService: BsModalService,private _pagosService: PagosService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getCombos();
  }

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Pago();
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } 
  }

  clienteSelect(){
    this.handelPago();
    this._pagosService.getContratos(this.model.Usuario)
    .subscribe(
      data => {
        console.log(data)
        this.contratos = data
      },
        error => this.toastr.error(error.message, 'Error!')
    );
  }

  aplicarImporte(){
    this._pagosService.aplicarImporte(this.model.ID_Contrato, this.model.Importe, this.model.Fecha)
    .subscribe(
      data => {
        console.log(data)
        this.pagos = data
      },
        error => this.toastr.error(error.message, 'Error!')
    );
  }

  onSubmit(){

    const data = {
      data: this.model,
      detalles: this.pagos
    }

    this._pagosService.pagoContrato(data).subscribe(
      data =>{
        this.toastr.success('Pago realizado con exito.', 'Guardado!');
        this.pagos = [];
          this.modalRef.hide();
          //this.onBuscar();
      }
    );
  }

  handelPago(){
    this.importeAplicado = false;
  }

  getCombos(){
    this._pagosService.getCombos()
      .subscribe(
        data => {
          this.clientes = data.clientes
          this.formasPago = data.metodospago
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  getPagos(){
    this._pagosService.getPagos(this.model.ID_Contrato, this.model.Fecha)
      .subscribe(
        data => {
          console.log(data)
          this.pagos = data
        },
        error => this.toastr.error(error.message, 'Error!') );

  }
}
