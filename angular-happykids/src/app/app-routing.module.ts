import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'webshop', loadChildren:
  () => import('./webshop/webshop.module').then(mod => mod.WebshopModule)},
  {path: 'childrenitems', loadChildren:
  () => import('./admin/childrenitems/childrenitems.module').then(mod => mod.ChildrenitemsModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
