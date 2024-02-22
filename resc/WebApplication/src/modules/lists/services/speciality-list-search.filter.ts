import { BaseSearchFilter } from "src/infrastructure/services/base-search.filter";
import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";

export class SpecialityListSearchFilter extends BaseSearchFilter {
  schoolYearId: number;

  speciality: NomenclatureDto;
  specialityName: string;
  specialityId: number;

  institution: NomenclatureDto;
  institutionId: number;

  researchArea: NomenclatureDto;
  researchAreaId: number;

  constructor() {
    super(200);
  }
}