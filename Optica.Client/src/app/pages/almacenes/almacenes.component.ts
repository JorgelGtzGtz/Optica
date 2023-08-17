import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Almacen } from '../../models/Almacen';
import { AlmacenesService } from '../../services/almacenes/almacenes.service';
import { UsersService } from '../../services/users/users.service';

@Component({
  selector: 'app-almacenes',
  templateUrl: './almacenes.component.html',
  styleUrls: ['./almacenes.component.css']
})
export class AlmacenesComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-lg'
  };
  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };
  almacenFilter: string = '';
  almacenes: any[] = [];
  model: Almacen = new Almacen();
  sucursales: any[] = [];
  constructor(private _userService: UsersService, private _almacenService: AlmacenesService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.onBuscar();
  }

  onBuscar() {
    this._almacenService.getLista(this.almacenFilter).subscribe(
      (data: any) => {
        this.almacenes = data;
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
      this._almacenService.guardar(this.model)
    .subscribe(
      success => {
        this.toastr.success('Almacen guardado con exito.', 'Guardado!');
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
      text: 'Esta seguro que quiere eliminar almacen, no se podra revertir!',
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
        this._almacenService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Almacen a sido eliminado con exito.',
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
    this.model = new Almacen();
    this.getSucursales();
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._almacenService.getAlmacen(id)
    .subscribe(
      data => {
        this.model = data;
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getSucursales() {
    this._almacenService.getSucursales()
      .subscribe(
        data => {
          this.sucursales = data;
        },
        error => this.toastr.error(error.message, 'Error!') );
   }
}
