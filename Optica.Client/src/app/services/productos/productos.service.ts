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
export class ProductosService {

  private _url = `${AppSettings.API_ENDPOINT}/Productos`;
  private _getLista = `${this._url}/Lista`;
  private _getGetProducto = `${this._url}/GetProducto`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;
  private _getCombos = `${this._url}/Combos`;
  private _getProductos = `${this._url}/Productos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(producto: String): Observable<any[]> {
    const params = new HttpParams().set('producto', producto.toString());

    return this._http.get<any[]>(this._getLista, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getCombos(): Observable<any> {
    return this._http.get<any>(this._getCombos, { params: null, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getProductos(): Observable<any> {
    return this._http.get<any>(this._getProductos, { params: null, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getProducto(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetProducto}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  guardar(model: any): Observable<any> {
    return this._http.post<any>(`${this._guardar}`, model, { headers: this._userService.header})
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
