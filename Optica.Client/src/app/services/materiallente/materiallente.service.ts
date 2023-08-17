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
export class MaterialLenteService {

  private _url = `${AppSettings.API_ENDPOINT}/Materiales`;
  private _getLista = `${this._url}/Lista`;
  private _getGetMaterialLente = `${this._url}/GetMateriale`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(filter: String): Observable<any[]> {
    const params = new HttpParams().set('descripcion', filter.toString());

    return this._http.get<any[]>(this._getLista, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  getMaterialLente(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getGetMaterialLente}/${id}`, { headers: this._userService.header})
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
