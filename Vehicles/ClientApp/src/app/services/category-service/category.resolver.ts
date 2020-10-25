import { Injectable } from '@angular/core';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { WeightCategoryModel } from 'src/app/interfaces/vehicle.model';
import { CategoryService } from './category.service';

@Injectable({ providedIn: 'root' })
export class CategoryResolver implements Resolve<WeightCategoryModel> {
  constructor(private categoryService: CategoryService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> | Promise<any> | any {
    return this.categoryService.listCategories();
  }
}
