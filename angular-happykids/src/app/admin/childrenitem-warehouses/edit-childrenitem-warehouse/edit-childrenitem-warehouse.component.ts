import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ChildrenItemWarehouse } from 'src/app/shared/models/childrenItemWarehouse';
import { ChildrenitemWarehousesService } from '../childrenitem-warehouses.service';

@Component({
  selector: 'app-edit-childrenitem-warehouse',
  templateUrl: './edit-childrenitem-warehouse.component.html',
  styleUrls: ['./edit-childrenitem-warehouse.component.scss']
})
export class EditChildrenitemWarehouseComponent implements OnInit {
  childrenItemWarehouseForm: FormGroup;
  childrenItemWarehouse: ChildrenItemWarehouse;
  childrenItemsList = [];
  warehousesList = [];
  id: number;
  warehouseid: number;

  constructor(public childrenItemWarehouseService: ChildrenitemWarehousesService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.warehouseid = this.activatedRoute.snapshot.params['warehouseid'];

    this.childrenItemWarehouseService.getChildrenItems()
    .subscribe(res => this.childrenItemsList = res as []);

    this.childrenItemWarehouseService.getWarehouses()
    .subscribe(res => this.warehousesList = res as []);

    this.childrenItemWarehouseForm = this.fb.group({
      childrenItemId: [this.id, [Validators.min(1)]],
      warehouseId: [this.warehouseid, [Validators.min(1)]],
      stockQuantity: ['', [Validators.required]]
     });

    this.childrenItemWarehouseService.getChildrenItemWarehouseByChildrenItemIdAndWarehouseId
    (this.id, this.warehouseid)
    .pipe(first())
    .subscribe(x => this.childrenItemWarehouseForm.patchValue(x));
  }

  onSubmit() {
    if (this.childrenItemWarehouseForm.invalid) {
        return;
    }
    this.updateChildrenItemWarehouse();
  }

  private updateChildrenItemWarehouse() {
    this.childrenItemWarehouseService.updateChildrenItemWarehouse
    (this.id, this.warehouseid, this.childrenItemWarehouseForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('childrenitemwarehouses');
          }, error => {
            console.log(error);
          });
        }

}
