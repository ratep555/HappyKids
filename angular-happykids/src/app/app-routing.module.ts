import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminManagerGuard } from './core/guards/admin-manager.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
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

  {path: 'happyblogs', loadChildren:
  () => import('./happyblogs/happyblogs.module').then(mod => mod.HappyblogsModule)},

  {path: 'locations', loadChildren:
  () => import('./locations/locations.module').then(mod => mod.LocationsModule)},

  {path: 'webshop', loadChildren:
  () => import('./webshop/webshop.module').then(mod => mod.WebshopModule)},

  {path: 'blogs', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/blogs/blogs.module').then(mod => mod.BlogsModule)},

  {path: 'branches', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/branches/branches.module').then(mod => mod.BranchesModule)},

  {path: 'categories', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/categories/categories.module').then(mod => mod.CategoriesModule)},

  {path: 'birthdaypackages', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/birthdaypackages/birthdaypackages.module').then(mod => mod.BirthdaypackagesModule)},

  {path: 'childrenitems', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/childrenitems/childrenitems.module').then(mod => mod.ChildrenitemsModule)},

  {path: 'childrenitemwarehouses', canActivate: [AdminManagerGuard], loadChildren:
  () => import('./admin/childrenitem-warehouses/childrenitem-warehouses.module').then(mod => mod.ChildrenitemWarehousesModule)},

  {path: 'discounts', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/discounts/discounts.module').then(mod => mod.DiscountsModule)},

  {path: 'kidactivities', loadChildren:
  () => import('./admin/kidactivities/kidactivities.module').then(mod => mod.KidactivitiesModule)},

  {path: 'manufacturers', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/manufacturers/manufacturers.module').then(mod => mod.ManufacturersModule)},

  {path: 'messages', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/messages/messages.module').then(mod => mod.MessagesModule)},

  {path: 'ordersbirthdays', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/orders-birthdays/orders-birthdays.module').then(mod => mod.OrdersBirthdaysModule)},

  {path: 'orderschildrenitems', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/orders-childrenitems/orders-childrenitems.module').then(mod => mod.OrdersChildrenitemsModule)},

  {path: 'orderstatuses', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/orderstatuses/orderstatuses.module').then(mod => mod.OrderstatusesModule)},

  {path: 'paymentoptions', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/paymentoptions/paymentoptions.module').then(mod => mod.PaymentoptionsModule)},

  {path: 'shippingoptions', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/shippingoptions/shippingoptions.module').then(mod => mod.ShippingoptionsModule)},

  {path: 'statistics', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/statistics/statistics.module').then(mod => mod.StatisticsModule)},

  {path: 'tags', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/tags/tags.module').then(mod => mod.TagsModule)},

  {path: 'userslist', canActivate: [AdminGuard],
  loadChildren: () => import('./admin/users-list/users-list.module').then(mod => mod.UsersListModule)},

  {path: 'warehouses', canActivate: [AdminManagerGuard],
  loadChildren: () => import('./admin/warehouses/warehouses.module').then(mod => mod.WarehousesModule)},

  {path: 'orderschildrenitemsclient', canActivate: [AuthGuard],
  loadChildren: () => import('./client/orders-childrenitems/orders-childrenitems.module').then(mod => mod.OrdersChildrenitemsModule)},

  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},

  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
