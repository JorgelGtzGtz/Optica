import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { AclService } from 'ng2-acl/dist';

import { AppSettings } from '../../models/app-settings';
import { Login } from '../../models/Login';
import { Usuario } from '../../models/Usuario';
import { Acceso } from '../../models/Acceso';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private _url = `${AppSettings.API_ENDPOINT}/Usuario`;
  private _getLista = `${this._url}/Lista`;
  private _getGetUsuario = `${this._url}/GetUsuario`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;
  private _getSucursales = `${this._url}/Sucursales`;
  private _getTiposUsuarios = `${this._url}/TiposUsuarios`;
  private _login = `${this._url}/Login`;

  user: Usuario;
  userAccesos: Acceso[];
  header: HttpHeaders;
  aclData = {};
  constructor(public _http: HttpClient, public router: Router, private aclService: AclService) {
    this.loadStorage();
  }

  getLista(usuario: String): Observable<any[]> {
    const params = new HttpParams().set('usuario', usuario.toString());

    return this._http.get<any[]>(this._getLista, { params: params, headers: this.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getSucursales(): Observable<any[]> {
    return this._http.get<any[]>(this._getSucursales, { params: null, headers: this.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getTiposUsuarios(): Observable<any[]> {
    return this._http.get<any[]>(this._getTiposUsuarios, { params: null, headers: this.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getUsuario(id: number): Observable<Usuario>  {
    return this._http.get<Usuario>(`${this._getGetUsuario}/${id}`, { headers: this.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  // tslint:disable-next-line: variable-name
  guardar(_usuario: any): Observable<Usuario> {
    return this._http.post<Usuario>(`${this._guardar}`, _usuario, { headers: this.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  eliminar(id: number) {
      return this._http.delete(`${this._eliminar}/${id}`, { headers: this.header}).pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  login( _user: Login ) {

    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa(`${_user.user}:${_user.password}`),
      'Content-Type': 'application/json'
    });

    return this._http.post(`${this._login}`, null, { headers: headers})
    .pipe(
      tap((data: any) => {
        let accesoList:string[] = [];
        data.accesos.forEach(acceso => {
          accesoList.push(acceso.Nombre);
        });

        this.aclData = {
            member: accesoList
        };

        this.aclService.setAbilities(this.aclData);

        // Attach the member role to the current user
        this.aclService.attachRole('member');

        this.saveStorage(data.usuario, data.accesos, _user.user, _user.password);
        return true;
      }),
      catchError(this.handleError)
    );
  }

  // Logout
  logout() {
    this.user = null;

    localStorage.removeItem('user_' + AppSettings.site_name);
    localStorage.removeItem('header');
    this.router.navigate(['/login']);
  }

  isLogged() {
    return ( this.user !== null  ) ? true : false;
  }

  loadStorage() {
    if ( localStorage.getItem('user_' + AppSettings.site_name)) {
      const now = new Date().getTime();
      const hours = 24;
      this.user = JSON.parse( localStorage.getItem('user_' + AppSettings.site_name) );
      this.userAccesos = JSON.parse( localStorage.getItem('user_Acceso_' + AppSettings.site_name) );
      const basicauthentication = JSON.parse( localStorage.getItem('header') );
      this.header = new HttpHeaders({
        'Authorization': basicauthentication,
        'Content-Type': 'application/json'
      });
      const ExpirationSetupTime = JSON.parse( localStorage.getItem('ExpirationSetupTime') );
      if (ExpirationSetupTime !== undefined && ExpirationSetupTime !== null) {
          if (now - ExpirationSetupTime > hours * 60 * 60 * 1000) {
            this.user = null;
            localStorage.removeItem('user_' + AppSettings.site_name);
            localStorage.removeItem('user_Acceso_' + AppSettings.site_name);
            localStorage.removeItem('header');
            this.router.navigate(['/login']);
          }
      } else {
        this.user = null;
        localStorage.removeItem('user_' + AppSettings.site_name);
        localStorage.removeItem('user_Acceso_' + AppSettings.site_name);
        localStorage.removeItem('header');
        this.router.navigate(['/login']);
      }
    } else {
      this.user = null;
      localStorage.removeItem('user_' + AppSettings.site_name);
      localStorage.removeItem('user_Acceso_' + AppSettings.site_name);
      localStorage.removeItem('header');
      this.router.navigate(['/login']);
    }
  }

  saveStorage(user: Usuario, accesos: Acceso[], currentuser: string, currentpassword: string) {
    const now = new Date().getTime();
    localStorage.setItem('user_' + AppSettings.site_name, JSON.stringify(user) );
    localStorage.setItem('user_Acceso_' + AppSettings.site_name, JSON.stringify(accesos) );
    localStorage.setItem('header', JSON.stringify('Basic ' + btoa(`${currentuser}:${currentpassword}`)));
    localStorage.setItem('ExpirationSetupTime', JSON.stringify(now) );
    this.user = user;
  }

  // Handdle Error methor for observale
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
