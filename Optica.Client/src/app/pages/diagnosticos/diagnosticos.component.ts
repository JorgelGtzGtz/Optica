import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, DiagnosticosService } from 'src/app/services/service.index';
import { Diagnostico } from '../../models/Diagnostico';

@Component({
  selector: 'app-diagnosticos',
  templateUrl: './diagnosticos.component.html',
  styleUrls: ['./diagnosticos.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class DiagnosticosComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-lg'
  };

  configLarge = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'Custom-size-modal'
  };

  configMd = {
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'gray modal-md'
  };

  from: NgbDateStruct;
  to: NgbDateStruct;
  folio = '';
  factura = '';
  paciente: any;
  optometrista: any;

  lista: any[] = [];
  model: Diagnostico = new Diagnostico();

  pacientes: any[] = [];
  optometristas: any[] = [];
  tiposlente: any[] = [];
  materiales: any[] = [];
  coloreslente: any[] = [];

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _diagnosticosService: DiagnosticosService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.onBuscar();
    this.getCombos();
  }

  onBuscar() {
    let from = '';
    let to = ''; 
    if (this.from !== undefined && this.to !== undefined) {
      from = `${this.from.year}-${this.from.month}-${this.from.day}`;
      to = `${this.to.year}-${this.to.month}-${this.to.day}`;
    }

    this._diagnosticosService.getLista(from, to, this.paciente, this.optometrista, this.folio).subscribe(
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

      this._diagnosticosService.guardar(this.model)
    .subscribe(
      success => {
        this.toastr.success('Diagnostico guardado con exito.', 'Guardado!');

        this.onBuscar();
        FormData.resetForm();
        this.modalRef.hide();
      },
      error => {
        this.toastr.error(error.message, 'Error!');
      });
    }
  }

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Diagnostico();
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.configLarge);
    } else {
      this._diagnosticosService.getDiagnostico(id)
    .subscribe(
      data => {
        this.model = data;

        if (this.model.Fecha) {
          this.model.Fecha = new Date(this.model.Fecha);
          this.model.SelectedDate = this.getDateStructFromDate(this.model.Fecha);
        }

        this.modalRef = this.modalService.show(template, this.configLarge);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getCombos() {
    this._diagnosticosService.getCombos()
      .subscribe(
        data => {
          this.pacientes = data.pacientes;
          this.optometristas = data.optometristas;
          this.tiposlente = data.tiposlente;
          this.materiales = data.materiales;
          this.coloreslente = data.coloreslente;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  onFechaFacturaChanged(selectedDate: NgbDateStruct) {
    this.model.Fecha = this.getDateFromDateStruct(selectedDate);
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

  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.Clave.toLocaleLowerCase().indexOf(term) > -1 || 
    item.Descripcion.toLocaleLowerCase().indexOf(term) > -1 || 
    (item.Clave + ' - ' + item.Descripcion).toLocaleLowerCase().indexOf(term) > -1;
  }

}
