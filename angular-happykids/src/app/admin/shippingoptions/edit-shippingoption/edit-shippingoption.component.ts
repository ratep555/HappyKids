import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ShippingOption } from 'src/app/shared/models/shippingOption';
import { ShippingoptionsService } from '../shippingoptions.service';

@Component({
  selector: 'app-edit-shippingoption',
  templateUrl: './edit-shippingoption.component.html',
  styleUrls: ['./edit-shippingoption.component.scss']
})
export class EditShippingoptionComponent implements OnInit {
  shippingOptionForm: FormGroup;
  shippingOption: ShippingOption;
  id: number;

  constructor(public shippingOptionsService: ShippingoptionsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.shippingOptionForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]],
      transitDays: ['', [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(5), Validators.maxLength(1000)]],     });

    this.shippingOptionsService.getShippingOptionById
    (this.id)
    .pipe(first())
    .subscribe(x => this.shippingOptionForm.patchValue(x));
  }

  onSubmit() {
    if (this.shippingOptionForm.invalid) {
        return;
    }
    this.updateShippingOption();
  }

  private updateShippingOption() {
    this.shippingOptionsService.updateShippingOption
    (this.id, this.shippingOptionForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('shippingoptions');
          }, error => {
            console.log(error);
          });
        }

}
