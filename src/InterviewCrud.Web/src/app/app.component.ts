
import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { signOut, isLogged } from './helpers/storage';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent  {

  constructor(
    private router: Router
  ){}

  title = 'interview-web';

  signOutPage() {
    signOut()
    this.router.navigate(['signin'])
  }

  isLoggedPage() {
    return isLogged()
  }
}
