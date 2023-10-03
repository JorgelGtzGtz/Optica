import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Inventario } from 'src/app/models/Inventario';
import { ItemSelectProducto } from 'src/app/models/ItemSelectProduct';
import { InventariosService } from 'src/app/services/inventarios/inventarios.service';

@Component({
  selector: 'app-inventario',
  templateUrl: './inventario.component.html',
  styleUrls: ['./inventario.component.css']
})

export class InventarioComponent implements OnInit {
  productos: any[] = [];
  model: Inventario = new Inventario();
  almacenes: any[] = [];
  inventario: any[] = [];
  productoSelect: boolean = false;
  itemProductselect: ItemSelectProducto = new ItemSelectProducto();
  constructor(private _inventarioService: InventariosService, private toastr: ToastrService) { }
  

  ngOnInit() {
    this.getCombos();
  }

  onDetalles() {
    const model = {
      id: this.model.ID_Producto,
    };
    this._inventarioService.getLista(model)
    .subscribe(
      (data: any) => {
        console.log(data)
        this.inventario = data;
        this.productoSelect = true;
        this.getProducto(this.inventario[0].ID_Producto);
      },
      error => {
        this.toastr.error(error.message, 'Error!');
      });
  }

  getProducto(id:any){
    this._inventarioService.getProducto(id).
    subscribe(
      (data: any) => {
        console.log(data);
        this.itemProductselect.Nombre = data.producto.Descripcion;
        this.itemProductselect.Existencia = data.producto.Cantidad;
        this.itemProductselect.Disponible = data.producto.Disponible;
        this.itemProductselect.CostoPromedio = data.producto.Costo;
        this.itemProductselect.CostoUltimo = data.producto.UltimoCosto;
        this.itemProductselect.Precio = data.producto.Precio;
        this.itemProductselect.IVA = data.producto.Iva;
        console.log(this.itemProductselect);
      }
    )
  }
  
  getCombos() {
    this._inventarioService.getCombos()
      .subscribe(
        data => {
          this.productos = data.productos;
          this.almacenes = data.almacenes;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }
  onProductoChanged(selectedValue: any) {
    if (!selectedValue) {
      return;
    }
    if (this.model.ID_Producto > 0) {
        this.onDetalles();
    } else {
      this.inventario = [];
    }
  }
}
