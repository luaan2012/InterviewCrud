import { staticUfs } from './../../helpers/static';
import { ClientService } from './../../services/client.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registerClient',
  templateUrl: './registerClient.component.html',
  styleUrls: ['./registerClient.component.css']
})
export class RegisterClientComponent implements OnInit {

  registerClient!: FormGroup;
  ufs = staticUfs
  showLine: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private clientService: ClientService,
    private router: Router
    ) {}

  ngOnInit() {
    // Inicializa o formulário
    this.registerClient = this.formBuilder.group({
      name: ['', [Validators.required, Validators.email]],
      lastName: ['', Validators.required],
      CPF: ['', Validators.required],
      RG: ['', Validators.required],
      dateBirthday: ['', Validators.required],
      email: ['', Validators.required],
      addresses: this.formBuilder.array([]),
      contacts: this.formBuilder.array([]),
    });

    // Adiciona um endereço e um contato inicialmente
    this.addAddress();
    this.addContact();
  }

  // Getter para obter o array de endereços
  // Getter para obter o array de endereços
  get addresses(): any {
    return (this.registerClient.get('addresses')) as any as FormArray;
  }

  toggleLineVisibility() {
    this.showLine = !this.showLine;
  }
  // Getter para obter o array de contatos
  get contacts(): any {
    return (this.registerClient.get('contacts') as any)as FormArray;
  }

  // Método para adicionar um novo endereço ao array
  addAddress() {
    if(this.addresses.length == 1)
      this.toggleLineVisibility()

    this.addresses.push(this.createAddressFormGroup());
  }

  // Método para criar um FormGroup para um endereço
  createAddressFormGroup(): FormGroup {
    return this.formBuilder.group({
      number: ['', [Validators.required]],
      complement: ['', [Validators.required]],
      neighborhood: ['', [Validators.required]],
      CEP: ['', [Validators.required]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      publicPlace: ['', [Validators.required]],
    });
  }

  // Método para adicionar um novo contato ao array
  addContact() {
    this.contacts.push(this.createContactFormGroup());
  }

  // Método para criar um FormGroup para um contato
  createContactFormGroup(): FormGroup {
    return this.formBuilder.group({
      nameContact: ['', [Validators.required]],
      typeContact: this.formBuilder.group({
        contact: ['', [Validators.required]],
        typeContactEnum: ['', [Validators.required]],
      }),
    });
  }

  onSubmit() {

    this.markFormFieldsAsTouched()

    if(!this.registerClient.valid)
      alert("Invalido")

    if (this.registerClient.valid) {
      this.clientService.add(this.registerClient.value).subscribe({
        next: (response) => {
          this.router.navigate(['viewClients'])
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

  markFormFieldsAsTouched() {
    Object.values(this.registerClient.controls).forEach(control => {
      control.markAsTouched();
    });
  }

}
