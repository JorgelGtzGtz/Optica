import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Entrada } from 'src/app/models/Entrada';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import { EntradasService } from 'src/app/services/entradas/entradas.service';
import Swal from 'sweetalert2';
import { DetalleEntrada } from 'src/app/models/DetalleEntrada';

@Component({
  selector: 'app-entradas',
  templateUrl: './entradas.component.html',
  styleUrls: ['./entradas.component.css']
})
export class EntradasComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  productos: any[] = [];
  cmbProductos: any[] = [];
  modalRefProd: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };
  model: Entrada = new Entrada();
  modelProducto: DetalleEntrada = new DetalleEntrada();
  lista: any[] = [];
  detalles: any[] = [];
  movimientos: any[] = ["Entrada por cancelacion", "Entrada por devolucion", "Salida por venta punto venta"];
  cmbAlmacenes: any[] = [];
  cmbTipoEntradaSalida: any[] = [];
  movimiento: any;
  entradas: any[] = [];
  modelFecha = {
    Fecha: ''
  }

  constructor(private modalService: BsModalService, private _entradaService: EntradasService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getCombos();
    this.onBuscar();

  }

  onBuscar() {
    this._entradaService.getLista().
    subscribe(
      (data: any) => {
        this.entradas = data;
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
        this._entradaService.procesar(id)
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

  onDelete(id: number) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere eliminar entrada, no se podra revertir!',
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
        this._entradaService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Entrada a sido eliminado con exito.',
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

  getCombos() {
    this._entradaService.getCombos()
      .subscribe(
        data => {
          this.cmbProductos = data.productos;
          this.cmbAlmacenes = data.almacenes;
          this.cmbTipoEntradaSalida = data.tipoEntradaSalida;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  onSubmit(FormData){
    if (FormData.valid){
      const entrada = {
        ID_Almacen: this.model.ID_Almacen,
        ID_EntradaSalida: 1,
        ID_TipoEntradaSalida: this.model.ID_TipoEntradaSalida,
        Referencia: "",
        Total: 0,
        Observaciones: this.model.Observaciones,
        Fecha:this.modelFecha.Fecha,
        Estatus: "G"
      }
      const data = {
        data: entrada,
        detalles: this.productos
      }
      console.log(data)

      this._entradaService.guardar(data).
      subscribe(
        success => {
          this.toastr.success('Producto guardado con exito.', 'Guardado!');
          FormData.resetForm();
          this.modalRef.hide();

        },
        error => {
          this.toastr.error(error.message, 'Error!');
        });
    }
  }

  

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Entrada();
    this.productos = [];
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._entradaService.getEntrada(id)
    .subscribe(
      data => {
        console.log(data)
        this.model = data.model;
        const fechaDividida = data.model.Fecha.split('T')[0];
        this.modelFecha.Fecha = fechaDividida;
        this.productos = data.detalles;
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  onShowNewProduct(template: TemplateRef<any>){
    this.modelProducto = new DetalleEntrada();
    this.modalRefProd = this.modalService.show(template, this.config);
  }

  onSubmitDetalle(FormData: any) {
    if (FormData.valid) {
      this.productos.push(this.modelProducto);
      this.toastr.success('Producto agregado con exito.', 'Agregado!');
      this.modalRefProd.hide();
    }
  }
  
  onCantidadChanged(event: any){
    this.modelProducto.CostoTotal = this.modelProducto.Costo * this.modelProducto.Cantidad;
  }

  onProductoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }
    this.modelProducto.Costo = (selectedValue.Costo) ? selectedValue.Costo : 0;
    this.modelProducto.Descripcion = selectedValue.Descripcion;
    this.modelProducto.ID_Producto = selectedValue.ID;
  }

}
