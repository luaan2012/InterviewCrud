import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SignInComponent } from './pages/signIn/signIn.component';
import { RegisterComponent } from './pages/register/register.component';
import { ViewClientsComponent } from './pages/viewClients/viewClients.component';
import { AppRoutingModule } from './app.routing.module';
import { RouterOutlet } from '@angular/router';
import { RegisterClientComponent } from './pages/registerClient/registerClient.component';

@NgModule({
  declarations: [
    AppComponent,
    SignInComponent,
    RegisterComponent,
    ViewClientsComponent,
    RegisterClientComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterOutlet,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
