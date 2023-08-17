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
export class ComprasService {

  private _url = `${AppSettings.API_ENDPOINT}/Compras`;
  private _getLista = `${this._url}/Lista`;
  private _getGetCompra = `${this._url}/GetCompra`;
  private _guardar = `${this._url}/Guardar`;
  private _cancelar = `${this._url}/Cancelar`;
  private _procesar = `${this._url}/Procesar`;
  private _getCombos = `${this._url}/Combos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(from: String, to: String, proveedor: number, folio: String, factura: String): Observable<any[]> {
    const params = new HttpParams()
      .set('from', from.toString())
      .set('to', to.toString())
      .set('proveedor', String(proveedor))
      .set('folio', String(folio))
      .set('factura', String(factura));

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

  getCompra(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetCompra}/${id}`, { headers: this._userService.header})
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

  cancelar(id: number) {
      return this._http.post(`${this._cancelar}/${id}`, null, { headers: this._userService.header}).pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  procesar(id: number) {
      return this._http.post(`${this._procesar}/${id}`, null, { headers: this._userService.header}).pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  // Handdle Error methor for observale
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
