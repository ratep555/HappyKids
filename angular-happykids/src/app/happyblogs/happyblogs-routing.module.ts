import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HappyblogsComponent } from './happyblogs.component';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { RouterModule, Routes } from '@angular/router';



const routes: Routes = [
  {path: '', component: HappyblogsComponent},
  {path: 'blogdetail/:id', component: BlogDetailComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class HappyblogsRoutingModule { }
