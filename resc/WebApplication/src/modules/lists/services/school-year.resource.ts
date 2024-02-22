import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { SpecialityListDto } from '../models/speciality-list/speciality-list.dto';

@Injectable()
export class SchoolYearResource extends BaseResource {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'SchoolYear');
  }

  createYear(schoolYearId: number): Observable<SpecialityListDto> {
    return this.http.post<SpecialityListDto>(`${this.baseUrl}/create`, schoolYearId);
  }

  getCurrentSchoolYear(): Observable<NomenclatureDto> {
    return this.http.get<NomenclatureDto>(`${this.baseUrl}/current`);
  }
}
