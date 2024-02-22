import { EmployerListItemDto } from "src/modules/lists/models/employers-list/employer-list-item.dto";

export class EmployerDto {
  employerListItem: EmployerListItemDto;

  representative: string;
  email: string;
  phoneNumber: string;
}