import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

import { AppSettings } from '../../models/app-settings';
import { UsersService } from '../users/users.service';

@Injectable({
  providedIn: 'root'
})
export class TiposUsuarioService {
  private _url = `${AppSettings.API_ENDPOINT}/TiposUsuario`;
  private _getLista = `${this._url}/Lista`;
  private _getGetTipoUsuario = `${this._url}/GetTipoUsuario`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;
  private _getAccesos = `${this._url}/Accesos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(usuario: String): Observable<any[]> {
    const params = new HttpParams().set('usuario', usuario.toString());

    return this._http.get<any[]>(this._getLista, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getAccesos(): Observable<any[]> {
    return this._http.get<any[]>(this._getAccesos, { params: null, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getTipoUsuario(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetTipoUsuario}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  guardar(_usuario: any): Observable<any> {
    return this._http.post<any>(`${this._guardar}`, _usuario, { headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  eliminar(id: number) {
      return this._http.delete(`${this._eliminar}/${id}`, { headers: this._userService.header}).pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  // Handdle Error methor for observale
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
