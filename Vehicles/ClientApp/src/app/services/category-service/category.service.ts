import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiPath } from 'src/app/interfaces/apiPath';
import { WeightCategoryModel } from 'src/app/interfaces/vehicle.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService{

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  listCategories(): Observable<any>{
    return this.http.get(this.baseUrl+ApiPath.ListCategories);
  }

  removeCategory(categoryId: number): Observable<any>{
    const params = new HttpParams().set('categoryId', categoryId.toString());

    return this.http.delete(this.baseUrl+ApiPath.DeleteCategory,{params});
  }

  addCategory(category: WeightCategoryModel): Observable<any>{
    const body ={
      Name: category.name,
      MinWeight: category.minWeight,
      MaxWeight: category.maxWeight,
      IconId: category.iconId,
    }
    return this.http.post(this.baseUrl+ApiPath.AddCategory, body);
  }
  updateCategory(category: WeightCategoryModel): Observable<any>{
    const body ={
      Id: category.id,
      Name: category.name,
      MinWeight: category.minWeight,
      MaxWeight: category.maxWeight,
      IconId: category.iconId,
    }
    return this.http.put(this.baseUrl+ApiPath.UpdateCategory, body);
  }

}
