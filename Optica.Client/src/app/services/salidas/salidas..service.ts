import { Injectable } from '@angular/core';
import { UsersService } from '../users/users.service';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { AppSettings } from '../../models/app-settings';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';


@Injectable({
    providedIn: 'root'
  })
export class SalidaService {

  private _url = `${AppSettings.API_ENDPOINT}/Salida`;
  private _getLista = `${this._url}/Lista`;
  private _guardar = `${this._url}/Guardar`;
  private _eliminar = `${this._url}/Eliminar`;
  private _procesar = `${this._url}/Procesar`;
  private _getSalida = `${this._url}/GetSalida`;
  private _cancelar = `${this._url}/Cancelar`;
  private _getCombos = `${this._url}/Combos`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLista(): Observable<any[]> {
    return this._http.get<any[]>(this._getLista, { params: null, headers: this._userService.header})
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

  getCombos(): Observable<any> {
    return this._http.get<any>(this._getCombos, { params: null, headers: this._userService.header})
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

  procesar(id: number) {
    return this._http.post(`${this._procesar}/${id}`, null, { headers: this._userService.header}).pipe(
    tap(data => data),
    catchError(this.handleError)
  );
}

  getSalida(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getSalida}/${id}`, { headers: this._userService.header})
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

  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
  
}