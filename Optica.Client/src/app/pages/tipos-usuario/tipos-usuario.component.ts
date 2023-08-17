import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { TipoUsuario } from '../../models/TipoUsuario';
import { UsersService } from '../../services/users/users.service';
import { TiposUsuarioService } from '../../services/tiposUsuario/tipos-usuario.service';
import { Acceso } from '../../models/Acceso';

@Component({
  selector: 'app-tipos-usuario',
  templateUrl: './tipos-usuario.component.html',
  styleUrls: ['./tipos-usuario.component.css']
})
export class TiposUsuarioComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-lg'
  };
  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };
  usuarioFilter: string = '';
  tiposusuario: any[] = [];
  tipousuario: TipoUsuario = new TipoUsuario();
  accesos: any[] = [];
  selectedAccesoDisponibles: Acceso[] = [];
  selectedAccesoAsignados: Acceso[] = [];
  AccesoDisponibles: Acceso[] = [];
  AccesoAsignados: Acceso[] = [];

  constructor(private _userService: UsersService, private _tipoUserService: TiposUsuarioService, private modalService: BsModalService, private toastr: ToastrService) {
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.onBuscar();
  }

  onBuscar() {
    this._tipoUserService.getLista(this.usuarioFilter).subscribe(
      (data: any) => {
        this.tiposusuario = data;
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
      const data = {
        tipo: this.tipousuario,
        accesos: this.AccesoAsignados
      };

      this._tipoUserService.guardar(data)
    .subscribe(
      success => {
        this.toastr.success('Tipo de Usuario guardado con exito.', 'Guardado!');
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
      text: 'Esta seguro que quiere eliminar el tipo de usuario, no se podra revertir!',
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
        this._tipoUserService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Tipo de usuario a sido eliminado con exito.',
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
    this.getAccesos();
    this.tipousuario = new TipoUsuario();
    if (id <= 0) {
      this.AccesoAsignados = [];
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._tipoUserService.getTipoUsuario(id)
    .subscribe(
      data => {
        this.tipousuario = data.usuario;
        this.AccesoAsignados = data.accesos;
        this.FillAccesos();
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getAccesos() {
    this._tipoUserService.getAccesos()
      .subscribe(
        data => {
          this.AccesoDisponibles = data;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  AgregarAcceso() {
    this.selectedAccesoDisponibles.forEach(acceso => {
      const  existingAccess = this.AccesoAsignados.filter(
        item => item.ID === acceso.ID
      )[0];

      if (!existingAccess) {
        this.AccesoAsignados.push(acceso);
        const accesoIndex = this.AccesoDisponibles.findIndex((accesoItem: Acceso) => accesoItem.ID === acceso.ID);
        if (accesoIndex !== -1) {
            this.AccesoDisponibles.splice(accesoIndex, 1);
        }
      }
    });
  }

  RemoverAcceso() {
    this.selectedAccesoAsignados.forEach(acceso => {
      const  existingAccess = this.AccesoAsignados.filter(
        item => item.ID === acceso.ID
      )[0];

      if (existingAccess) {

        const accesoIndex = this.AccesoAsignados.indexOf(existingAccess);
        const newAcceso = new Acceso();
        newAcceso.ID = acceso.ID;
        newAcceso.Nombre = acceso.Nombre;
        newAcceso.Status = acceso.Status;

        if (accesoIndex !== -1) {
          this.AccesoAsignados.splice(accesoIndex, 1);
        }

        const  existingDisponibleAccess = this.AccesoDisponibles.filter(
          item => item.ID === acceso.ID
        )[0];

        if (!existingDisponibleAccess) {
          this.AccesoDisponibles.push(newAcceso);
        }
      }
    });
  }

  FillAccesos() {
    if (this.AccesoAsignados) {
      this.AccesoAsignados.forEach(acceso => {
        const existingAccess = this.AccesoDisponibles.filter(
          item => item.ID === acceso.ID
        )[0];

        if (existingAccess) {
          const accesoIndex = this.AccesoDisponibles.findIndex((accesoItem: Acceso) => accesoItem.ID === existingAccess.ID);
          if (accesoIndex !== -1) {
            this.AccesoDisponibles.splice(accesoIndex, 1);
          }
        }
      });
    }
  }
}
