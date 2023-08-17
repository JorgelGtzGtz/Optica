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
export class DiagnosticosService {

  private _url = `${AppSettings.API_ENDPOINT}/Diagnosticos`;
  private _getLista = `${this._url}/Lista`;
  private _getDiagnostico = `${this._url}/GetDiagnostico`;
  private _guardar = `${this._url}/Guardar`;
  private _getCombos = `${this._url}/Combos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(from: String, to: String, paciente: number, opto: number, folio: String): Observable<any[]> {
    const params = new HttpParams()
      .set('from', from.toString())
      .set('to', to.toString())
      .set('paciente', String(paciente))
      .set('opto', String(opto))
      .set('folio', String(folio));

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

  getDiagnostico(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getDiagnostico}/${id}`, { headers: this._userService.header})
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
