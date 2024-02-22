import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";
import { EmployerSpecialityDto } from "./employer-speciality-dto";

export class EmployerListItemDto {
  id: number;
  employerListId: number;
  name: string;
  bulstat: string;
  city: NomenclatureDto;
  address: string;
  specialities: EmployerSpecialityDto[] = [];
  studentsCount: number;

  fullAddress: string;

  constructor() {
    this.specialities = [new EmployerSpecialityDto()];
  }
}