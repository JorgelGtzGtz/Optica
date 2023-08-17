import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, ComprasService } from 'src/app/services/service.index';
import { Compra } from '../../models/Compra';
import { CompraDetalle } from '../../models/CompraDetalle';

@Component({
  selector: 'app-compras',
  templateUrl: './compras.component.html',
  styleUrls: ['./compras.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class ComprasComponent implements OnInit {
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

  from: NgbDateStruct;
  to: NgbDateStruct;
  folio = '';
  factura = '';
  proveedor: any;

  lista: any[] = [];
  model: Compra = new Compra();
  modeldetalle: CompraDetalle = new CompraDetalle();
  detalles: CompraDetalle[] = [];
  detallesEdit = false;
  indexEdit = 0;

  proveedores: any[] = [];
  productos: any[] = [];
  almacenes: any[] = [];
  productosProveedor: any[] = [];

  iva: number;
  total: number;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _comprasService: ComprasService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    // this.from = {
    //   day: new Date().getDate(),
    //   month: new Date().getMonth() + 1,
    //   year: new Date().getFullYear()
    // };
    // this.to = {
    //   day: new Date().getDate(),
    //   month: new Date().getMonth() + 1,
    //   year: new Date().getFullYear()
    // };
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

    this._comprasService.getLista(from, to, this.proveedor, this.folio, this.factura).subscribe(
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
      const model = {
        model: this.model,
        detalles: this.detalles
      };

      this._comprasService.guardar(model)
    .subscribe(
      success => {
        if (this.model.Estatus === 'P') {
          this.toastr.success('Compra guardada con exito. Reprocese para reflejar los cambios.', 'Guardado!');
        } else {
          this.toastr.success('Compra guardada con exito.', 'Guardado!');
        }

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
    this.model = new Compra();
    this.detalles = [];
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.configLarge);
    } else {
      this._comprasService.getCompra(id)
    .subscribe(
      data => {
        this.model = data.model;
        this.detalles = data.detalles;

        if (this.model.FechaFactura) {
          this.model.FechaFactura = new Date(this.model.FechaFactura);
          this.model.SelectedDate = this.getDateStructFromDate(this.model.FechaFactura);
        }

        this.modalRef = this.modalService.show(template, this.configLarge);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  onProcesar(id: number) {
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
        this._comprasService.procesar(id)
        .subscribe(
          success => {
            Swal.fire({
              title: 'Procesada!',
              text: 'Compra a sido procesada con exito.',
              type: 'success',
              confirmButtonText: 'Aceptar'
            });
            this.onBuscar();
          },
          error => {
            this.toastr.error(error.message, 'Error!');
          });
      }
    });
  }

  onCancelar(modelDetalle: any) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere cancelar compra, no se podra revertir!',
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
        const model = {
          id: modelDetalle.ID
        };

        this._comprasService.cancelar(modelDetalle.ID)
        .subscribe(
          success => {
            if (modelDetalle.Estatus === 'P') {
              Swal.fire({
                title: 'Cancelada!',
                text: 'Compra a sido cancelada con exito. Reprocese para reflejar los cambios.',
                type: 'success',
                confirmButtonText: 'Aceptar'
              });
            } else {
              Swal.fire({
                title: 'Cancelada!',
                text: 'Compra a sido cancelada con exito.',
                type: 'success',
                confirmButtonText: 'Aceptar'
              });
            }
            this.onBuscar();
          },
          error => {
            this.toastr.error(error.message, 'Error!');
          });
      }
    });
  }

  onImprimir(id: number) {
  }

  getCombos() {
    this._comprasService.getCombos()
      .subscribe(
        data => {
          this.productos = data.productos;
          this.proveedores = data.proveedores;
          this.almacenes = data.almacenes;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  onFechaFacturaChanged(selectedDate: NgbDateStruct) {
    this.model.FechaFactura = this.getDateFromDateStruct(selectedDate);
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

  onShowDetalle(model: CompraDetalle, template: TemplateRef<any>, index?: number) {
    this.modeldetalle = new CompraDetalle();
    if (model) {
      this.detallesEdit = true;
      this.indexEdit = index;
      this.modeldetalle = JSON.parse(JSON.stringify(model)) as any;
    } else {
      this.detallesEdit = true;
    }

    this.modaldetalleRef = this.modalService.show(template, this.configMd);
  }

  onDeleteDetalle(model: CompraDetalle) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere eliminar detalle, no se podra revertir!',
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
        const Index = this.detalles.findIndex((Item: CompraDetalle) => Item.ID === model.ID);
        if (Index !== -1) {
            this.detalles.splice(Index, 1);
        }
        Swal.fire({
          title: 'Eliminado!',
          text: 'Detalle a sido eliminado con exito.',
          type: 'success',
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }

  onGetSubTotal() {
    let subtotal = 0;
    // tslint:disable-next-line: prefer-for-of
    for (let index = 0; index < this.detalles.length; index++) {
      const element = this.detalles[index];
      subtotal += element.Cantidad * element.CostoConDescuento;
    }

    this.iva = (subtotal * 0.16);
    this.total = subtotal + this.iva;

    this.model.Subtotal = subtotal;
    this.model.Iva = this.iva;
    this.model.Total = this.total;

    return subtotal;
  }

  OnUpdateTotal() {
    if (this.model.Descuento > 0) {
        let total = (this.modeldetalle.Cantidad * this.modeldetalle.Costo);
        let tmpCostoDescuento = this.modeldetalle.Costo * this.model.Descuento / 100;

        this.modeldetalle.CostoSinDescuento = this.modeldetalle.Costo;
        this.modeldetalle.CostoConDescuento = this.modeldetalle.Costo - tmpCostoDescuento;

        this.modeldetalle.Total = (this.modeldetalle.Cantidad * this.modeldetalle.Costo) - ((this.model.Descuento * total) / 100);
    } else {
      this.modeldetalle.CostoSinDescuento = this.modeldetalle.Costo;
      this.modeldetalle.CostoConDescuento = this.modeldetalle.Costo;
      this.modeldetalle.Total = (this.modeldetalle.Cantidad * this.modeldetalle.Costo);
    }
  }

  onSubmitDetalle(FormData: any) {
    if (FormData.valid) {
      if (this.detallesEdit) {
        this.detalles[this.indexEdit] = JSON.parse(JSON.stringify(this.modeldetalle)) as any;
        this.toastr.success('Producto actualizado con exito.', 'Agregado!');
      } else {
        this.detalles.push(this.modeldetalle);
        this.toastr.success('Producto agregado con exito.', 'Agregado!');
      }

      this.modaldetalleRef.hide();
    }
  }

  onProductoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }
    this.modeldetalle.Clave = selectedValue.Clave;
    this.modeldetalle.Descripcion = selectedValue.Descripcion;
    this.modeldetalle.Costo = selectedValue.Costo;
    this.OnUpdateTotal();
  }

  onProveedorChanged(selectedValue: any) {
    if (!selectedValue) {
      this.productosProveedor = [];
      return;
    }
    this.productosProveedor = [];

    this.productosProveedor = <any>JSON.parse(JSON.stringify(this.productos));
  }

  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.Clave.toLocaleLowerCase().indexOf(term) > -1 || 
    item.Descripcion.toLocaleLowerCase().indexOf(term) > -1 || 
    (item.Clave + ' - ' + item.Descripcion).toLocaleLowerCase().indexOf(term) > -1;
  }

}
