import { Component, OnInit, Inject } from "@angular/core";
import { VehicleService } from "src/app/services/vehicle-service/vehicle.service";
import { ActivatedRoute } from "@angular/router";
import {
  ManufacturerModel,
  VehicleDetailsModel,
} from "../../interfaces/vehicle.model";
import { ManufacturerService } from "src/app/services/manufacturer-service/manufacturer.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { OrderByPipe } from "src/app/services/pipes/orderby.pipe";

@Component({
  selector: "app-list-vehicle",
  templateUrl: "./list-vehicle.component.html",
  styleUrls: ["./list-vehicle.component.css"],
})
export class ListVehicleComponent implements OnInit {
  vehicles: Array<VehicleDetailsModel>;
  vehicleDetails: VehicleDetailsModel;
  manufacturers: Array<ManufacturerModel>;
  tableHeaders: Array<any>;
  vehicleForm: FormGroup;
  vId: number;
  add = false;
  key: string = "id";
  reverse: boolean = false;

  constructor(
    private vehicleService: VehicleService,
    private activatedRoute: ActivatedRoute,
    private manufacturerService: ManufacturerService,
    private toastr: ToastrService,
    private orderBy: OrderByPipe
  ) {}
  ngOnInit(): void {
    var x = document.getElementById("addTable");
    x.style.display = "none";
    this.activatedRoute.data.subscribe((data: any) => {
      this.vehicles = data.list;
      console.log(data);
    });
    this.manufacturerService.listManufacturers().subscribe(
      (data) => {
        this.manufacturers = data;
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    this.defineVehicleObj();

    this.vehicleForm = new FormGroup({
      ownerName: new FormControl(this.vehicleDetails.ownerName, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(30),
        Validators.pattern("^[A-Za-z ]+$"),
      ]),
      manufacturerId: new FormControl(this.vehicleDetails.manufacturerId, [
        Validators.required,
      ]),
      manufactureYear: new FormControl(this.vehicleDetails.manufactureYear, [
        Validators.required,
        Validators.min(1880),
        Validators.max(2100),
      ]),
      vehicleWeight: new FormControl(this.vehicleDetails.vehicleWeight, [
        Validators.required,
        Validators.min(10),
        Validators.max(99999999999999999),
        Validators.pattern("^[0-9]+(.[0-9]{1,2})?$"),
      ]),
    });
  }
  get ownerName() {
    return this.vehicleForm.get("ownerName");
  }
  get manufacturerId() {
    return this.vehicleForm.get("manufacturerId");
  }
  get vehicleWeight() {
    return this.vehicleForm.get("vehicleWeight");
  }
  get manufactureYear() {
    return this.vehicleForm.get("manufactureYear");
  }
  refreshVehicleDetails() {
    this.vehicleService
      .listVehicles()
      .subscribe((data: Array<VehicleDetailsModel>) => {
        this.vehicles = data;
      });
  }
  submitAdd() {
    // stop here if the form is invalid
    if (this.vehicleForm.invalid) {
      return;
    }

    this.vehicleDetails = {
      id: this.vId ? this.vId : 0,
      ownerName: this.vehicleForm.value.ownerName.trim(),
      manufacturerId: this.vehicleForm.value.manufacturerId,
      manufactureYear: this.vehicleForm.value.manufactureYear,
      vehicleWeight: this.vehicleForm.value.vehicleWeight,
    };
    if (this.add) {
      this.vehicleService.addVehicleDetails(this.vehicleDetails).subscribe(
        (vehicleDetails: VehicleDetailsModel) => {
          console.log("SUBMIT");
          this.vehicles.push(vehicleDetails);
          this.refreshVehicleDetails();
          this.vehicleForm.reset();
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    } else {
      this.vehicleService.updateVehicleDetails(this.vehicleDetails).subscribe(
        () => {
          this.refreshVehicleDetails();
          this.vehicleForm.reset();
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }

  sort(key) {
    console.log(key);
    this.key = key;
    this.reverse = !this.reverse;
  }

  iconClass(vehicle: VehicleDetailsModel): string {
    if (vehicle.weightCategory.iconId === 1) {
      return "fa fa-automobile";
    } else if (vehicle.weightCategory.iconId === 2) {
      return "fa fa-motorcycle";
    } else if (vehicle.weightCategory.iconId === 3) {
      return "fa fa-bus";
    } else {
      return "fa  fa-truck";
    }
  }
  edit(vehicle: VehicleDetailsModel) {
    this.add = false;
    this.vId = vehicle.id;
    this.vehicleForm.setValue({
      ownerName: vehicle.ownerName.trim(),
      manufacturerId: vehicle.manufacturer.id,
      manufactureYear: vehicle.manufactureYear,
      vehicleWeight: vehicle.vehicleWeight,
    });

    var x = document.getElementById("addTable");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  }

  removeVehicle(vehicle: VehicleDetailsModel) {
    let remove = confirm(
      "You are about to remove this vehicle! Please cancel if this was not your intention."
    );
    if (remove) {
      this.vehicleService.removeVehicle(vehicle.id).subscribe(
        () => {
          this.refreshVehicleDetails();
          this.vehicleForm.reset();
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }

  addButtonClick() {
    this.add = true;
    this.vehicleForm.reset();
    var x = document.getElementById("addTable");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  }

  defineVehicleObj() {
    this.vehicleDetails = {
      id: 0,
      ownerName: null,
      manufactureYear: 0,
      manufacturerId: 0,
      vehicleWeight: null,
    };
  }
}
