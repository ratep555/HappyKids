import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ShippingoptionsService } from '../shippingoptions.service';

@Component({
  selector: 'app-add-shippingoption',
  templateUrl: './add-shippingoption.component.html',
  styleUrls: ['./add-shippingoption.component.scss']
})
export class AddShippingoptionComponent implements OnInit {
  shippingOptionForm: FormGroup;

  constructor(private shippingOptionsService: ShippingoptionsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.shippingOptionForm = this.fb.group({
      name: ['', [Validators.required]],
      transitDays: ['', [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(5), Validators.maxLength(1000)]],
     });
  }

  onSubmit() {
    if (this.shippingOptionForm.invalid) {
        return;
    }
    this.createShippingOption();
  }

  private createShippingOption() {
    this.shippingOptionsService.createShippingOption(this.shippingOptionForm.value).subscribe(() => {
      this.router.navigateByUrl('shippingoptions');
    },
    error => {
      console.log(error);
    });
  }

}
