import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto"

export class SchoolYearDto extends NomenclatureDto {
  isCurrent: boolean;
  primaryYear: number;
}