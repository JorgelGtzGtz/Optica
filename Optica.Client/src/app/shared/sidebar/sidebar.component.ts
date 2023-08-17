import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users/users.service';
import { AclService } from 'ng2-acl/dist';


@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  usuarioName: string;
  constructor(public _userService: UsersService, public aclService: AclService) {
    this._userService.loadStorage();
  }

  ngOnInit() {
    this.usuarioName = this._userService.user._Usuario;
  }

}
