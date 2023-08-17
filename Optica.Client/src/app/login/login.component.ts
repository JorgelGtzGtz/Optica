import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { Login } from '../models/Login';
import { UsersService } from '../services/service.index';
declare function init_plugins();

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {

  user: Login = new Login();
  isSing = false;

  Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
  });

  constructor(private _userService: UsersService, public router: Router) { }

  ngOnInit() {
  }

  onSubmit() {
    this.isSing = true;
    this._userService.login(this.user)
    .subscribe(
      success => {
        this.isSing = false;
        this.Toast.fire({
          type: 'success',
          title: 'Inició Sesión Exitosamente'
        });
        this.router.navigate(['/dashboard']);
      },
      error => {
        this.isSing = false;
        Swal.fire('Error!', 'Error al iniciar sesion' , 'error');
      });
  }

  ngAfterViewInit(): void {
    init_plugins();
  }
}
