import { BrowserModule } from "@angular/platform-browser";
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { ListVehicleComponent } from "./components/list/list-vehicle.component";
import { VehicleResolver } from "./services/vehicle-service/vehicle.resolver";
import { AlertmodalComponent } from "./modals/alertmodal/alertmodal.component";
import { UpdateModalComponent } from "./modals/update/update-modal/update-modal.component";
import { CategoriesComponent } from "./components/categories/categories/categories.component";
import { CategoryResolver } from "./services/category-service/category.resolver";
import { ToastrModule } from "ngx-toastr";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { OrderByPipe } from "./services/pipes/orderby.pipe";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ListVehicleComponent,
    AlertmodalComponent,
    UpdateModalComponent,
    CategoriesComponent,
    OrderByPipe,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent, pathMatch: "full" },
      {
        path: "list",
        component: ListVehicleComponent,
        resolve: {
          list: VehicleResolver,
        },
      },
      {
        path: "categories",
        component: CategoriesComponent,
        resolve: {
          list: CategoryResolver,
        },
      },
    ]),
  ],

  providers: [OrderByPipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent],
})
export class AppModule {}
