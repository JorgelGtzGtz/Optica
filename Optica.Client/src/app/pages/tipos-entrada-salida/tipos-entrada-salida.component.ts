import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { UsersService, TipoEntradaSalidaService } from 'src/app/services/service.index';
import { TipoEntradaSalida } from '../../models/TipoEntradaSalida';

@Component({
  selector: 'app-tipos-entrada-salida',
  templateUrl: './tipos-entrada-salida.component.html',
  styleUrls: ['./tipos-entrada-salida.component.css']
})
export class TiposEntradaSalidaComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };
  modelFilter: string = '';
  lista: any[] = [];
  model: TipoEntradaSalida = new TipoEntradaSalida();
  radiotipo: any;

  constructor(private _userService: UsersService, private _tipoentradasalidaService: TipoEntradaSalidaService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.onBuscar();
  }

  onBuscar() {
    this._tipoentradasalidaService.getLista(this.modelFilter).subscribe(
      (data: any) => {
        this.lista = data;
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

  onSubmit(FormData) {
    if (FormData.valid) {
      this._tipoentradasalidaService.guardar(this.model)
    .subscribe(
      success => {
        this.toastr.success('Tipo de Entrada/Salida guardado con exito.', 'Guardado!');
        this.onBuscar();
        FormData.resetForm();
        this.modalRef.hide();
      },
      error => {
        this.toastr.error(error.message, 'Error!');
      });
    }
  }

  onDelete(id: number) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere eliminar tipo de entrada/salida, no se podra revertir!',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, Eliminar!',
      focusConfirm: false,
      focusCancel: false,
      allowEnterKey: false
    }).then((result) => {
      if (result.value) {
        this._tipoentradasalidaService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Tipo de Entrada/Salida a sido eliminado con exito.',
              type: 'success',
              confirmButtonText: 'Aceptar'
            });
          },
          error => {
            this.toastr.error(error.message, 'Error!');
          });
      }
    });
  }

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new TipoEntradaSalida();
    this.radiotipo = 1;
    if (id <= 0) {
      this.onRadioChanged();
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._tipoentradasalidaService.getTipoEntradaSalida(id)
    .subscribe(
      data => {
        this.model = data;
        this.modalRef = this.modalService.show(template, this.config);
        if (this.model.Tipo === 'Entrada') {
          this.radiotipo = 1;
        } else {
          this.radiotipo = 2;
        }
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  onRadioChanged() {
    if (this.radiotipo === 1) {
      this.model.Tipo = 'Entrada';
    } else {
      this.model.Tipo = 'Salida';
    }
  }
}
