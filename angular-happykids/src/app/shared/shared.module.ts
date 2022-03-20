import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TextInputComponent } from './components/text-input/text-input.component';
import { PagerComponent } from './components/pager/pager.component';
import { MultipleSelectorComponent } from './components/multiple-selector/multiple-selector.component';
import { ImgInputComponent } from './components/img-input/img-input.component';
import { BasketReviewComponent } from './components/basket-review/basket-review.component';
import { OrderSumComponent } from './components/order-sum/order-sum.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CdkStepperComponent } from './components/cdk-stepper/cdk-stepper.component';
import { RatingComponent } from './components/rating/rating.component';


@NgModule({
  declarations: [
    TextInputComponent,
    PagerComponent,
    MultipleSelectorComponent,
    ImgInputComponent,
    BasketReviewComponent,
    OrderSumComponent,
    CdkStepperComponent,
    RatingComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    CdkStepperModule

  ],
  exports: [
    BsDropdownModule,
    CollapseModule,
    ReactiveFormsModule,
    FormsModule,
    TabsModule,
    BsDatepickerModule,
    PaginationModule,
    ModalModule,
    TextInputComponent,
    PagerComponent,
    MultipleSelectorComponent,
    ImgInputComponent,
    BasketReviewComponent,
    OrderSumComponent,
    CdkStepperModule,
    CdkStepperComponent,
    RatingComponent
  ]
})
export class SharedModule { }
