import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { BirthdaypackagesService } from '../birthdaypackages.service';

@Component({
  selector: 'app-edit-birthdaypackage',
  templateUrl: './edit-birthdaypackage.component.html',
  styleUrls: ['./edit-birthdaypackage.component.scss']
})
export class EditBirthdaypackageComponent implements OnInit {
  birthdayPackageForm: FormGroup;
  model: BirthdayPackage;
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedKidActivities: MultipleSelectorModel[] = [];
  selectedKidActivities: MultipleSelectorModel[] = [];
  id: number;
  birthdayPackage: BirthdayPackage;

  constructor(public birthdayPackagesService: BirthdaypackagesService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadBirthdayPackage();

    this.activatedRoute.params.subscribe(params => {
      this.birthdayPackagesService.putGetBirthdayPackage(params.id).subscribe(putGet => {
        this.model = putGet.BirthdayPackage;
        this.selectedDiscounts = putGet.selectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.nonSelectedDiscounts = putGet.nonSelectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.selectedKidActivities = putGet.selectedKidActivities.map(kidactivity => {
          return {key: kidactivity.id, value: kidactivity.name} as MultipleSelectorModel;
        });
        this.nonSelectedKidActivities = putGet.nonSelectedKidActivities.map(kidactivity => {
          return {key: kidactivity.id, value: kidactivity.name} as MultipleSelectorModel;
        });
      });
    });

    this.birthdayPackageForm = this.fb.group({
      id: [this.id],
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

    this.birthdayPackagesService.getBirthdayPackageById(this.id)
    .pipe(first())
    .subscribe(x => this.birthdayPackageForm.patchValue(x));
  }

  loadBirthdayPackage() {
    return this.birthdayPackagesService.getBirthdayPackageById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.birthdayPackage = response;
    }, error => {
    console.log(error);
    });
    }

  onSubmit() {
    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.birthdayPackageForm.get('discountsIds').setValue(discountsIds);

    const kidActivitiesIds = this.selectedKidActivities.map(value => value.key);
    this.birthdayPackageForm.get('kidActivitiesIds').setValue(kidActivitiesIds);

    this.birthdayPackagesService.updateBirthdayPackage(this.id, this.birthdayPackageForm.value).subscribe(() => {
    this.router.navigateByUrl('birthdaypackages');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.birthdayPackageForm.get('picture').setValue(image);
      }

}
