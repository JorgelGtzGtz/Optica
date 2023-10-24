import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import { Contrato } from 'src/app/models/Contrato';
import { ContratosService } from 'src/app/services/contratos/contratos.service';
import { ContratoDetalle } from 'src/app/models/ContratoDetalle';
import { throwError } from 'rxjs';
import { UsersService } from 'src/app/services/service.index';

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
  gastoCobroDisabled: boolean = false;
  clienteDisabled:boolean = true;
  diaSemanaDisabled:boolean = true;
  productoSelected: any;
  detalles: any[] = [];
  isChecked: boolean = true;
  frecuenciasPagos: any[] = ["Mensual", "Quincenal", "Catorcenal", "Semanal"]
  DiasSemana: any[] = ["Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"]
  frecuanciaSelect: string;
  diaSemanaSelect: string;
  total: number = 0;
  corrido: any[] = [];
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };

  constructor(private modalService: BsModalService, private _userService:UsersService, private toastr: ToastrService, private _contratoService: ContratosService) { }


  ngOnInit() {
    this.getCombos();
  }

  onSubmit(FormData) {
    console.log(this.model)
    if (FormData.valid) {
      if (this.corrido.length > 0){
        this.model.Restante = this.model.ImportePago;
        this.model.ocupaAnticipo = this.anticipoDisabled;
        this.model.ocupaCE = this.contraEntregaDisabled;
        this.model.gastoCob = this.gastoCobroDisabled;
        this.model.ID_UsuarioCreacion = this._userService.user.ID;

        const data = {
          data: this.model,
          detalles: this.corrido
        }
        console.log(data)

        this._contratoService.guardar(data)
        .subscribe(
          () =>{
            this.toastr.success('Contrato guardado con exito.', 'Guardado!');
            this.detalles = [];
            this.corrido = [];
            FormData.resetForm();
            this.model = new Contrato();
            this.setFechaActual();
            this.getID();
          }
        );
      }else{
        this.toastr.error('Calcular pagos.', 'Haga el calculo de pagos!');

      }
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
      this.calcularTotal();
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
    this.setFechaActual();
    this.getID();
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

  getID(){
    this._contratoService.getLastContrato()
    .subscribe(
      data => {
        console.log(data)
        this.model.ID = data+1;
      }
    );
  }

  onFrecuenciaChange() {
    this.model.Frecuencia = this.frecuenciasPagos.indexOf(this.frecuanciaSelect) + 1;
    if (this.model.Frecuencia == 1){
      this.diaSemanaDisabled = false;
    }else{
      this.diaSemanaDisabled = true;
      this.model.DiaCobro = 0;

    }
  }

  onDiaSemanaChange(){
    this.model.DiaCobro = this.DiasSemana.indexOf(this.diaSemanaSelect) + 1;
  }

  limitInput(event: any) {
    if (event.target.value <= 0) {
      event.target.value = 1;
      this.model.Plazo = 1; 
    }
  }

  calcularTotal(){
    this.model.Total = 0;
    this.detalles.forEach(detalle => {
      this.model.Total += detalle.PCredito;
    })
    this.model.ImportePrestamo = this.model.Total - this.model.Anticipo - this.model.Contraentrega
    this.model.ImportePago = this.model.ImportePrestamo / this.model.Plazo
  }

  calcularCorrido(FormData){

    const pagos = {
      Mensual: "Mes",
      Catorcenal: "Catorcena",
      Quincenal: "Quincena",
      Semanal: "Semana"
    }

    if (true) {
      if (this.detalles.length > 0){
        this.corrido = [];
        let saldo = this.model.ImportePrestamo;
        let fechaInicial = new Date(this.model.FechaInicial);
        for (let i = 0; i < this.model.Plazo; i++) {
          const fecha = new Date(fechaInicial);
          const anio = fecha.getFullYear();
          const mes = String(fecha.getMonth() + 1).padStart(2, '0');
          const dia = String(fecha.getDate()+1).padStart(2, '0');
          const fechaFormateada = `${anio}-${mes}-${dia}`;
          const model = {
            Fecha: fechaFormateada,
            Descripcion: pagos[this.frecuanciaSelect] + " - " + (i+1),
            Importe: this.model.ImportePago,
            Saldo: saldo
          }
          this.corrido.push(model);
          if (this.frecuanciaSelect == "Mensual"){
            fechaInicial.setMonth(fechaInicial.getMonth() + 1);
          } else if (this.frecuanciaSelect == "Quincenal"){
            fechaInicial.setDate(fechaInicial.getDate() + 15);
          } else if (this.frecuanciaSelect == "Catorcenal"){
            fechaInicial.setDate(fechaInicial.getDate() + 14);
          } else if (this.frecuanciaSelect == "Semanal"){
            fechaInicial.setDate(fechaInicial.getDate() + 7);
          }
          saldo -= this.model.ImportePago;
        }
        console.log(this.corrido)
      }else{
        this.toastr.error('Agregue detalles.', 'Agregue almenos un producto!');
      }
    }else{
      this.toastr.error('Formulario incompleto.', 'Rellenar todos los campos!');
    }
  }

  setFechaActual() {
    let fechaActual;
    const hoy = new Date();
    const año = hoy.getFullYear();
    const mes = (hoy.getMonth() + 1).toString().padStart(2, '0');
    const día = hoy.getDate().toString().padStart(2, '0');
    fechaActual = `${año}-${mes}-${día}`;
    this.model.Fecha = fechaActual;
  }

}
