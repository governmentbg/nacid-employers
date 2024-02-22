import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";
import { EmployerListItemDto } from "./employer-list-item.dto";

export class EmployerListDto {
  id: number;
  items: EmployerListItemDto[] = [];
  isPublished: boolean;
  schoolYear: NomenclatureDto;
}