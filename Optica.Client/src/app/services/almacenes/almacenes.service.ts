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
export class AlmacenesService {

  private _url = `${AppSettings.API_ENDPOINT}/Almacenes`;
  private _getLista = `${this._url}/Lista`;
  private _getGetAlmacen = `${this._url}/GetAlmacen`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;
  private _getSucursales = `${this._url}/Sucursales`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(sucursal: String): Observable<any[]> {
    const params = new HttpParams().set('almacen', sucursal.toString());

    return this._http.get<any[]>(this._getLista, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getSucursales(): Observable<any[]> {
    return this._http.get<any[]>(this._getSucursales, { params: null, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getAlmacen(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetAlmacen}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  guardar(sucursal: any): Observable<any> {
    return this._http.post<any>(`${this._guardar}`, sucursal, { headers: this._userService.header})
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
