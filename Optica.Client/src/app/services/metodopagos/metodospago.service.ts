import { Injectable } from '@angular/core';
import { UsersService } from '../users/users.service';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { AppSettings } from '../../models/app-settings';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MetodospagoService {

  private _url = `${AppSettings.API_ENDPOINT}/Metodospago`;
  private _getLista = `${this._url}/Lista`;
  private _getCombos = `${this._url}/Combos`;
  private _guardar = `${this._url}/Guardar`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(descripcion: String): Observable<any[]> {
    const params = new HttpParams()
    .set('descripcion', String(descripcion));
    return this._http.get<any[]>(this._getLista, { params: params, headers: this._userService.header})
    .pipe(
      tap(data => data),
      catchError(this.handleError)
    );
  }

  guardar(model: any) {
    return this._http.post<any>(`${this._guardar}`, model, { headers: this._userService.header})
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
  
  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }

}
