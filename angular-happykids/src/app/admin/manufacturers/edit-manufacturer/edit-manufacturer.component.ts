import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Manufacturer } from 'src/app/shared/models/manufacturer';
import { ManufacturersService } from '../manufacturers.service';

@Component({
  selector: 'app-edit-manufacturer',
  templateUrl: './edit-manufacturer.component.html',
  styleUrls: ['./edit-manufacturer.component.scss']
})
export class EditManufacturerComponent implements OnInit {
  manufacturerForm: FormGroup;
  manufacturer: Manufacturer;
  id: number;

  constructor(public manufacturersService: ManufacturersService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.manufacturerForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]]
     });

    this.manufacturersService.getManufacturerById
    (this.id)
    .pipe(first())
    .subscribe(x => this.manufacturerForm.patchValue(x));
  }

  onSubmit() {
    if (this.manufacturerForm.invalid) {
        return;
    }
    this.updateManufacturer();
  }

  private updateManufacturer() {
    this.manufacturersService.updateManufacturer
    (this.id, this.manufacturerForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('manufacturers');
          }, error => {
            console.log(error);
          });
        }

}
