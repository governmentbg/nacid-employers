import { NomenclatureDto } from "../nomenclature.dto";
import { SpecialityMinisterDto } from "./speciality-minister.dto";

export class SpecialityListItemDto {
  id: number | null;
  specialityListId: number;
  speciality: NomenclatureDto;
  researchArea: NomenclatureDto;
  institution: NomenclatureDto;
  educationalQualification: NomenclatureDto;
  educationFormType: NomenclatureDto;
  specialityMinisters: SpecialityMinisterDto[] = [];
  studentsCount: number;

  constructor() {
    this.specialityMinisters = [new SpecialityMinisterDto()];
  }
}