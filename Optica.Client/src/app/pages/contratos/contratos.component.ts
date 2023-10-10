import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ToastrService } from 'ngx-toastr';
import { Contrato } from 'src/app/models/Contrato';


@Component({
  selector: 'app-contratos',
  templateUrl: './contratos.component.html',
  styleUrls: ['./contratos.component.css']
})
export class ContratosComponent implements OnInit {
  @ViewChild('editModal') editModal: ModalDirective;
  model: Contrato = new Contrato();
  modalRefProd: BsModalRef;
  modalRef: BsModalRef;
  productos: any[] = [];
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
  };

  constructor(private modalService: BsModalService, private toastr: ToastrService) { }


  ngOnInit() {
  }

  onShow(id: number, template: TemplateRef<any>) {
    this.model = new Contrato();
    this.productos = [];
    if (id <= 0) {
      this.modalRef = this.modalService.show(template, this.config);
    } 
  }

  onShowNewProduct(template: TemplateRef<any>){
    this.modalRefProd = this.modalService.show(template, this.config);
  }

}
