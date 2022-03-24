import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BirthdaysService } from 'src/app/birthdays/birthdays.service';
import { LocationsService } from 'src/app/locations/locations.service';
import { BirthdayOrderEdit } from 'src/app/shared/models/birthdayorder';
import { OrdersChildrenitemsService } from '../../orders-childrenitems/orders-childrenitems.service';
import { OrdersBirthdaysService } from '../orders-birthdays.service';

@Component({
  selector: 'app-edit-order-birthdays',
  templateUrl: './edit-order-birthdays.component.html',
  styleUrls: ['./edit-order-birthdays.component.scss']
})
export class EditOrderBirthdaysComponent implements OnInit {
  birthdayOrderForms: FormArray = this.fb.array([]);
  id: number;
  locationList = [];
  birthdayPackageList = [];
  orderstatusesList = [];


  constructor(private birthdaysService: BirthdaysService,
              private locationsService: LocationsService,
              private orderChildrenitemsService: OrdersChildrenitemsService,
              private orderBirthdaysService: OrdersBirthdaysService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.locationsService.getPureLocations()
    .subscribe(res => this.locationList = res as []);

    this.birthdaysService.getPureBirthdayPackages()
    .subscribe(res => this.birthdayPackageList = res as []);

    this.orderChildrenitemsService.getOrderStatuses()
    .subscribe(res => this.orderstatusesList = res as []);

    this.orderBirthdaysService.getBirthdayOrderById(this.id).subscribe(
      (birthday: BirthdayOrderEdit) => {
      this.birthdayOrderForms.push(this.fb.group({
        id: [this.id],
        clientName: [birthday.clientName],
        birthdayGirlBoyName: [birthday.birthdayGirlBoyName],
        birthdayNo: [birthday.birthdayNo],
        price: [birthday.price],
        numberOfGuests: [birthday.numberOfGuests],
        contactEmail: [birthday.contactEmail],
        contactPhone: [birthday.contactPhone],
        birthdayPackageId: [birthday.birthdayPackageId],
        branchId: [birthday.branchId],
        orderStatusId: [birthday.orderStatusId],
        remarks: [birthday.remarks],
        startDateAndTime: [new Date(birthday.startDateAndTime)],
        endDateAndTime: [new Date(birthday.endDateAndTime)]
        }));
      });

  }

    recordSubmit(fg: FormGroup) {
      this.orderBirthdaysService.updateBirthdayOrder(fg.value).subscribe(
          (res: any) => {
            this.router.navigateByUrl('ordersbirthdays');
          }, error => {
              console.log(error);
            });
          }

}
