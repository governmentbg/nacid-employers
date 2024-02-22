import { AttachedFile } from "src/infrastructure/models/attached-file.model";
import { TaxType } from "../enums/tax-type.enum";
import { ContactPerson } from "./contact-person.dto";

export class ContractDto {
  signingDate: Date;
  endDate: Date;
  number: string;
  term: string;
  employmentTerm: string;
  taxType: TaxType;

  attachedFile: AttachedFile;
  contacts: ContactPerson[] = [];
}