import { NomenclatureDto } from "../nomenclature.dto";
import { EmployerListItemDto } from "./employer-list-item.dto";

export class EmployerListDto {
  id: number;
  items: EmployerListItemDto[] = [];
  isPublished: boolean;
  schoolYear: NomenclatureDto;
}