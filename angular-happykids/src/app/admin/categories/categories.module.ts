import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories.component';
import { AddCategoryComponent } from './add-category/add-category.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CategoriesRoutingModule } from './categories-routing.module';



@NgModule({
  declarations: [
    CategoriesComponent,
    AddCategoryComponent,
    EditCategoryComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CategoriesRoutingModule
  ]
})
export class CategoriesModule { }
