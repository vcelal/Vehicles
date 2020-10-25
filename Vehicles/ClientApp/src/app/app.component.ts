import { Component } from "@angular/core";
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
} from "@angular/router";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent {
  title = "app";
  public spinner = true;

  constructor(private router: Router) {
    this.router.events.subscribe(
      (event) => {
        if (event instanceof NavigationStart) {
          this.spinner = true;
        } else if (
          event instanceof NavigationEnd ||
          event instanceof NavigationCancel ||
          event instanceof NavigationError
        ) {
          this.spinner = false;
        }
      },
      () => {
        this.spinner = false;
      }
    );
  }
}
