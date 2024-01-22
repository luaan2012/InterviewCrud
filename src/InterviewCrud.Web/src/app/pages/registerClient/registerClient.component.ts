import { TypeContact } from './../../models/contact';
import { User } from './../../models/token';
import { contactTypeOptions, staticUfs } from './../../helpers/static';
import { ClientService } from './../../services/client.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { getUser } from '../../helpers/storage';
import { Client } from '../../models/client';

@Component({
  selector: 'app-registerClient',
  templateUrl: './registerClient.component.html',
  styleUrls: ['./registerClient.component.css']
})
export class RegisterClientComponent implements OnInit {

  registerClient!: FormGroup;
  ufs = staticUfs
  contactType = contactTypeOptions
  showLineAddress: boolean = false;
  showLineContact: boolean = false;
  user = getUser()
  client!: Client
  edit: boolean = false

  constructor(
    private formBuilder: FormBuilder,
    private clientService: ClientService,
    private route: ActivatedRoute,
    private router: Router
    ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const clientId = params['id'];

      if (clientId) {
        this.edit = true
        this.clientService.getById(clientId).subscribe({
          next: (client: Client) => {
            if (client) {
              this.client = client;
              this.populateFormWithClientData(client);
            } else {
              this.router.navigate(['/viewClients']);
            }
          },
          error: error => {
            console.error('Erro ao obter informações do cliente:', error);
          }
        });
      }
    });

    this.registerClient = this.formBuilder.group({
      name: ['', [Validators.required]],
      lastName: ['', Validators.required],
      cpf: ['', Validators.required],
      rg: ['', Validators.required],
      dateBirthday: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      addresses: this.formBuilder.array([]),
      contacts: this.formBuilder.array([]),
      userId: this.user?.userId,
      emailUser: this.user?.email
    });

    if(!this.edit){
      this.addAddress();
      this.addContact();
    }

  }

  private populateFormWithClientData(client: Client): void {
    this.registerClient = this.formBuilder.group({
      name: [client.name, [Validators.required]],
      lastName: [client.lastName, Validators.required],
      cpf: [client.cpf, Validators.required],
      rg: [client.rg, Validators.required],
      dateBirthday: [new Date(client.dateBirthday).toISOString().split('T')[0], Validators.required],
      email: [client.email, [Validators.required, Validators.email]],
      id: [client.id],
      addresses: this.formBuilder.array([]),
      contacts: this.formBuilder.array([]),
    });

    client.addresses.forEach(address => {
      this.addresses.push(this.formBuilder.group(address));
    });

    client.contacts.forEach(contact => {
      const contactGroup = this.formBuilder.group({
        id: [contact.id],
        clientId: [contact.clientId],
        typeContactId: [contact.typeContactId],
        nameContact: [contact.nameContact, [Validators.required]],
        typeContact: this.formBuilder.group({
          id: [contact.typeContact.id],
          clientId: [contact.typeContact.clientId],
          contact: [contact.typeContact.contact, [Validators.required]],
          typeContactEnum: [contact.typeContact.typeContactEnum, [Validators.required]],
        }),
      });

      this.contacts.push(contactGroup);
    });
  }

  get addresses(): any {
    return (this.registerClient.get('addresses')) as any as FormArray;
  }

  toggleLineVisibilityAddress() {
    this.showLineAddress = !this.showLineAddress;
  }

  toggleLineVisibilityContact() {
    this.showLineContact = !this.showLineContact;
  }

  get contacts(): any {
    return (this.registerClient.get('contacts') as any)as FormArray;
  }

  // get typeContacts(): any {
  //   return (this.registerClient.get('contacts')?.get('typeContact') as any)as FormArray;
  // }

  addAddress() {
    if(this.addresses.length == 1)
      this.toggleLineVisibilityAddress()

    this.addresses.push(this.createAddressFormGroup());
  }

  createAddressFormGroup(): FormGroup {
    return this.formBuilder.group({
      number: ['', [Validators.required]],
      complement: ['', [Validators.required]],
      cep: ['', [Validators.required]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      publicPlace: ['', [Validators.required]],
    });
  }

  addContact() {
    if(this.contacts.length == 1)
      this.toggleLineVisibilityContact()

    this.contacts.push(this.createContactFormGroup());
  }

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

    const requestData = { ...this.registerClient.value };

    if (this.registerClient.valid && !this.edit) {

      requestData.contacts.forEach((contact: any) => {
        contact.typeContact.typeContactEnum = +contact.typeContact.typeContactEnum;
      });

      this.clientService.add(requestData).subscribe({
        next: () => {
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

    if(this.registerClient.valid && this.edit){
      requestData.contacts.forEach((contact: any) => {
        contact.typeContact.typeContactEnum = +contact.typeContact.typeContactEnum;
      });

      this.clientService.edit(this.client.id, requestData).subscribe({
        next: () => {
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
