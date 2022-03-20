import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Discount } from 'src/app/shared/models/discount';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ChildrenitemWarehousesService } from '../../childrenitem-warehouses/childrenitem-warehouses.service';
import { DiscountsService } from '../discounts.service';

@Component({
  selector: 'app-add-discount',
  templateUrl: './add-discount.component.html',
  styleUrls: ['./add-discount.component.scss']
})
export class AddDiscountComponent implements OnInit {
  discountForm: FormGroup;
  model: Discount;
  nonSelectedChildrenItems: MultipleSelectorModel[] = [];
  selectedChildrenItems: MultipleSelectorModel[] = [];
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];

  constructor(public discountsService: DiscountsService,
              private childrenitemWarehousesService: ChildrenitemWarehousesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.childrenitemWarehousesService.getChildrenItems().subscribe(response => {
      this.nonSelectedChildrenItems = response.map(childrenItem => {
        return  {key: childrenItem.id, value: childrenItem.name} as MultipleSelectorModel;
      });
    });

    this.discountsService.getCategories().subscribe(response => {
      this.nonSelectedCategories = response.map(category => {
        return  {key: category.id, value: category.name} as MultipleSelectorModel;
      });
    });

    this.discountsService.getManufacturers().subscribe(response => {
      this.nonSelectedManufacturers = response.map(manufacturer => {
        return  {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
      });
    });

    this.createDiscountForm();
  }

  createDiscountForm() {
    this.discountForm = this.fb.group({
      name: [null, [Validators.required]],
      discountPercentage: ['', [Validators.required]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      childrenItemsIds: [null],
      categoriesIds: [null],
      manufacturersIds: [null]
    });
  }

  onSubmit() {
    const childrenItemsIds = this.selectedChildrenItems.map(value => value.key);
    this.discountForm.get('childrenItemsIds').setValue(childrenItemsIds);

    const categoriesIds = this.selectedCategories.map(value => value.key);
    this.discountForm.get('categoriesIds').setValue(categoriesIds);

    const manufacturersIds = this.selectedManufacturers.map(value => value.key);
    this.discountForm.get('manufacturersIds').setValue(manufacturersIds);

    this.discountsService.createDiscount(this.discountForm.value).subscribe(() => {
      this.router.navigateByUrl('discounts');
    },
    error => {
      console.log(error);
    });
  }

}
