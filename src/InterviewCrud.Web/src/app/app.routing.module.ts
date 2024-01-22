import { RouterModule, Routes } from '@angular/router';
import { SignInComponent } from './pages/signIn/signIn.component';
import { RegisterComponent } from './pages/register/register.component';
import { RegisterClientComponent } from './pages/registerClient/registerClient.component';
import { ViewClientsComponent } from './pages/viewClients/viewClients.component';
import { NgModule } from '@angular/core';

 const routes: Routes = [
  { path: '', redirectTo: 'signin', pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
  { path: 'signin', component: SignInComponent },
  { path: 'registerClient', component: RegisterClientComponent },
  { path: 'viewClients', component: ViewClientsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
