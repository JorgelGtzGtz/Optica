import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { UsersService } from '../users/users.service';

@Injectable()
export class LoginGuardGuard implements CanActivate {
  constructor(public _usersService: UsersService, public router: Router) {
  }

  canActivate() {
    if (this._usersService.isLogged()) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
