import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { OrderstatusesService } from '../orderstatuses.service';

@Component({
  selector: 'app-edit-orderstatus',
  templateUrl: './edit-orderstatus.component.html',
  styleUrls: ['./edit-orderstatus.component.scss']
})
export class EditOrderstatusComponent implements OnInit {
  orderstatusForm: FormGroup;
  orderstatus: OrderStatus;
  id: number;

  constructor(private orderstatusesService: OrderstatusesService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.orderstatusForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]]
     });

    this.orderstatusesService.getOrderStatusById
    (this.id)
    .pipe(first())
    .subscribe(x => this.orderstatusForm.patchValue(x));
  }

  onSubmit() {
    if (this.orderstatusForm.invalid) {
        return;
    }
    this.updateOrderStatus();
  }

  private updateOrderStatus() {
    this.orderstatusesService.updateOrderStatus
    (this.id, this.orderstatusForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('orderstatuses');
          }, error => {
            console.log(error);
          });
        }

}
