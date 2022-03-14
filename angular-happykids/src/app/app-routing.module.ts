import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'webshop', loadChildren:
  () => import('./webshop/webshop.module').then(mod => mod.WebshopModule)},
  {path: 'childrenitems', loadChildren:
  () => import('./admin/childrenitems/childrenitems.module').then(mod => mod.ChildrenitemsModule)},
  {path: 'childrenitemwarehouses', loadChildren: () =>
  import('./admin/childrenitem-warehouses/childrenitem-warehouses.module').then(mod => mod.ChildrenitemWarehousesModule)},
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
