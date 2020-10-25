import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiPath } from '../../interfaces/apiPath';
import { map } from 'rxjs/operators';
import { VehicleDetailsModel } from '../../interfaces/vehicle.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  addVehicleDetails(vehicleDetails: VehicleDetailsModel): Observable<any>{
    const body ={
      OwnerName: vehicleDetails.ownerName,
      ManufacturerId: vehicleDetails.manufacturerId,
      ManufactureYear: vehicleDetails.manufactureYear,
      VehicleWeight: vehicleDetails.vehicleWeight,
    }
    return this.http.post(this.baseUrl+ApiPath.AddVehicle, body);
  }

  listVehicles(): Observable<any> {
    return this.http.get(this.baseUrl + ApiPath.ListVehicle);
  }
  
  updateVehicleDetails(vehicleDetails: VehicleDetailsModel): Observable<any>{
    const body ={
      Id: vehicleDetails.id,
      OwnerName: vehicleDetails.ownerName,
      ManufacturerId: vehicleDetails.manufacturerId,
      ManufactureYear: vehicleDetails.manufactureYear,
      VehicleWeight: vehicleDetails.vehicleWeight,
    }
    return this.http.put(this.baseUrl+ApiPath.EditVehicle, body);
  }

  removeVehicle(vehicleId: number): Observable<any> {
    const params = new HttpParams().set('vehicleId', vehicleId.toString());
    return this.http.delete(this.baseUrl + ApiPath.RemoveVehicle, { params })
  }
}
