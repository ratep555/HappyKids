import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiscountsComponent } from './discounts.component';
import { AddDiscountComponent } from './add-discount/add-discount.component';
import { EditDiscountComponent } from './edit-discount/edit-discount.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { DiscountsRoutingModule } from './discounts-routing.module';



@NgModule({
  declarations: [
    DiscountsComponent,
    AddDiscountComponent,
    EditDiscountComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    DiscountsRoutingModule
  ]
})
export class DiscountsModule { }
