import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';
import { Product } from '../interfaces/product';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private url = environment.apiUrl + '/api/products';

  httpOptions = {
    headers: new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', 'f4522298-a723-4c47-ad43-594f09eeae66'),
  };

  constructor(
    private http: HttpClient,
    private commonService: CommonService,
    private storageManager: StorageManager,
  ) {}

  getHttpHeaders(): HttpHeaders {
    const login = JSON.parse(this.storageManager.getLogin());
    const token = login ? login.token : '';

    return new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', token);
  }

  /** GET products from the server */
  getProducts(): Observable<Product[]> {
    return this.http
      .get<Product[]>(this.url, { headers: this.getHttpHeaders() })
      .pipe(tap(), catchError(this.handleError<Product[]>('Get Products', [])));
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   *
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      //console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log error with the MessageService */
  private log(message: string) {
    this.commonService.updateToastData(message, 'danger', 'Error');
  }
}
