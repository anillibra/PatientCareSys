import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HeaderComponent } from './Header/Header.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './security/login/login.component';
import { HasClaimDirective } from './security/has-claim.directive';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './security/auth.guard';
import { HttpInterceptorModule } from './security/http-interceptor.module';

@NgModule({
   declarations: [
      AppComponent,
      DashboardComponent,
      HeaderComponent,
      LoginComponent,
      HasClaimDirective,
      HomeComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      HttpClientModule,
      AppRoutingModule,
      HttpInterceptorModule,
   ],
   providers: [AuthGuard],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
