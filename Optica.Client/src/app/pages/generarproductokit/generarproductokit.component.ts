import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, GenerarProductoKitService } from 'src/app/services/service.index';

import { GenerarKitDto } from '../../models/GenerarKitDto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-generarproductokit',
  templateUrl: './generarproductokit.component.html',
  styleUrls: ['./generarproductokit.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class GenerarProductoKitComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  modaldetalleRef: BsModalRef;
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


  model: GenerarKitDto = new GenerarKitDto();
  detalles: any[] = [];
  detallesEdit = false;
  indexEdit = 0;

  proveedores: any[] = [];
  productos: any[] = [];
  almacenes: any[] = [];
  productosProveedor: any[] = [];

  iva: number;
  total: number;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private router: Router, private _userService: UsersService, private _generarKitService: GenerarProductoKitService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.getCombos();
  }

  onDetalles() {
    const model = {
      idalmacen: this.model.ID_Almacen,
      idproductobase: this.model.ID_Producto,
      cantidad: this.model.Cantidad,
    };

    this._generarKitService.getLista(model)
    .subscribe(
      (data: any) => {
        this.detalles = data;
      },
      error => {
        this.toastr.error(error.message, 'Error!');
      });
  }

  onGenerar(FormData) {
    if (FormData.valid) {
      Swal.fire({
        title: 'Esta seguro?',
        text: 'Esta seguro que quiere procesar compra, no se podra revertir!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, Procesar!',
        focusConfirm: false,
        focusCancel: false,
        allowEnterKey: false
      }).then((result) => {
        if (result.value) {
          this._generarKitService.generar(this.model)
          .subscribe(
            success => {
              Swal.fire({
                title: 'Generado!',
                text: 'Producto kit a sido generado con exito.',
                type: 'success',
                confirmButtonText: 'Aceptar'
              });
              this.model = new GenerarKitDto();
              this.detalles = [];
            },
            error => {
              this.toastr.error(error.message, 'Error!', {
                timeOut: 3000
              });
            });
        }
      });
    }
  }

  getCombos() {
    this._generarKitService.getCombos()
      .subscribe(
        data => {
          this.productos = data.productos;
          this.almacenes = data.almacenes;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  onFechaChanged(selectedDate: NgbDateStruct) {
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

  onGetSubTotal() {
    let subtotal = 0;
    // tslint:disable-next-line: prefer-for-of
    for (let index = 0; index < this.detalles.length; index++) {
      const element = this.detalles[index];
      subtotal += element.Total;
    }

    return subtotal;
  }

  OnUpdateTotal() {
    if (this.model.ID_Producto > 0 && this.model.ID_Almacen > 0 && this.model.Cantidad > 0) {
        this.onDetalles();
    } else {
      this.detalles = [];
    }
  }

  onProductoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }
    if (this.model.ID_Producto > 0 && this.model.ID_Almacen > 0 && this.model.Cantidad > 0) {
        this.onDetalles();
    } else {
      this.detalles = [];
    }
  }

  onCerrar() {
    this.router.navigate(['/dashboard']);
  }

  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.Clave.toLocaleLowerCase().indexOf(term) > -1 || 
    item.Descripcion.toLocaleLowerCase().indexOf(term) > -1 || 
    (item.Clave + ' - ' + item.Descripcion).toLocaleLowerCase().indexOf(term) > -1;
  }

}
