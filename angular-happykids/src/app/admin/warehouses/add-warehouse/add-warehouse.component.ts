import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { WarehousesService } from '../warehouses.service';

@Component({
  selector: 'app-add-warehouse',
  templateUrl: './add-warehouse.component.html',
  styleUrls: ['./add-warehouse.component.scss']
})
export class AddWarehouseComponent implements OnInit {
  warehouseForm: FormGroup;
  countriesList = [];

  constructor(public warehousesService: WarehousesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.warehousesService.getCountries()
    .subscribe(res => this.countriesList = res as []);

    this.warehouseForm = this.fb.group({
      name: ['', [Validators.required]],
      city: ['', [Validators.required]],
      countryId: [0, [Validators.min(1)]],
      street: ['', [Validators.required]],
     });
  }

  onSubmit() {
    if (this.warehouseForm.invalid) {
        return;
    }
    this.createWarehouse();
  }

  private createWarehouse() {
    this.warehousesService.createWarehouse(this.warehouseForm.value).subscribe(() => {
      this.router.navigateByUrl('warehouses');
    },
    error => {
      console.log(error);
    });
  }

}
