import { Observable } from 'rxjs';
// sign-in.component.ts

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../../services/identity.service';
import { getUser, isLogged, saveUser } from '../../helpers/storage';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './signIn.component.html',
  styleUrls: ['./signIn.component.css'],
})
export class SignInComponent implements OnInit {
  loginForm =  this.formBuilder.group({
    emailOrUsername: ['', [Validators.required]],
    password: ['', Validators.required],
  });

  constructor(
    private IdentityService: IdentityService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    if(isLogged())
      this.router.navigate(['/viewClients'])
  }

  onSubmit() {

    this.markFormFieldsAsTouched()

    if(!this.loginForm.valid)
      alert("Invalido")

    if (this.loginForm.valid) {
      const { emailOrUsername, password } = this.loginForm.value;
      this.IdentityService.login(emailOrUsername || '', password || '').subscribe({
        next: (response) => {
          saveUser(response)
          this.router.navigate(['viewClients'])
        },
        error: (error) => {
          alert(error?.error || 'Algo deu errado.')
        },
        complete: () => {

        }
      });
    }
  }

  markFormFieldsAsTouched() {
    Object.values(this.loginForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }
}
