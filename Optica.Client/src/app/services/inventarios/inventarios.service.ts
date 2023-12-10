import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppSettings } from 'src/app/models/app-settings';
import { UsersService } from '../service.index';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InventariosService {
  private _url = `${AppSettings.API_ENDPOINT}/Inventario`;
  private _getLista = `${this._url}/Lista`;
  private _getCombos = `${this._url}/Combos`;
  private _getGetProducto = `${this._url}/GetProducto`;
  private _getGetKardexProducto = `${this._url}/GetKardexProducto`;

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

  getProducto(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetProducto}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getKardexProducto(producto: number, almacen: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetKardexProducto}/${producto}/${almacen}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getLista(model: any): Observable<any> {
    return this._http.post<any>(`${this._getLista}`, model, { headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
