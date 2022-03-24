import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Branch } from 'src/app/shared/models/branch';
import { CoordinatesMap } from 'src/app/shared/models/coordinate';
import { WarehousesService } from '../../warehouses/warehouses.service';
import { BranchesService } from '../branches.service';

@Component({
  selector: 'app-add-branch',
  templateUrl: './add-branch.component.html',
  styleUrls: ['./add-branch.component.scss']
})
export class AddBranchComponent implements OnInit {
  branchForm: FormGroup;
  countryList = [];
  initialCoordinates: CoordinatesMap[] = [];
  model: Branch;

  constructor(private branchesService: BranchesService,
              private warehousesService: WarehousesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.warehousesService.getCountries()
    .subscribe(res => this.countryList = res as []);

    this.createBranchForm();
  }

  createBranchForm() {
    this.branchForm = this.fb.group({
      street: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      city: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      email: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      phone: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      workingHours: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      longitude: ['', [Validators.required]],
      latitude: ['', [Validators.required]],
      countryId: [0, [Validators.min(1)]]
    });
  }

  onSubmit() {
    this.branchesService.createBranch(this.branchForm.value).subscribe(() => {
      this.router.navigateByUrl('branches');
    },
    error => {
      console.log(error);
    });
  }

  onSelectedLocation(coordinates: CoordinatesMap) {
    this.branchForm.patchValue(coordinates);
 }

}
