import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, PacientesService } from 'src/app/services/service.index';
import { Paciente } from '../../models/Paciente';

@Component({
  selector: 'app-pacientes',
  templateUrl: './pacientes.component.html',
  styleUrls: ['./pacientes.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class PacientesComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-lg'
  };

  modelFilter: string = '';
  lista: any[] = [];
  model: Paciente = new Paciente();
  clientes: any[] = [];
  radiotipo: any;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _pacienteService: PacientesService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.onBuscar();
    this.getCombos();
  }

  onBuscar() {
    this._pacienteService.getLista(this.modelFilter).subscribe(
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

      this._pacienteService.guardar(this.model)
    .subscribe(
      success => {
        this.toastr.success('Paciente guardado con exito.', 'Guardado!');
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
      text: 'Esta seguro que quiere eliminar paciente, no se podra revertir!',
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
        this._pacienteService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Paciente a sido eliminado con exito.',
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
    this.model = new Paciente();
    this.radiotipo = 1;
    if (id <= 0) {
      this.onRadioChanged();
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._pacienteService.getPaciente(id)
    .subscribe(
      data => {
        this.model = data;
        if (this.model.FechaNacimiento) {
          this.model.FechaNacimiento = new Date(this.model.FechaNacimiento);
          this.model.SelectedDate = this.getDateStructFromDate(this.model.FechaNacimiento);
        }

        if (this.model.Sexo === 'M') {
          this.radiotipo = 1;
        } else {
          this.radiotipo = 2;
        }
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getCombos() {
    this._pacienteService.getCombos()
      .subscribe(
        data => {
          this.clientes = data.clientes;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  onRadioChanged() {
    if (this.radiotipo === 1) {
      this.model.Sexo = 'M';
    } else {
      this.model.Sexo = 'F';
    }
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
