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
export class VentasService {

  private _url = `${AppSettings.API_ENDPOINT}/Ventas`;
  private _getLista = `${this._url}/Lista`;
  private _getGetVenta= `${this._url}/GetVenta`;
  private _guardar = `${this._url}/Guardar`;
  private _cancelar = `${this._url}/Cancelar`;
  private _procesar = `${this._url}/Procesar`;
  private _autorizar = `${this._url}/Autorizar`;
  private _getCombos = `${this._url}/Combos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(from: String, to: String, idcliente: number, idsucursal: number, idvendedor: number, idproducto: number, folio: String, folioalt: String): Observable<any[]> {
    const params = new HttpParams()
      .set('from', from.toString())
      .set('to', to.toString())
      .set('idcliente', String(idcliente))
      .set('idsucursal', String(idsucursal))
      .set('idvendedor', String(idvendedor))
      .set('idproducto', String(idproducto))
      .set('folio', String(folio))
      .set('folioalt', String(folioalt));

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

  getVenta(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetVenta}/${id}`, { headers: this._userService.header})
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

  autorizar(model: any): Observable<any> {
    return this._http.post<any>(`${this._autorizar}`, model, { headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  // Handdle Error methor for observale
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
