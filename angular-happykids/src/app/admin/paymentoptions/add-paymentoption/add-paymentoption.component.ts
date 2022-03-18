import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PaymentoptionsService } from '../paymentoptions.service';

@Component({
  selector: 'app-add-paymentoption',
  templateUrl: './add-paymentoption.component.html',
  styleUrls: ['./add-paymentoption.component.scss']
})
export class AddPaymentoptionComponent implements OnInit {
  paymentOptionForm: FormGroup;

  constructor(private paymentOptionsService: PaymentoptionsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.paymentOptionForm = this.fb.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(5), Validators.maxLength(1000)]],
     });
  }

  onSubmit() {
    if (this.paymentOptionForm.invalid) {
        return;
    }
    this.createPaymentOption();
  }

  private createPaymentOption() {
    this.paymentOptionsService.createPaymentOption(this.paymentOptionForm.value).subscribe(() => {
      this.router.navigateByUrl('paymentoptions');
    },
    error => {
      console.log(error);
    });
  }

}
