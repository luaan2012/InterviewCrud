import { IdentityService } from './../../services/identity.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm = this.formBuilder.group({
    userName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email] ],
    phoneNumber: ['', [Validators.required,
      Validators.minLength(10),
      Validators.maxLength(11)]],
    password: ['', Validators.required],
    confirmPassword: ['', [Validators.required]],
    profilePhoto: [''],
    base64Profile: [null],
    termAgree: [false, Validators.required]
  });

  constructor(
    private formBuilder: FormBuilder,
    private IdentityService: IdentityService,
    private router: Router
  ) { }

  ngOnInit() {

  }

  onSubmit() {

    this.markFormFieldsAsTouched()

    if(!this.registerForm.valid)
      alert("Invalido")

      const formData = new FormData();
      formData.append('profilePhoto', this.registerForm.get('profilePhoto')?.value!);

      console.log(formData)

    if (this.registerForm.valid) {
      const requestRegister = this.registerForm.value;

      this.IdentityService.register(requestRegister).subscribe({
        next: () => {
          alert('Sucesso')
          this.router.navigate(['signin'])
        },
        error: (error) => {
          alert(error)
        },
        complete: () => {
          setTimeout(() => {
          }, 2000)
        }
      });
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    if (file && this.isImage(file)) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.registerForm.patchValue({
          profilePhoto: file.name,
          base64Profile: e.target.result.split(',')[1]
        });
      };
      reader.readAsDataURL(file);
    }else {
      alert('Por favor, selecione uma imagem válida.');
      this.registerForm.get('base64Profile')?.setValue(null);
    }
  }

  markFormFieldsAsTouched() {
    Object.values(this.registerForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  private isImage(file: File): boolean {
    const allowedExtensions = ['jpg', 'jpeg', 'png', 'gif'];
    const extension = file.name?.split('.')?.pop()?.toLowerCase();

    return allowedExtensions.includes(extension!) && file.type.startsWith('image/');
  }
}
