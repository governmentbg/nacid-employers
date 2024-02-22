import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { SpecialityListItemDto } from '../models/speciality-list/speciality-list-item.dto';
import { SpecialityListSearchFilter } from '../services/speciality-list-search.filter';
import { SpecialityListDto } from '../models/speciality-list/speciality-list.dto';
import { BaseListFilter } from './base-list.filter';

@Injectable()
export class SpecialityListResource extends BaseResource
  implements IFilterable<SpecialityListSearchFilter, SpecialityListItemDto> {
  constructor(
    protected override http: HttpClient,
    protected override configuration: Configuration
  ) {
    super(http, configuration, 'SpecialityPublicList');
  }
  getFiltered(filter?: SpecialityListSearchFilter): Observable<SpecialityListItemDto[]> {
    return this.http.get<SpecialityListItemDto[]>(`${this.baseUrl}/search${this.composeQueryString(filter)}`);
  }

  getSpecialityList(filter?: BaseListFilter): Observable<SpecialityListDto> {
    return this.http.get<SpecialityListDto>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }
}
