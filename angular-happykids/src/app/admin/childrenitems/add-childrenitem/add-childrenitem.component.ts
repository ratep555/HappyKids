import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ChildrenitemsService } from '../childrenitems.service';

@Component({
  selector: 'app-add-childrenitem',
  templateUrl: './add-childrenitem.component.html',
  styleUrls: ['./add-childrenitem.component.scss']
})
export class AddChildrenitemComponent implements OnInit {
  childrenitemForm: FormGroup;
  model: ChildrenItem;
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];
  nonSelectedTags: MultipleSelectorModel[] = [];
  selectedTags: MultipleSelectorModel[] = [];

  constructor(public childrenitemsService: ChildrenitemsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.childrenitemsService.getAllCategories().subscribe(response => {
      this.nonSelectedCategories = response.map(category => {
        return  {key: category.id, value: category.name} as MultipleSelectorModel;
      });
    });

    this.childrenitemsService.getAllDiscounts().subscribe(response => {
      this.nonSelectedDiscounts = response.map(discount => {
        return  {key: discount.id, value: discount.name} as MultipleSelectorModel;
      });
    });

    this.childrenitemsService.getAllManufacturers().subscribe(response => {
      this.nonSelectedManufacturers = response.map(manufacturer => {
        return  {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
      });
    });

    this.childrenitemsService.getAllTags().subscribe(response => {
      this.nonSelectedTags = response.map(tag => {
        return  {key: tag.id, value: tag.name} as MultipleSelectorModel;
      });
    });

    this.createChildrenitemForm();
  }

  createChildrenitemForm() {
    this.childrenitemForm = this.fb.group({
      name: [null, [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      categoriesIds: [null],
      manufacturersIds: [null],
      tagsIds: [null],
      discountsIds: [null],
      picture: ''
    });
  }

  onSubmit() {
    const categoriesIds = this.selectedCategories.map(value => value.key);
    this.childrenitemForm.get('categoriesIds').setValue(categoriesIds);

    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.childrenitemForm.get('discountsIds').setValue(discountsIds);

    const manufacturersIds = this.selectedManufacturers.map(value => value.key);
    this.childrenitemForm.get('manufacturersIds').setValue(manufacturersIds);

    const tagsIds = this.selectedTags.map(value => value.key);
    this.childrenitemForm.get('tagsIds').setValue(tagsIds);

    this.childrenitemsService.createChildrenItem(this.childrenitemForm.value).subscribe(() => {
      this.router.navigateByUrl('childrenitems');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.childrenitemForm.get('picture').setValue(image);
  }

}
