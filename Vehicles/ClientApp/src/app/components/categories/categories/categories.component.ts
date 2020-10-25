import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { WeightCategoryModel } from "src/app/interfaces/vehicle.model";
import { CategoryService } from "src/app/services/category-service/category.service";

@Component({
  selector: "app-categories",
  templateUrl: "./categories.component.html",
  styleUrls: ["./categories.component.css"],
})
export class CategoriesComponent implements OnInit {
  categories: Array<WeightCategoryModel>;
  category: WeightCategoryModel;
  categoryForm: FormGroup;
  add = false;
  cId: number;
  constructor(
    private activatedRoute: ActivatedRoute,
    private categoryService: CategoryService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    var x = document.getElementById("addTable");
    x.style.display = "none";
    this.activatedRoute.data.subscribe((data: any) => {
      this.categories = data.list;
    });
    this.formValidation();
  }

  addButtonClick() {
    this.add = true;
    this.categoryForm.reset();
    var x = document.getElementById("addTable");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  }

  submitAdd() {
    // stop here if the form is invalid
    if (this.categoryForm.invalid) {
      return;
    }
    console.log(this.categoryForm.value);
    this.category = {
      id: this.cId ? this.cId : 0,
      name: this.categoryForm.value.categoryName.trim(),
      minWeight: this.categoryForm.value.minWeight,
      maxWeight: this.categoryForm.value.maxWeight,
      iconId: this.categoryForm.value.iconId,
    };
    if (this.add) {
      this.categoryService.addCategory(this.category).subscribe(
        (category: WeightCategoryModel) => {
          this.categories.push(category);
          this.categoryForm.reset();
        },
        (error) => this.toastr.error(error.error)
      );
    } else {
      this.categoryService.updateCategory(this.category).subscribe(
        () => {
          this.categoryService.listCategories().subscribe(
            (data: Array<WeightCategoryModel>) => {
              this.categories = data;
              this.categoryForm.reset();
            },
            (error) => {
              this.toastr.error(error.error);
            }
          );
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }

  removeCategory(category: WeightCategoryModel) {
    let remove = confirm(
      "You are about to remove this category! This will remove all related vehicle history."
    );
    if (remove) {
      this.categoryService.removeCategory(category.id).subscribe(
        () => {
          let index = this.categories.indexOf(category);
          this.categories.splice(index, 1);
          this.categoryService
            .listCategories()
            .subscribe((data: Array<WeightCategoryModel>) => {
              this.categories = data;
            });
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }

  edit(category: WeightCategoryModel) {
    this.add = false;
    this.cId = category.id;
    this.categoryForm.setValue({
      categoryName: category.name.trim(),
      minWeight: category.minWeight,
      maxWeight: category.maxWeight,
      iconId: category.iconId,
    });

    var x = document.getElementById("addTable");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  }

  defineCategoryObj() {
    this.category = {
      id: 0,
      name: null,
      minWeight: 0,
      maxWeight: 0,
      iconId: 0,
    };
  }

  formValidation() {
    this.defineCategoryObj();

    this.categoryForm = new FormGroup({
      categoryName: new FormControl(this.category.name, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(20),
        Validators.pattern("^[A-Za-z ]+$"),
      ]),
      minWeight: new FormControl(this.category.minWeight, [
        Validators.required,
        Validators.pattern("^[0-9]+(.[0-9]{1,2})?$"),
      ]),
      maxWeight: new FormControl(this.category.maxWeight, [
        Validators.required,
        Validators.pattern("^[0-9]+(.[0-9]{1,2})?$"),
      ]),
      iconId: new FormControl(this.category.iconId, [Validators.required]),
    });
  }
  get categoryName() {
    return this.categoryForm.get("categoryName");
  }
  get minWeight() {
    return this.categoryForm.get("minWeight");
  }
  get maxWeight() {
    return this.categoryForm.get("maxWeight");
  }
  get iconId() {
    return this.categoryForm.get("iconId");
  }
}
