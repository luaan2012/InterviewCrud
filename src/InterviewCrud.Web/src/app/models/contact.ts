export interface Contact {
  id: string,
  clientId: string,
  typeContactId: string,
  nameContact: string,
  contactNumber: string
  typeContact: TypeContact
}

export interface TypeContact {
  id: string,
  clientId: string,
  contact: string,
  typeContactEnum: TypeContactEnum
}

export enum TypeContactEnum {
  email,
  phone,
  cellPhone,
  whatsApp
}
