import { Injectable } from '@angular/core';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { VehicleDetailsModel } from 'src/app/interfaces/vehicle.model';
import { VehicleService } from './vehicle.service';

@Injectable({ providedIn: 'root' })
export class VehicleResolver implements Resolve<VehicleDetailsModel> {
  constructor(private vehicleService: VehicleService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> | Promise<any> | any {
    return this.vehicleService.listVehicles();
  }
}
