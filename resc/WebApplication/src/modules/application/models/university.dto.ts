import { SpecialityListItemDto } from "src/modules/lists/models/speciality-list/speciality-list-item.dto";
import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";

export class UniversityDto {
  institution: NomenclatureDto;
  specialityListItem: SpecialityListItemDto;

  rector: string;
}