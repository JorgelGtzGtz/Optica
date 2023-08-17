import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { I18n, CustomDatepickerI18n } from '../../directives/custom-datepickerI18n';
import { NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { UsersService, ProductosService } from 'src/app/services/service.index';
import { Producto } from '../../models/Producto';
import { ProductosDetalleKit } from '../../models/ProductosDetalleKit';

@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  modalRef: BsModalRef;
  modalListaKitsRef: BsModalRef;
  modalAgregarKitRef: BsModalRef;

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
  model: Producto = new Producto();
  kits: ProductosDetalleKit[] = [];
  modelKit: ProductosDetalleKit = new ProductosDetalleKit();
  marcas: any[] = [];
  modelos: any[] = [];
  colores: any[] = [];
  tiposlente: any[] = [];
  material: any[] = [];
  materialfilter: any[] = [];
  colorlente: any[] = [];
  productos: any[] = [];
  tipos = [{Id: '0', Name: 'Armazón'},
             {Id: '1', Name: 'Estuche'},
             {Id: '2', Name: 'Mica'},
             {Id: '3', Name: 'Limpieza'}];

  micaojo = [{Id: '0', Name: 'Izquiedo'},
             {Id: '1', Name: 'Derecho'}];

  micaesferavalues = [{Id: '0',Value: '-20'},
  {Id: '1',Value: '-19.75'},
  {Id: '2',Value: '-19.50'},
  {Id: '3',Value: '-19.25'},
  {Id: '4',Value: '-19'},
  {Id: '5',Value: '-18.75'},
  {Id: '6',Value: '-18.50'},
  {Id: '7',Value: '-18.25'},
  {Id: '8',Value: '-18'},
  {Id: '9',Value: '-17.75'},
  {Id: '10',Value: '-17.50'},
  {Id: '11',Value: '-17.25'},
  {Id: '12',Value: '-17'},
  {Id: '13',Value: '-16.75'},
  {Id: '14',Value: '-16.50'},
  {Id: '15',Value: '-16.25'},
  {Id: '16',Value: '-16'},
  {Id: '17',Value: '-15.75'},
  {Id: '18',Value: '-15.50'},
  {Id: '19',Value: '-15.25'},
  {Id: '20',Value: '-15'},
  {Id: '21',Value: '-14.75'},
  {Id: '22',Value: '-14.50'},
  {Id: '23',Value: '-14.25'},
  {Id: '24',Value: '-14'},
  {Id: '25',Value: '-13.75'},
  {Id: '26',Value: '-13.50'},
  {Id: '27',Value: '-13.25'},
  {Id: '28',Value: '-13'},
  {Id: '29',Value: '-12.75'},
  {Id: '30',Value: '-12.50'},
  {Id: '31',Value: '-12.25'},
  {Id: '32',Value: '-12'},
  {Id: '33',Value: '-11.75'},
  {Id: '34',Value: '-11.50'},
  {Id: '35',Value: '-11.25'},
  {Id: '36',Value: '-11'},
  {Id: '37',Value: '-10.75'},
  {Id: '38',Value: '-10.50'},
  {Id: '39',Value: '-10.25'},
  {Id: '40',Value: '-10'},
  {Id: '41',Value: '-9.75'},
  {Id: '42',Value: '-9.50'},
  {Id: '43',Value: '-9.25'},
  {Id: '44',Value: '-9'},
  {Id: '45',Value: '-8.75'},
  {Id: '46',Value: '-8.50'},
  {Id: '47',Value: '-8.25'},
  {Id: '48',Value: '-8'},
  {Id: '49',Value: '-7.75'},
  {Id: '50',Value: '-7.50'},
  {Id: '51',Value: '-7.25'},
  {Id: '52',Value: '-7'},
  {Id: '53',Value: '-6.75'},
  {Id: '54',Value: '-6.50'},
  {Id: '55',Value: '-6.25'},
  {Id: '56',Value: '-6'},
  {Id: '57',Value: '-5.75'},
  {Id: '58',Value: '-5.50'},
  {Id: '59',Value: '-5.25'},
  {Id: '60',Value: '-5'},
  {Id: '61',Value: '-4.75'},
  {Id: '62',Value: '-4.50'},
  {Id: '63',Value: '-4.25'},
  {Id: '64',Value: '-4'},
  {Id: '65',Value: '-3.75'},
  {Id: '66',Value: '-3.50'},
  {Id: '67',Value: '-3.25'},
  {Id: '68',Value: '-3'},
  {Id: '69',Value: '-2.75'},
  {Id: '70',Value: '-2.50'},
  {Id: '71',Value: '-2.25'},
  {Id: '72',Value: '-2'},
  {Id: '73',Value: '-1.75'},
  {Id: '74',Value: '-1.50'},
  {Id: '75',Value: '-1.25'},
  {Id: '76',Value: '-1'},
  {Id: '77',Value: '-0.75'},
  {Id: '78',Value: '-0.50'},
  {Id: '79',Value: '-0.25'},
  {Id: '80',Value: '0'},
  {Id: '81',Value: '0.25'},
  {Id: '82',Value: '0.50'},
  {Id: '83',Value: '0.75'},
  {Id: '84',Value: '1'},
  {Id: '85',Value: '1.25'},
  {Id: '86',Value: '1.50'},
  {Id: '87',Value: '1.75'},
  {Id: '88',Value: '2'},
  {Id: '89',Value: '2.25'},
  {Id: '90',Value: '2.50'},
  {Id: '91',Value: '2.75'},
  {Id: '92',Value: '3'},
  {Id: '93',Value: '3.25'},
  {Id: '94',Value: '3.50'},
  {Id: '95',Value: '3.75'},
  {Id: '96',Value: '4'},
  {Id: '97',Value: '4.25'},
  {Id: '98',Value: '4.50'},
  {Id: '99',Value: '4.75'},
  {Id: '100',Value: '5'},
  {Id: '101',Value: '5.25'},
  {Id: '102',Value: '5.50'},
  {Id: '103',Value: '5.75'},
  {Id: '104',Value: '6'},
  {Id: '105',Value: '6.25'},
  {Id: '106',Value: '6.50'},
  {Id: '107',Value: '6.75'},
  {Id: '108',Value: '7'},
  {Id: '109',Value: '7.25'},
  {Id: '110',Value: '7.50'},
  {Id: '111',Value: '7.75'},
  {Id: '112',Value: '8'},
  {Id: '113',Value: '8.25'},
  {Id: '114',Value: '8.50'},
  {Id: '115',Value: '8.75'},
  {Id: '116',Value: '9'},
  {Id: '117',Value: '9.25'},
  {Id: '118',Value: '9.50'},
  {Id: '119',Value: '9.75'},
  {Id: '120',Value: '10'},
  {Id: '121',Value: '10.25'},
  {Id: '122',Value: '10.50'},
  {Id: '123',Value: '10.75'},
  {Id: '124',Value: '11'},
  {Id: '125',Value: '11.25'},
  {Id: '126',Value: '11.50'},
  {Id: '127',Value: '11.75'},
  {Id: '128',Value: '12'},
  {Id: '129',Value: '12.25'},
  {Id: '130',Value: '12.50'},
  {Id: '131',Value: '12.75'},
  {Id: '132',Value: '13'},
  {Id: '133',Value: '13.25'},
  {Id: '134',Value: '13.50'},
  {Id: '135',Value: '13.75'},
  {Id: '136',Value: '14'},
  {Id: '137',Value: '14.25'},
  {Id: '138',Value: '14.50'},
  {Id: '139',Value: '14.75'},
  {Id: '140',Value: '15'},
  {Id: '141',Value: '15.25'},
  {Id: '142',Value: '15.50'},
  {Id: '143',Value: '15.75'},
  {Id: '144',Value: '16'},
  {Id: '145',Value: '16.25'},
  {Id: '146',Value: '16.50'},
  {Id: '147',Value: '16.75'},
  {Id: '148',Value: '17'},
  {Id: '149',Value: '17.25'},
  {Id: '150',Value: '17.50'},
  {Id: '151',Value: '17.75'},
  {Id: '152',Value: '18'},
  {Id: '153',Value: '18.25'},
  {Id: '154',Value: '18.50'},
  {Id: '155',Value: '18.75'},
  {Id: '156',Value: '19'},
  {Id: '157',Value: '19.25'},
  {Id: '158',Value: '19.50'},
  {Id: '159',Value: '19.75'},
  {Id: '160',Value: '20'},];

  micaadicionvalues = [{Id: '0',Value: '0'},
  {Id: '1',Value: '0.25'},
  {Id: '2',Value: '0.50'},
  {Id: '3',Value: '0.75'},
  {Id: '4',Value: '1'},
  {Id: '5',Value: '1.25'},
  {Id: '6',Value: '1.50'},
  {Id: '7',Value: '1.75'},
  {Id: '8',Value: '2'},
  {Id: '9',Value: '2.25'},
  {Id: '10',Value: '2.50'},
  {Id: '11',Value: '2.75'},
  {Id: '12',Value: '3'},
  {Id: '13',Value: '3.25'},
  {Id: '14',Value: '3.50'},
  {Id: '15',Value: '3.75'},
  {Id: '16',Value: '4'},];
  
  micacilindrovalues = [{Id: '0',Value: '-6'},
  {Id: '1',Value: '-5.75'},
  {Id: '2',Value: '-5.50'},
  {Id: '3',Value: '-5.25'},
  {Id: '4',Value: '-5'},
  {Id: '5',Value: '-4.75'},
  {Id: '6',Value: '-4.50'},
  {Id: '7',Value: '-4.25'},
  {Id: '8',Value: '-4'},
  {Id: '9',Value: '-3.75'},
  {Id: '10',Value: '-3.50'},
  {Id: '11',Value: '-3.25'},
  {Id: '12',Value: '-3'},
  {Id: '13',Value: '-2.75'},
  {Id: '14',Value: '-2.50'},
  {Id: '15',Value: '-2.25'},
  {Id: '16',Value: '-2'},
  {Id: '17',Value: '-1.75'},
  {Id: '18',Value: '-1.50'},
  {Id: '19',Value: '-1.25'},
  {Id: '20',Value: '-1'},
  {Id: '21',Value: '-0.75'},
  {Id: '22',Value: '-0.50'},
  {Id: '23',Value: '-0.25'},
  {Id: '24',Value: '0'},
  ];

  public imagePath;
  imgURL: any;
  public message: string;

  public toastconfig: any = { timeOut: 0, extendedTimeOut: 0, preventDuplicates: true, maxOpened: 1, autoDismiss: false };

  constructor(private _userService: UsersService, private _productoService: ProductosService, private modalService: BsModalService, private toastr: ToastrService) { 
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.imgURL = 'assets/images/default-upload.png';
    this.onBuscar();
    this.getCombos();
  }

  onBuscar() {
    this._productoService.getLista(this.modelFilter).subscribe(
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

      if (this.imgURL !== 'assets/images/default-upload.png' && this.imgURL !== this.model.Imagen) {
        this.model.Imagen = this.imgURL;
      }

      const data = {
        producto: this.model,
        productoskit: this.kits
      };

      this._productoService.guardar(data)
    .subscribe(
      success => {
        this.toastr.success('Producto guardado con exito.', 'Guardado!');
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
      text: 'Esta seguro que quiere eliminar producto, no se podra revertir!',
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
        this._productoService.eliminar(id)
        .subscribe(
          success => {
            this.onBuscar();
            Swal.fire({
              title: 'Eliminado!',
              text: 'Producto a sido eliminado con exito.',
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
    this.model = new Producto();
    this.imgURL = 'assets/images/default-upload.png';
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } else {
      this._productoService.getProducto(id)
    .subscribe(
      data => {
        this.model = data.producto;
        this.kits = data.kit;
        if (this.model.Imagen) {
            this.imgURL = this.model.Imagen;
        }
        if (this.model.Tipo === 'Armazón') {
          this.materialfilter = this.material.filter(
            (item) => item.Tipo === 'Armazón'
          );
        }
        this.modalRef = this.modalService.show(template, this.config);
      },
      error => this.toastr.error(error.message, 'Error!') );
    }
  }

  getCombos() {
    this._productoService.getCombos()
      .subscribe(
        data => {
          this.marcas = data.marcas;
          this.modelos = data.modelos;
          this.colores = data.colores;
          this.tiposlente = data.tipos;
          this.material = data.material;
          this.colorlente = data.colorlente;
          this.productos = data.productos;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  preview(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }

    const reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    // tslint:disable-next-line: variable-name
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    };
  }

  onShowKits(template: TemplateRef<any>) {
    this.modalListaKitsRef = this.modalService.show(template, this.config);
  }

  onShowDetalle(model: ProductosDetalleKit, template: TemplateRef<any>) {
    this.modelKit = new ProductosDetalleKit();
    if (model) {
      this.modelKit = model;
    }
    this.modalAgregarKitRef = this.modalService.show(template, this.configMd);
  }

  onDeleteDetalle(model: ProductosDetalleKit) {
    Swal.fire({
      title: 'Esta seguro?',
      text: 'Esta seguro que quiere eliminar componente, no se podra revertir!',
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
        const Index = this.kits.findIndex((Item: ProductosDetalleKit) => Item.ID === model.ID);
        if (Index !== -1) {
            this.kits.splice(Index, 1);
        }
        Swal.fire({
          title: 'Eliminado!',
          text: 'Componente a sido eliminado con exito.',
          type: 'success',
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }

  onProductoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }

    this.modelKit.Descripcion = selectedValue.Descripcion;
    this.modelKit.Clave = selectedValue.Clave;
  }

  onSubmitDetalle(FormData: any) {
    if (FormData.valid) {
      this.kits.push(this.modelKit);
      this.toastr.success('Producto agregado con exito.', 'Agregado!');
      this.modalAgregarKitRef.hide();
    }
  }

  onTipoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }

    if (selectedValue.Name !== 'Mica') {
      this.model.ID_TipoLente = null;
      this.model.ID_MaterialLente = null;
      this.model.ID_ColorLente = null;
      this.model.MicaOjo = '';
      this.model.MicaEsfera = '';
      this.model.MicaAdicion = '';
    }

    if (selectedValue.Name === 'Armazón') {
      this.materialfilter = this.material.filter(
        (item) => item.Tipo === 'Armazón'
      );
    }else{
      this.materialfilter = this.material.filter(
        (item) => item.Tipo === 'Mica'
      );
    }

  }
}
