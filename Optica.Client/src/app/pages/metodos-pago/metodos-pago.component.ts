import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-metodos-pago',
  templateUrl: './metodos-pago.component.html',
  styleUrls: ['./metodos-pago.component.css']
})
export class MetodosPagoComponent implements OnInit {

  metodospago:any[] = [];
  a:any =1;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };
  constructor(private modalService: BsModalService) { }

  ngOnInit() {
  }

  onShow(id: number, template: TemplateRef<any>) {
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } 
  }

}
