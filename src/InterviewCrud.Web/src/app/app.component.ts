import { environment } from './../environments/environment';

import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { signOut, isLogged, getUser } from './helpers/storage';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit  {
  user = getUser()
  urlImage: string = ''

  constructor(
    private router: Router
  ){}
  ngOnInit(): void {
    if(isLogged()){
      this.urlImage = environment.application.urlIdentity + 'images/' + this.user?.profileImage
    }
  }

  isRouteActive(route: string): boolean {
    return this.router.isActive(route, true);
  }

  title = 'interview-web';

  signOutPage() {
    signOut()
    this.router.navigate(['signin'])
  }

  isLoggedPage() {
    return isLogged()
  }
}
