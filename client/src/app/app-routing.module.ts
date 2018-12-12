import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './security/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './security/auth.guard';

const routes: Routes = [
  // {
  //   path: '',
  //   component: HomeComponent,
  // },
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: { claimType: 'canReadCareHome' }
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: { claimType: 'canReadCareHome' }
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '**', component: HomeComponent
  }
];
// , {useHash: true}
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
