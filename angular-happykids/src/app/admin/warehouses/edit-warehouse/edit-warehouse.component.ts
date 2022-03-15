import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Warehouse } from 'src/app/shared/models/warehouse';
import { WarehousesService } from '../warehouses.service';

@Component({
  selector: 'app-edit-warehouse',
  templateUrl: './edit-warehouse.component.html',
  styleUrls: ['./edit-warehouse.component.scss']
})
export class EditWarehouseComponent implements OnInit {
  warehouseForm: FormGroup;
  warehouse: Warehouse;
  countriesList = [];
  id: number;

  constructor(public warehousesService: WarehousesService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.warehousesService.getCountries()
    .subscribe(res => this.countriesList = res as []);

    this.warehouseForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]],
      city: ['', [Validators.required]],
      countryId: [0, [Validators.min(1)]],
      street: ['', [Validators.required]],
     });

    this.warehousesService.getWarehouseById
    (this.id)
    .pipe(first())
    .subscribe(x => this.warehouseForm.patchValue(x));
  }

  onSubmit() {
    if (this.warehouseForm.invalid) {
        return;
    }
    this.updateWarehouse();
  }

  private updateWarehouse() {
    this.warehousesService.updateWarehouse
    (this.id, this.warehouseForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('warehouses');
          }, error => {
            console.log(error);
          });
        }

}
