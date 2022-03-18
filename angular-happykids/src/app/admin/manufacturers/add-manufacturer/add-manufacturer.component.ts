import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ManufacturersService } from '../manufacturers.service';

@Component({
  selector: 'app-add-manufacturer',
  templateUrl: './add-manufacturer.component.html',
  styleUrls: ['./add-manufacturer.component.scss']
})
export class AddManufacturerComponent implements OnInit {
  manufacturerForm: FormGroup;

  constructor(private manufacturersService: ManufacturersService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.manufacturerForm = this.fb.group({
      name: ['', [Validators.required]]
     });
  }

  onSubmit() {
    if (this.manufacturerForm.invalid) {
        return;
    }
    this.createManufacturer();
  }

  private createManufacturer() {
    this.manufacturersService.createManufacturer(this.manufacturerForm.value).subscribe(() => {
      this.router.navigateByUrl('manufacturers');
    },
    error => {
      console.log(error);
    });
  }

}
