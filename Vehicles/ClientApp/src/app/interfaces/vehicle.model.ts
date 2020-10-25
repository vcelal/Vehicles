export interface VehicleDetailsModel{
   id: number;
   ownerName: string;
   manufactureYear: number;
   manufacturer?: ManufacturerModel;
   weightCategory?: WeightCategoryModel;   
   manufacturerId?: number;
   vehicleWeight?: number;
  }

  export interface ManufacturerModel{
      id: number;
      name: string;
  }

  export interface WeightCategoryModel{
      id: number;
      name: string;
      minWeight: number;
      maxWeight: number;
      iconId: number;
    }