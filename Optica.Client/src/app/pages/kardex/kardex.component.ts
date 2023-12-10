import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Kardex } from 'src/app/models/Kardex';
import { InventariosService } from 'src/app/services/inventarios/inventarios.service';

@Component({
  selector: 'app-kardex',
  templateUrl: './kardex.component.html',
  styleUrls: ['./kardex.component.css']
})
export class KardexComponent implements OnInit {

  productos: any[] = [];
  almacenes: any[] = [];
  inventario: any[] = [];
  model: Kardex = new Kardex();

  constructor(private _inventarioService: InventariosService, private toastr: ToastrService) { }


  ngOnInit() {
    this.getCombos();
  }

  getCombos() {
    this._inventarioService.getCombos()
      .subscribe(
        data => {
          console.log(data)
          this.productos = data.productos;
          this.almacenes = data.almacenes;
        },
        error => this.toastr.error(error.message, 'Error!') );
  }

  showKardex(evt){

    this._inventarioService.getKardexProducto(this.model.producto, evt)
    .subscribe(
      data=> {
        this.inventario = data
      }
    );
  }

}
