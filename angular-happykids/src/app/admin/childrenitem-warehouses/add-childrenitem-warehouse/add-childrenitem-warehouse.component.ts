import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ChildrenItemWarehouse } from 'src/app/shared/models/childrenItemWarehouse';
import { ChildrenitemWarehousesService } from '../childrenitem-warehouses.service';

@Component({
  selector: 'app-add-childrenitem-warehouse',
  templateUrl: './add-childrenitem-warehouse.component.html',
  styleUrls: ['./add-childrenitem-warehouse.component.scss']
})
export class AddChildrenitemWarehouseComponent implements OnInit {
  childrenItemWarehouseForm: FormGroup;
  childrenItemsList = [];
  warehousesList = [];

  constructor(public childrenItemWarehousesService: ChildrenitemWarehousesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.childrenItemWarehousesService.getChildrenItems()
    .subscribe(res => this.childrenItemsList = res as []);

    this.childrenItemWarehousesService.getWarehouses()
    .subscribe(res => this.warehousesList = res as []);

    this.childrenItemWarehouseForm = this.fb.group({
      childrenItemId: [0, [Validators.min(1)]],
      warehouseId: [0, [Validators.min(1)]],
      stockQuantity: ['', [Validators.required]],
     });
  }

  onSubmit() {
    if (this.childrenItemWarehouseForm.invalid) {
        return;
    }
    this.createChildrenItemWarehouse();
  }

  private createChildrenItemWarehouse() {
    this.childrenItemWarehousesService.createChildrenItemWarehouse(this.childrenItemWarehouseForm.value).subscribe(() => {
      this.router.navigateByUrl('childrenitemwarehouses');
    },
    error => {
      console.log(error);
    });
  }

}
