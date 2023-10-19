import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import { Contrato } from 'src/app/models/Contrato';
import { ContratosService } from 'src/app/services/contratos/contratos.service';
import { ContratoDetalle } from 'src/app/models/ContratoDetalle';


@Component({
  selector: 'app-contratos',
  templateUrl: './contratos.component.html',
  styleUrls: ['./contratos.component.css']
})
export class ContratosComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  model: Contrato = new Contrato();
  modelProducto: ContratoDetalle = new ContratoDetalle();
  modalRefProd: BsModalRef;
  modalRef: BsModalRef;
  productos: any[] = [];
  sucursales: any[] = [];
  clientes: any[] = [];
  diagnosticos: any[] = [];
  pacientes: any[] = [];
  diagnosticoDisabled: boolean = true;
  anticipoDisabled: boolean = false;
  contraEntregaDisabled: boolean = false;
  clienteDisabled:boolean = true;
  productoSelected: any;
  detalles: any[] = [];
  isChecked: boolean = true;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };

  constructor(private modalService: BsModalService, private toastr: ToastrService, private _contratoService: ContratosService) { }


  ngOnInit() {
    this.getCombos();
  }

  onSubmit(FormData) {
    if (FormData.valid) {
      console.log(this.model);
    }
  }

  onSubmitDetalle(FormData){
    if (FormData.valid){

      this._contratoService.getProducto(this.modelProducto.ID_Producto)
      .subscribe(
        data => {
          this.modelProducto.Clave = data.producto.Clave
          this.modelProducto.Descripcion = data.producto.Descripcion
        }
      );

      this._contratoService.getPaciente(this.modelProducto.ID_Paciente)
      .subscribe(
        data => {
          this.modelProducto.Paciente = data.paciente.Nombre
        }
      );
      
      this.detalles.push(this.modelProducto);
      this.toastr.success('Producto agregado con exito.', 'Agregado!');
      this.modalRefProd.hide();
      console.log(this.detalles)
    }
  }

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Contrato();
    this.productos = [];
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } 
  }

  newPaciente(value:any){
    this._contratoService.getDiagnosticos(value)
    .subscribe(
      data => {
        this.diagnosticos = data.diagnosticos;
        this.diagnosticoDisabled = false;
      }
    );
  }

  newProducto(value:any){
    this._contratoService.getProducto(value)
    .subscribe(
      data => {
        this.modelProducto.PContado = data.producto.Precio
        this.modelProducto.PCredito = data.producto.PrecioCred
      }
    );
  }

  onShowNewProduct(template: TemplateRef<any>){
    this._contratoService.getPacientes(this.model.ID_Cliente)
    .subscribe(
      data => {
        this.pacientes = data.pacientes;
      }
    );
    if (this.model.ID_Cliente){
      this.modelProducto = new ContratoDetalle();
      this.modalRefProd = this.modalService.show(template, this.config);
    }else{
      //mostrar mensaje de error 
    }
  }
  
  getCombos() {
    this._contratoService.getCombos()
      .subscribe(
        data => {
          console.log(data)
          this.sucursales = data.sucursales;
          this.clientes = data.clientes;
          this.productos = data.productos;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  toggleContraEntrega() {
    this.contraEntregaDisabled = !this.contraEntregaDisabled;
  }

  toggleAnticipo() {
    this.anticipoDisabled = !this.anticipoDisabled;
  }

}
