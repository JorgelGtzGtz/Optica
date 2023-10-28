import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
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
  model: Pago;
  liquidarContratoDisabled: boolean = false;
  importesugerido: any;
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


  getCombos(){
    this._pagosService.getCombos()
      .subscribe(
        data => {
          this.clientes = data.clientes
          this.formasPago = data.metodospago
          this.contratos = data.contratos
          console.log(data)
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

}
