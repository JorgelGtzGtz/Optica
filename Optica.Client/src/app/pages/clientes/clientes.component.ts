import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, ClientesService } from 'src/app/services/service.index';
import { Cliente } from '../../models/Cliente';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class ClientesComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  modalIneFRef: BsModalRef;
  modalIneRRef: BsModalRef;
  modalClienteRef: BsModalRef;
  modalCasaRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-lg'
  };

  configMd = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-md'
  };

  modelFilter: string = '';
  lista: any[] = [];
  model: Cliente = new Cliente();
  zonas: any[] = [];
  cobradores: any[] = [];
  sucursales: any[] = [];
  public imgIneFPath;
  imgIneF: any;
  public imgIneRPath;
  imgIneR: any;
  public imgClientePath;
  imgCliente: any;
  public imgCasaPath;
  imgCasa: any;
  public message: string;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _clienteService: ClientesService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.imgIneF = 'assets/images/default-upload.png';
    this.imgIneR = 'assets/images/default-upload.png';
    this.imgCliente = 'assets/images/default-upload.png';
    this.imgCasa = 'assets/images/default-upload.png';
    this.onBuscar();
    this.getCombos();
  }

  onBuscar() {
    this._clienteService.getLista(this.modelFilter).subscribe(
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
      if (this.model.FechaCreacion === undefined || this.model.FechaCreacion === null ) {
        this.model.FechaCreacion = new Date();
      }

      if (this.imgIneF !== 'assets/images/default-upload.png' && this.imgIneF !== this.model.ImagenCredencialFrente) {
        this.model.ImagenCredencialFrente = this.imgIneF;
      }

      if (this.imgIneR !== 'assets/images/default-upload.png' && this.imgIneR !== this.model.ImagenCredencialAtras) {
        this.model.ImagenCredencialAtras = this.imgIneR;
      }

      if (this.imgCliente !== 'assets/images/default-upload.png' && this.imgCliente !== this.model.ImagenCliente) {
        this.model.ImagenCliente = this.imgCliente;
      }

      if (this.imgCasa !== 'assets/images/default-upload.png' && this.imgCasa !== this.model.ImagenCasa) {
        this.model.ImagenCasa = this.imgCasa;
      }

      this._clienteService.guardar(this.model)
    .subscribe(
      success => {
        this.toastr.success('Cliente guardado con exito.', 'Guardado!');
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
      text: 'Esta seguro que quiere eliminar cliente, no se podra revertir!',
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
        this._clienteService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Cliente a sido eliminado con exito.',
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
    this.model = new Cliente();
    this.imgIneF = 'assets/images/default-upload.png';
    this.imgIneR = 'assets/images/default-upload.png';
    this.imgCliente = 'assets/images/default-upload.png';
    this.imgCasa = 'assets/images/default-upload.png';
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._clienteService.getCliente(id)
    .subscribe(
      data => {
        this.model = data;
        if (this.model.FechaNacimiento) {
          this.model.FechaNacimiento = new Date(this.model.FechaNacimiento);
          this.model.SelectedDate = this.getDateStructFromDate(this.model.FechaNacimiento);
        }

        if (this.model.ImagenCredencialFrente) {
            this.imgIneF = this.model.ImagenCredencialFrente;
        }
        if (this.model.ImagenCredencialAtras) {
          this.imgIneR = this.model.ImagenCredencialAtras;
        }
        if (this.model.ImagenCliente) {
          this.imgCliente = this.model.ImagenCliente;
        }
        if (this.model.ImagenCasa) {
          this.imgCasa = this.model.ImagenCasa;
        }
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getCombos() {
    this._clienteService.getCombos()
      .subscribe(
        data => {
          this.sucursales = data.sucursales;
          this.zonas = data.zonas;
          this.cobradores = data.cobradores;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  previewIneF(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.imgIneFPath = files;
    reader.readAsDataURL(files[0]);
    // tslint:disable-next-line: variable-name
    reader.onload = (_event) => {
      this.imgIneF = reader.result;
    };
  }

  previewIneR(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.imgIneRPath = files;
    reader.readAsDataURL(files[0]);
    // tslint:disable-next-line: variable-name
    reader.onload = (_event) => {
      this.imgIneR = reader.result;
    };
  }
  previewCliente(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.imgClientePath = files;
    reader.readAsDataURL(files[0]);
    // tslint:disable-next-line: variable-name
    reader.onload = (_event) => {
      this.imgCliente = reader.result;
    };
  }
  previewCasa(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.imgCasaPath = files;
    reader.readAsDataURL(files[0]);
    // tslint:disable-next-line: variable-name
    reader.onload = (_event) => {
      this.imgCasa = reader.result;
    };
  }

  onShowIneF(template: TemplateRef<any>) {
    this.modalIneFRef = this.modalService.show(template, this.configMd);
  }
  onShowIneR(template: TemplateRef<any>) {
    this.modalIneRRef = this.modalService.show(template, this.configMd);
  }
  onShowFoto(template: TemplateRef<any>) {
    this.modalClienteRef = this.modalService.show(template, this.configMd);
  }
  onShowCasa(template: TemplateRef<any>) {
    this.modalCasaRef = this.modalService.show(template, this.configMd);
  }

  onStartChanged(selectedDate: NgbDateStruct) {
    this.model.FechaNacimiento = this.getDateFromDateStruct(selectedDate);
  }

  getDateFromDateStruct(date: NgbDateStruct): Date {
    return new Date(date.year, date.month - 1, date.day);
  }

  getDateStructFromDate(date: Date): NgbDateStruct {
    const dateStruct: NgbDateStruct = {
      day: date.getDate(),
      month: date.getMonth() + 1,
      year: date.getFullYear()
    };

    return dateStruct;
  }
}
