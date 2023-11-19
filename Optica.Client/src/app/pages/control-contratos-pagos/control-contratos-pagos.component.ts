import { Component, OnInit } from '@angular/core';
import { Contrato } from 'src/app/models/Contrato';

@Component({
  selector: 'app-control-contratos-pagos',
  templateUrl: './control-contratos-pagos.component.html',
  styleUrls: ['./control-contratos-pagos.component.css']
})
export class ControlContratosPagosComponent implements OnInit {

  model: Contrato = new Contrato();
  contratoSelect: boolean = false;
  detalles: any[] = [];

  constructor() { }

  test(){
    this.contratoSelect = !this.contratoSelect
  }

  tabs: string[] = ['Corrida Original', 'Edo. Cuenta', 'Pagos', 'Detalle Pagos'];
  selectedTab: string = '';

  onTabSelected(tab: string) {
    this.selectedTab = tab;
    // Aquí puedes agregar lógica adicional al cambiar de pestaña.
  }

  ngOnInit() {
  }

}
