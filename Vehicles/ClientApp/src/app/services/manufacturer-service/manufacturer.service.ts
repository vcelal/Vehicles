import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiPath } from 'src/app/interfaces/apiPath';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ManufacturerService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  listManufacturers(): Observable<any> {
    return this.http.get(this.baseUrl + ApiPath.ListManufacturers);
  }
}
