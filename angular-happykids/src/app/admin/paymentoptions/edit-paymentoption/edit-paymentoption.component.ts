import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { PaymentOption } from 'src/app/shared/models/paymentOption';
import { PaymentoptionsService } from '../paymentoptions.service';

@Component({
  selector: 'app-edit-paymentoption',
  templateUrl: './edit-paymentoption.component.html',
  styleUrls: ['./edit-paymentoption.component.scss']
})
export class EditPaymentoptionComponent implements OnInit {
  paymentOptionForm: FormGroup;
  paymentOption: PaymentOption;
  id: number;

  constructor(private paymentOptionsService: PaymentoptionsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.paymentOptionForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(5), Validators.maxLength(1000)]],     });

    this.paymentOptionsService.getPaymentOptionById
    (this.id)
    .pipe(first())
    .subscribe(x => this.paymentOptionForm.patchValue(x));
  }

  onSubmit() {
    if (this.paymentOptionForm.invalid) {
        return;
    }
    this.updatePaymentOption();
  }

  private updatePaymentOption() {
    this.paymentOptionsService.updatePaymentOption
    (this.id, this.paymentOptionForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('paymentoptions');
          }, error => {
            console.log(error);
          });
        }

}
