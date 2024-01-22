import { Address } from "./address"
import { Contact } from "./contact"

export interface Client {
  id: string,
  emailUser: string,
  email: string,
  name: string,
  lastName: string,
  cpf: string,
  rg: string,
  active: boolean,
  dateBirthday: string,
  addresses: Address[]
  contacts: Contact[]
}

