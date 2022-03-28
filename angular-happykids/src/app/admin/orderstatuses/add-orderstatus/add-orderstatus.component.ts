import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { OrderstatusesService } from '../orderstatuses.service';

@Component({
  selector: 'app-add-orderstatus',
  templateUrl: './add-orderstatus.component.html',
  styleUrls: ['./add-orderstatus.component.scss']
})
export class AddOrderstatusComponent implements OnInit {
  orderstatusForm: FormGroup;

  constructor(private orderstatusesService: OrderstatusesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.orderstatusForm = this.fb.group({
      name: ['', [Validators.required]]
     });
  }

  onSubmit() {
    if (this.orderstatusForm.invalid) {
        return;
    }
    this.createOrderStatus();
  }

  private createOrderStatus() {
    this.orderstatusesService.createOrderStatus(this.orderstatusForm.value).subscribe(() => {
      this.router.navigateByUrl('orderstatuses');
    },
    error => {
      console.log(error);
    });
  }

}
