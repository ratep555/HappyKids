import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'activities', loadChildren:
  () => import('./activities/activities.module').then(mod => mod.ActivitiesModule)},
  {path: 'basket', loadChildren:
   () => import('./basket/basket.module').then(mod => mod.BasketModule)},
  {path: 'billing', canActivate: [AuthGuard],
  loadChildren: () => import('./billing/billing.module').then(mod => mod.BillingModule)},
  {path: 'birthdays', loadChildren:
  () => import('./birthdays/birthdays.module').then(mod => mod.BirthdaysModule)},
  {path: 'locations', loadChildren:
  () => import('./locations/locations.module').then(mod => mod.LocationsModule)},
  {path: 'webshop', loadChildren:
  () => import('./webshop/webshop.module').then(mod => mod.WebshopModule)},
  {path: 'branches', loadChildren:
  () => import('./admin/branches/branches.module').then(mod => mod.BranchesModule)},
  {path: 'categories', loadChildren:
  () => import('./admin/categories/categories.module').then(mod => mod.CategoriesModule)},
  {path: 'birthdaypackages', loadChildren:
  () => import('./admin/birthdaypackages/birthdaypackages.module').then(mod => mod.BirthdaypackagesModule)},
  {path: 'childrenitems', loadChildren:
  () => import('./admin/childrenitems/childrenitems.module').then(mod => mod.ChildrenitemsModule)},
  {path: 'discounts', loadChildren:
  () => import('./admin/discounts/discounts.module').then(mod => mod.DiscountsModule)},
  {path: 'kidactivities', loadChildren:
  () => import('./admin/kidactivities/kidactivities.module').then(mod => mod.KidactivitiesModule)},
  {path: 'manufacturers', loadChildren:
  () => import('./admin/manufacturers/manufacturers.module').then(mod => mod.ManufacturersModule)},
  {path: 'ordersbirthdays', loadChildren:
  () => import('./admin/orders-birthdays/orders-birthdays.module').then(mod => mod.OrdersBirthdaysModule)},
  {path: 'orderschildrenitems', loadChildren:
  () => import('./admin/orders-childrenitems/orders-childrenitems.module').then(mod => mod.OrdersChildrenitemsModule)},
  {path: 'paymentoptions', loadChildren:
  () => import('./admin/paymentoptions/paymentoptions.module').then(mod => mod.PaymentoptionsModule)},
  {path: 'shippingoptions', loadChildren:
  () => import('./admin/shippingoptions/shippingoptions.module').then(mod => mod.ShippingoptionsModule)},
  {path: 'tags', loadChildren:
  () => import('./admin/tags/tags.module').then(mod => mod.TagsModule)},
  {path: 'warehouses', loadChildren:
  () => import('./admin/warehouses/warehouses.module').then(mod => mod.WarehousesModule)},
  {path: 'childrenitemwarehouses', loadChildren: () =>
  import('./admin/childrenitem-warehouses/childrenitem-warehouses.module').then(mod => mod.ChildrenitemWarehousesModule)},
  {path: 'orderschildrenitemsclient', loadChildren:
  () => import('./client/orders-childrenitems/orders-childrenitems.module').then(mod => mod.OrdersChildrenitemsModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
