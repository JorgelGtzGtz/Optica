import { HttpClient, HttpErrorResponse } from '@angular/common/http';
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

  private handleError(err: HttpErrorResponse) {
    return throwError(err.error);
  }
}
