import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse,  } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { AppSettings } from 'src/app/models/app-settings';
import { UsersService } from '../service.index';


@Injectable({
  providedIn: 'root'
})
export class ContratosService {
  private _url = `${AppSettings.API_ENDPOINT}/Contrato`;
  private _getLastContrato = `${this._url}/GetLastContrato`;
  private _getCombos = `${this._url}/Combos`;
  private _getPacientes = `${this._url}/Pacientes`;
  private _getPaciente = `${this._url}/Paciente`;
  private _getDiagnosticos = `${this._url}/Diagnosticos`;
  private _getAlmacenes = `${this._url}/Almacene`;
  private _getProducto = `${this._url}/Producto`;
  private _guardar = `${this._url}/Guardar`;

  constructor(public _http: HttpClient, private _userService: UsersService) {
    this._userService.loadStorage();
  }

  getLastContrato(): Observable<any> {
    return this._http.get<any>(this._getLastContrato, { headers: this._userService.header})
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

  getPacientes(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getPacientes}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getAlmacenes(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getAlmacenes}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getDiagnosticos(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getDiagnosticos}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getProducto(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getProducto}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  getPaciente(id: number): Observable<any>  {
    return this._http.get<any>(`${this._getPaciente}/${id}`, { headers: this._userService.header})
      .pipe(
        tap(data => data),
        catchError(this.handleError)
      );
  }

  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
