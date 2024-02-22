import { NomenclatureDto } from "./nomenclature.dto";

export class SchoolYearDto extends NomenclatureDto {
  isCurrent: boolean;
  primaryYear: number;
}