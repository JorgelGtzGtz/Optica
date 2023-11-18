import { Component, OnInit } from '@angular/core';
import { ContratosService } from 'src/app/services/contratos/contratos.service';

@Component({
  selector: 'app-list-contratos',
  templateUrl: './list-contratos.component.html',
  styleUrls: ['./list-contratos.component.css']
})
export class ListContratosComponent implements OnInit {

  constructor(private _contratoService: ContratosService) { }

 ngOnInit() {
    
    this._contratoService.getCombos().
    subscribe(
      data => {
        console.log(data)
      }
    );;
  }

}
