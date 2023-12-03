import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AppSettings } from 'src/app/models/app-settings';
import { UsersService } from '../service.index';

@Injectable({
  providedIn: 'root'
})
export class PagosService {
  private _url = `${AppSettings.API_ENDPOINT}/Pagos`;
  private _getCombos = `${this._url}/Combos`;
  private _getPagos = `${this._url}/GetPagos`;
  private _getContratos = `${this._url}/GetContratos`;
  private _aplicarImporte = `${this._url}/AplicarImporte`;
  private _pagoContrato = `${this._url}/PagoContrato`;

  constructor(public _http: HttpClient, private _userService: UsersService) { 
    this._userService.loadStorage();
  }

  getCombos(): Observable<any> {
    return this._http.get<any>(this._getCombos, { params: null, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getContratos(id:any): Observable<any> {
    const params = new HttpParams()
    .set('id', id)
    return this._http.get<any>(this._getContratos, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getPagos(id:any, fecha: any): Observable<any> {
    const params = new HttpParams()
    .set('id', id)
    .set('fecha', fecha)
    return this._http.get<any>(this._getPagos, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  aplicarImporte(id:any, importe: any, fecha: any): Observable<any> {
    const params = new HttpParams()
    .set('importe', importe)
    .set('fecha', fecha)
    .set('id', id)
    return this._http.get<any>(this._aplicarImporte, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  pagoContrato(model: any): Observable<any> {
    return this._http.post<any>(this._pagoContrato, model, {headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
