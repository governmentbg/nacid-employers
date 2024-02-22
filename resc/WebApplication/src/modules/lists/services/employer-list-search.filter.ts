import { BaseSearchFilter } from "src/infrastructure/services/base-search.filter";
import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";

export class EmployerListSearchFilter extends BaseSearchFilter {
  schoolYearId: number;

  speciality: NomenclatureDto;
  specialityName: string;
  specialityId: number;

  bulstat: string;

  company: NomenclatureDto
  companyId: number;

  constructor() {
    super(200);
  }
}