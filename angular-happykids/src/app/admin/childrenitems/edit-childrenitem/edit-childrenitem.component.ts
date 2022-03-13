import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ChildrenitemsService } from '../childrenitems.service';

@Component({
  selector: 'app-edit-childrenitem',
  templateUrl: './edit-childrenitem.component.html',
  styleUrls: ['./edit-childrenitem.component.scss']
})
export class EditChildrenitemComponent implements OnInit {
  model: ChildrenItem;
  childrenItemForm: FormGroup;
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];
  nonSelectedTags: MultipleSelectorModel[] = [];
  selectedTags: MultipleSelectorModel[] = [];
  id: number;

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private childrenitemsService: ChildrenitemsService) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.activatedRoute.params.subscribe(params => {
      this.childrenitemsService.putGetChildrenItem(params.id).subscribe(putGet => {
        this.model = putGet.childrenItem;
        this.selectedCategories = putGet.selectedCategories.map(category => {
          return {key: category.id, value: category.name} as MultipleSelectorModel;
        });
        this.nonSelectedCategories = putGet.nonSelectedCategories.map(category => {
          return {key: category.id, value: category.name} as MultipleSelectorModel;
        });
        this.selectedDiscounts = putGet.selectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.nonSelectedDiscounts = putGet.nonSelectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.selectedManufacturers = putGet.selectedManufacturers.map(manufacturer => {
          return {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
        });
        this.nonSelectedManufacturers = putGet.nonSelectedManufacturers.map(manufacturer => {
          return {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
        });
        this.selectedTags = putGet.selectedTags.map(tag => {
          return {key: tag.id, value: tag.name} as MultipleSelectorModel;
        });
        this.nonSelectedTags = putGet.nonSelectedTags.map(tag => {
          return {key: tag.id, value: tag.name} as MultipleSelectorModel;
        });
      });
    });

    this.childrenItemForm = this.fb.group({
      id: [this.id],
      name: [null, [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      categoriesIds: [null],
      discountsIds: [null],
      manufacturersIds: [null],
      tagsIds: [null],
      picture: ''
     });

    this.childrenitemsService.getChildrenItemById(this.id)
    .pipe(first())
    .subscribe(x => this.childrenItemForm.patchValue(x));
  }

  onSubmit() {
    const categoriesIds = this.selectedCategories.map(value => value.key);
    this.childrenItemForm.get('categoriesIds').setValue(categoriesIds);

    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.childrenItemForm.get('discountsIds').setValue(discountsIds);

    const manufacturersIds = this.selectedManufacturers.map(value => value.key);
    this.childrenItemForm.get('manufacturersIds').setValue(manufacturersIds);

    const tagsIds = this.selectedTags.map(value => value.key);
    this.childrenItemForm.get('tagsIds').setValue(tagsIds);

    this.childrenitemsService.updateChildrenItem(this.id, this.childrenItemForm.value).subscribe(() => {
    this.router.navigateByUrl('childrenitems');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.childrenItemForm.get('picture').setValue(image);
      }

}
