import { Component, OnInit } from '@angular/core';
import { AclService } from 'ng2-acl/dist';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  can:any;

  constructor(private aclService: AclService) {}

  ngOnInit() {
      this.can = this.aclService.can;
  }
}
