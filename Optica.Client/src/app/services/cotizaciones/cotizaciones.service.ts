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
export class CotizacionesService {

  private _url = `${AppSettings.API_ENDPOINT}/Cotizacion`;
  private _getLista = `${this._url}/Lista`;
  private _getGetCotizacion = `${this._url}/GetCotizacion`;
  private _guardar = `${this._url}/Guardar`;
  private _getCombos = `${this._url}/ListasCombos`;


  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(from: String, to: String, cliente: number, vendedor: number, estatus: String): Observable<any[]> {
    const params = new HttpParams()
      .set('from', from.toString())
      .set('to', to.toString())
      .set('cliente', String(cliente))
      .set('vendedor', String(vendedor))
      .set('estatus', String(estatus));

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

  getCotizacion(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetCotizacion}/${id}`, { headers: this._userService.header})
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

  // Handdle Error methor for observale
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
