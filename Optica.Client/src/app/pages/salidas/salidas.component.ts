import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { SalidaService } from 'src/app/services/salidas/salidas..service';
import { Salida } from 'src/app/models/Salida';
import { Entrada } from 'src/app/models/Entrada';
import { DetalleEntrada } from 'src/app/models/DetalleEntrada';

@Component({
  selector: 'app-salidas',
  templateUrl: './salidas.component.html',
  styleUrls: ['./salidas.component.css']
})
export class SalidasComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };
  salidas: any[] = [];
  cmbProductos: any[] = [];
  cmbAlmacenes: any[] = [];
  cmbTipoEntradaSalida: any[] = [];
  modalRefProd: BsModalRef;
  model: Entrada = new Entrada();
  modelProducto: DetalleEntrada = new DetalleEntrada();
  productos: any[] = [];
  modelFecha = {
    Fecha: ''
  }
  
  constructor(private modalService: BsModalService, private _salidaService: SalidaService, private toastr: ToastrService) { }


  ngOnInit() {
    this.getCombos();
    this.onBuscar();
  }

  onBuscar() {
    this._salidaService.getLista().
    subscribe(
      (data: any) => {
        this.salidas = data;
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

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Entrada();
    this.productos = [];
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._salidaService.getSalida(id)
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

  onSubmit(FormData){
    if (FormData.valid){
      const entrada = {
        ID_Almacen: this.model.ID_Almacen,
        ID_EntradaSalida: 2,
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

      this._salidaService.guardar(data).
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
        this._salidaService.procesar(id)
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

  onSubmitDetalle(FormData: any) {
    if (FormData.valid) {
      this.productos.push(this.modelProducto);
      this.toastr.success('Producto agregado con exito.', 'Agregado!');
      this.modalRefProd.hide();
    }
  }

  onShowNewProduct(template: TemplateRef<any>){
    this.modelProducto = new DetalleEntrada();
    this.modalRefProd = this.modalService.show(template, this.config);
  }

  getCombos() {
    this._salidaService.getCombos()
      .subscribe(
        data => {
          this.cmbProductos = data.productos;
          this.cmbAlmacenes = data.almacenes;
          this.cmbTipoEntradaSalida = data.tipoEntradaSalida;
        },
        error => this.toastr.error(error.message, 'Error!') );
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
