import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ChildrenitemsService } from '../../childrenitems/childrenitems.service';
import { BirthdaypackagesService } from '../birthdaypackages.service';

@Component({
  selector: 'app-add-birthdaypackage',
  templateUrl: './add-birthdaypackage.component.html',
  styleUrls: ['./add-birthdaypackage.component.scss']
})
export class AddBirthdaypackageComponent implements OnInit {
  birthdayPackageForm: FormGroup;
  model: BirthdayPackage;
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedKidActivities: MultipleSelectorModel[] = [];
  selectedKidActivities: MultipleSelectorModel[] = [];

  constructor(private birthdaypackagesService: BirthdaypackagesService,
              private childrenitemsService: ChildrenitemsService,
              private router: Router,
              private fb: FormBuilder) { }

 ngOnInit(): void {
    this.childrenitemsService.getAllDiscounts().subscribe(response => {
      this.nonSelectedDiscounts = response.map(discount => {
        return  {key: discount.id, value: discount.name} as MultipleSelectorModel;
      });
    });

    this.birthdaypackagesService.getAllKidActivities().subscribe(response => {
      this.nonSelectedKidActivities = response.map(kidactivity => {
        return  {key: kidactivity.id, value: kidactivity.name} as MultipleSelectorModel;
      });
    });

    this.createBirthdayPackageForm();
  }

  createBirthdayPackageForm() {
    this.birthdayPackageForm = this.fb.group({
      packageName: [null, [Validators.required]],
      price: ['', [Validators.required]],
      additionalBillingPerParticipant: ['', [Validators.required]],
      numberOfParticipants: ['', [Validators.required]],
      duration: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      discountsIds: [null],
      kidActivitiesIds: [null],
      picture: ''
    });
  }

  onSubmit() {
    const kidActivitiesIds = this.selectedKidActivities.map(value => value.key);
    this.birthdayPackageForm.get('kidActivitiesIds').setValue(kidActivitiesIds);

    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.birthdayPackageForm.get('discountsIds').setValue(discountsIds);

    this.birthdaypackagesService.createBirthdayPackage(this.birthdayPackageForm.value).subscribe(() => {
      this.router.navigateByUrl('birthdaypackages');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.birthdayPackageForm.get('picture').setValue(image);
  }

}
