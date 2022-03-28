import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { LocationsService } from 'src/app/locations/locations.service';
import { validate } from 'uuid';
import { BirthdaysService } from '../birthdays.service';

@Component({
  selector: 'app-add-order-birthdays',
  templateUrl: './add-order-birthdays.component.html',
  styleUrls: ['./add-order-birthdays.component.scss']
})
export class AddOrderBirthdaysComponent implements OnInit {
  birthdayOrderForm: FormGroup;
  locationList = [];
  birthdayPackageList = [];

  constructor(private birthdaysService: BirthdaysService,
              private locationsService: LocationsService,
              private toastr: ToastrService,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.locationsService.getPureLocations()
    .subscribe(res => this.locationList = res as []);

    this.birthdaysService.getPureBirthdayPackages()
    .subscribe(res => this.birthdayPackageList = res as []);

    this.createBirthdayForm();
  }

  createBirthdayForm() {
    this.birthdayOrderForm = this.fb.group({
      clientName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(70)]],
      birthdayGirlBoyName: [null, [Validators.required, Validators.maxLength(30)]],
      contactEmail: [null, [Validators.required, Validators.maxLength(30)]],
      contactPhone: [null, [Validators.required, Validators.maxLength(30)]],
      numberOfGuests: ['', [Validators.required, Validators.min(1), Validators.max(500)]],
      birthdayNo: ['', [Validators.required, Validators.min(1), Validators.max(20)]],
      startDateAndTime: ['', Validators.required],
      remarks: ['', [Validators.maxLength(2000)]],
      branchId: [0, [Validators.min(1)]],
      birthdayPackageId: [0, [Validators.min(1)]]
    });
  }

  onSubmit() {
    this.birthdaysService.createBirthdayOrder(this.birthdayOrderForm.value).subscribe(() => {
      this.birthdayOrderForm.reset();
      this.toastr.success('Thank you for showing interest for our services. We will contact you as soon as possible.');    },
    error => {
      console.log(error);
    });
  }

}
