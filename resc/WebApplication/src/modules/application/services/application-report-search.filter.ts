import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ReportType } from '../enums/report-type.enum';

@Injectable()
export class ApplicationReportSearchFilter extends BaseSearchFilter {
  institutionId: number;
  institution: NomenclatureDto;
  institutionName: string;

  researchAreaId: number;
  researchArea: NomenclatureDto;
  researchAreaName: string;

  specialityId: number;
  speciality: any;
  specialityName: string;

  educationalQualificationId: number;
  educationalQualification: NomenclatureDto;
  educationalQualificationName: string;

  schoolYearId: number;
  schoolYear: NomenclatureDto;
  schoolYearName: string;

  createdReportDate: Date;

  reportType: number;

  constructor() {
    super(10);

    this.reportType = ReportType.defaultReport;
  }
}
