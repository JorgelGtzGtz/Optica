import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { MetodosPago } from 'src/app/models/MetodosPago';
import { MetodospagoService } from 'src/app/services/metodopagos/metodospago.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-metodos-pago',
  templateUrl: './metodos-pago.component.html',
  styleUrls: ['./metodos-pago.component.css']
})
export class MetodosPagoComponent implements OnInit {

  metodospago:any[] = [];
  model:MetodosPago = new MetodosPago();
  descripcion:string = "";
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };
  constructor(private modalService: BsModalService, private _metodosPagoService: MetodospagoService, private toastr: ToastrService) { }

  ngOnInit() {
    this.onBuscar();
  }

  onSubmit(FormData){
    if (this.model.Descripcion != ""){
      const metodopago = {
        Descripcion: this.model.Descripcion,
        Estatus: 1
      }

      this._metodosPagoService.guardar(metodopago).
      subscribe(
        success => {
          this.toastr.success('Metodo de pago guardado con exito.', 'Guardado!');
          FormData.resetForm();
          this.modalRef.hide();
          this.onBuscar();
        },
        error => {
          this.toastr.error(error.message, 'Error!');
        });
    }
  }

  onBuscar() {
    console.log(this.descripcion)
    this._metodosPagoService.getLista(this.descripcion).
    subscribe(
      (data: any) => {
        console.log(data)
        this.metodospago = data;
      },
      (error) => {
        Swal.fire({
          title: 'Error!',
          text: String(error.message),
          type: 'error',
          focusConfirm: false,
          focusCancel: false,
          allowEnterKey: false
        });
      }
    );
  }

  onShow(id: number, template: TemplateRef<any>) {
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } 
  }

}
