import { ContactType } from "../enums/contact-type.enum";

export class ContactPerson {
  id: number | null;
  name: string;
  email: string;
  phoneNumber: string;
  type: ContactType;

  constructor(type: ContactType) {
    this.type = type;
  }
}