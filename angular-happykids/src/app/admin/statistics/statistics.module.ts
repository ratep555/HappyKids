import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { ChildrenitemOrderChartComponent } from './childrenitem-order-chart/childrenitem-order-chart.component';
import { BirthdaypackagesOrderChartComponent } from './birthdaypackages-order-chart/birthdaypackages-order-chart.component';



@NgModule({
  declarations: [
    StatisticsComponent,
    ChildrenitemOrderChartComponent,
    BirthdaypackagesOrderChartComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    StatisticsRoutingModule
  ]
})
export class StatisticsModule { }
