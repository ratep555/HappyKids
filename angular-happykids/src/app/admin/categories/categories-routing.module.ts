import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesComponent } from './categories.component';
import { AddCategoryComponent } from './add-category/add-category.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';



const routes: Routes = [
  {path: '', component: CategoriesComponent},
  {path: 'addcategory', component: AddCategoryComponent},
  {path: 'editcategory/:id', component: EditCategoryComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class CategoriesRoutingModule { }
