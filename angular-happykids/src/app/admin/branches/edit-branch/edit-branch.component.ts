import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Marker } from 'leaflet';
import { first } from 'rxjs/operators';
import { Branch } from 'src/app/shared/models/branch';
import { CoordinatesMap, CoordinatesMapWithMessage } from 'src/app/shared/models/coordinate';
import { WarehousesService } from '../../warehouses/warehouses.service';
import { BranchesService } from '../branches.service';

@Component({
  selector: 'app-edit-branch',
  templateUrl: './edit-branch.component.html',
  styleUrls: ['./edit-branch.component.scss']
})
export class EditBranchComponent implements OnInit {
  model: Branch;
  branchForm: FormGroup;
  countryList = [];
  id: number;
  initialCoordinates: CoordinatesMap[] = [];
  coordinates: CoordinatesMapWithMessage[] = [];
  layers: Marker<any>[] = [];

  constructor(private branchesService: BranchesService,
              private warehousesService: WarehousesService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) { }

ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];

  this.activatedRoute.params.subscribe((params) => {
    this.branchesService.getBranchById(params.id).subscribe((branch) => {
      console.log(branch);
      this.model = branch;
      this.coordinates = [{latitude: branch.latitude, longitude: branch.longitude, message: branch.street}];
      console.log(this.coordinates);
    });
  });

  this.warehousesService.getCountries()
    .subscribe(res => this.countryList = res as []);

  this.branchForm = this.fb.group({
    id: [this.id],
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

  this.branchesService.getBranchById(this.id)
  .pipe(first())
  .subscribe(x => this.branchForm.patchValue(x));
}

onSubmit() {
  if (this.branchForm.invalid) {
      return;
  }
  this.updateLocation();
}

private updateLocation() {
this.branchesService.updateBranch(this.id, this.branchForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('branches');
      }, error => {
        console.log(error);
      });
    }

 onSelectedLocation(coordinates: CoordinatesMap) {
  this.branchForm.patchValue(coordinates);
  this.initialCoordinates.push({latitude: this.model.latitude,
    longitude: this.model.longitude});
   }

}
