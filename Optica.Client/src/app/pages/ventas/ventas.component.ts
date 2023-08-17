import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, VentasService } from 'src/app/services/service.index';
import { Venta } from '../../models/Venta';
import { VentaDetalle } from '../../models/VentaDetalle';

@Component({
  selector: 'app-ventas',
  templateUrl: './ventas.component.html',
  styleUrls: ['./ventas.component.css'],
  providers: [I18n, {provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n}]
})
export class VentasComponent implements OnInit {
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
  folioalt = '';
  idcliente: any;
  idsucursal: any;
  idvendedor: any;
  idproducto: any;

  lista: any[] = [];
  model: Venta = new Venta();
  tipo = 'N';
  modeldetalle: VentaDetalle = new VentaDetalle();
  detalles: VentaDetalle[] = [];
  detallesEdit = false;
  indexEdit = 0;

  productos: any[] = [];
  almacenes: any[] = [];
  sucursales: any[] = [];
  clientes: any[] = [];
  vendedores: any[] = [];
  metodospago: any[] = [];

  iva: number;
  total: number;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _ventasService: VentasService, private modalService: BsModalService, private toastr: ToastrService) { 
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

    this._ventasService.getLista(from, to, this.idcliente, this.idsucursal, this.idvendedor, this.idproducto, this.folio, this.folioalt).subscribe(
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

  async onSubmit(FormData) {
    if (FormData.valid) {
      let accion = 'G';
      if (this.model.Estatus === 'P') {
        const { value: formValues } = await Swal.fire({
          title: 'Autorización',
          html:
            'Usuario: <br> <input id="swal-input1" class="swal2-input">' +
            'Contraseña: <br> <input type="password" id="swal-input2" class="swal2-input">',
          focusConfirm: false,
          preConfirm: () => {
            return [
              (<any>document.getElementById('swal-input1')).value,
              (<any>document.getElementById('swal-input2')).value
            ];
          }
        });

        if (formValues) {
          let us = formValues[0];
          let pass = formValues[1];
          
          const modelAut = {
            usuario: us,
            password: pass
          };

          this._ventasService.autorizar(modelAut)
          .subscribe(
            success => {
              const model = {
                model: this.model,
                detalles: this.detalles,
                accion,
              };

              this._ventasService.guardar(model)
              .subscribe(
                success => {
                  if (this.model.Estatus === 'P') {
                    this.toastr.success('Venta guardada con exito. Reprocese para reflejar los cambios.', 'Guardado!');
                  } else {
                    this.toastr.success('Venta guardada con exito.', 'Guardado!');
                  }
                  this.onBuscar();
                  FormData.resetForm();
                  this.modalRef.hide();
                },
                error => {
                  this.toastr.error(error.message, 'Error!');
                });
            },
            error => {
              this.toastr.error(error.message, 'Error!');
            });
        }
      } else {
        Swal.fire({
          title: 'Que accion desea tomar?',
          type: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#21A300',
          confirmButtonText: 'Procesar',
          cancelButtonText: 'Guardar',
          focusConfirm: false,
          focusCancel: false,
          allowEnterKey: false
        }).then((result) => {

          if (result.value) {
            accion = 'P';
          } else {
            accion = 'G';
          }

          const model = {
            model: this.model,
            detalles: this.detalles,
            accion,
          };

          this._ventasService.guardar(model)
        .subscribe(
          success => {
            if (this.model.Estatus === 'P') {
              this.toastr.success('Venta guardada con exito. Reprocese para reflejar los cambios.', 'Guardado!');
            } else {
              this.toastr.success('Venta guardada con exito.', 'Guardado!');
            }
            this.onBuscar();
            FormData.resetForm();
            this.modalRef.hide();
          },
          error => {
            this.toastr.error(error.message, 'Error!');
          });

        });
      }
    }
  }

  onShow(id: number, template: TemplateRef<any>, tipo: string) {
    this.model = new Venta();
    this.detalles = [];
    this.tipo = tipo;
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.configLarge);
    } else {
      this._ventasService.getVenta(id)
    .subscribe(
      data => {
        this.model = data.model;
        this.detalles = data.detalles;

        if (this.model.Fecha) {
          this.model.Fecha = new Date(this.model.Fecha);
          this.model.SelectedDate = this.getDateStructFromDate(this.model.Fecha);
        }

        this.modalRef = this.modalService.show(template, this.configLarge);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  onProcesar(id: number) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere procesar venta, no se podra revertir!',
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
        this._ventasService.procesar(id)
        .subscribe(
          success => {
            Swal.fire({
              title: 'Procesada!',
              text: 'Venta a sido procesada con exito.',
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

  async onCancelar(modelDetalle: any) {
    const { value: formValues } = await Swal.fire({
      title: 'Autorización',
      html:
        'Usuario: <br> <input id="swal-input1" class="swal2-input">' +
        'Contraseña: <br> <input type="password" id="swal-input2" class="swal2-input">',
      focusConfirm: false,
      preConfirm: () => {
        return [
          (<any>document.getElementById('swal-input1')).value,
          (<any>document.getElementById('swal-input2')).value
        ];
      }
    });

    if (formValues) {
      let us = formValues[0];
      let pass = formValues[1];

      const modelAut = {
        usuario: us,
        password: pass
      };

      this._ventasService.autorizar(modelAut)
      .subscribe(
        success => {
          Swal.fire({
            title: 'Esta seguro?',
            text: 'Esta seguro que quiere cancelar Venta, no se podra revertir!',
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
      
              this._ventasService.cancelar(modelDetalle.ID)
              .subscribe(
                success => {
                  if (modelDetalle.Estatus === 'P') {
                    Swal.fire({
                      title: 'Cancelada!',
                      text: 'Venta a sido cancelada con exito. Reprocese para reflejar los cambios.',
                      type: 'success',
                      confirmButtonText: 'Aceptar'
                    });
                  } else {
                    Swal.fire({
                      title: 'Cancelada!',
                      text: 'Venta a sido cancelada con exito.',
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
        },
        error => {
          this.toastr.error(error.message, 'Error!');
        });
    }
  }

  onImprimir(id: number) {
  }

  getCombos() {
    this._ventasService.getCombos()
      .subscribe(
        data => {
          this.productos = data.productos;
          this.almacenes = data.almacenes;
          this.sucursales = data.sucursales;
          this.clientes = data.clientes;
          this.vendedores = data.vendedores;
          this.metodospago = data.metodospago;
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

  onShowDetalle(model: VentaDetalle, template: TemplateRef<any>, index?: number) {
    this.modeldetalle = new VentaDetalle();
    if (model) {
      this.detallesEdit = true;
      this.indexEdit = index;
      this.modeldetalle = JSON.parse(JSON.stringify(model)) as any;
    } else {
      this.detallesEdit = true;
    }

    this.modaldetalleRef = this.modalService.show(template, this.configMd);
  }

  onDeleteDetalle(model: VentaDetalle) {
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
        const Index = this.detalles.findIndex((Item: VentaDetalle) => Item.ID === model.ID);
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
      subtotal += element.Cantidad * element.Costo;
    }

    this.iva = (subtotal * 0.16);
    this.total = subtotal + this.iva;

    this.model.Subtotal = subtotal;
    this.model.Iva = this.iva;
    this.model.Total = this.total;

    return subtotal;
  }

  OnUpdateTotal() {
      this.modeldetalle.CostoTotal = (this.modeldetalle.Cantidad * this.modeldetalle.Costo);
      this.modeldetalle.PrecioTotal = (this.modeldetalle.Cantidad * this.modeldetalle.Precio);
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
    this.modeldetalle.Precio = selectedValue.Precio;
    this.OnUpdateTotal();
  }

  onAlmacenChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }

    this.model.ID_Sucursal = selectedValue.ID_Sucursal;
  }

  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.Clave.toLocaleLowerCase().indexOf(term) > -1 || 
    item.Descripcion.toLocaleLowerCase().indexOf(term) > -1 || 
    (item.Clave + ' - ' + item.Descripcion).toLocaleLowerCase().indexOf(term) > -1;
  }

}
