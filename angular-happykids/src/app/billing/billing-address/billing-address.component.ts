import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { WarehousesService } from 'src/app/admin/warehouses/warehouses.service';
import { Address } from 'src/app/shared/models/address';

@Component({
  selector: 'app-billing-address',
  templateUrl: './billing-address.component.html',
  styleUrls: ['./billing-address.component.scss']
})
export class BillingAddressComponent implements OnInit {
  @Input() billingForm: FormGroup;
  countryList = [];

  constructor(private warehousesService: WarehousesService,
              private accountService: AccountService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.warehousesService.getCountries()
    .subscribe(res => this.countryList = res as []);
  }

  updateClientAddress() {
    this.accountService.updateClientAddress(this.billingForm.get('addressForm').value)
      .subscribe((address: Address) => {
        this.toastr.success('Address saved');
        this.billingForm.get('addressForm').reset(address);
      }, error => {
        this.toastr.error(error.message);
        console.log(error);
      });
  }

}
