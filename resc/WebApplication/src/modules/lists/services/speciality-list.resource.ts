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
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';

@Injectable()
export class SpecialityListResource extends BaseResource
  implements IFilterable<SpecialityListSearchFilter, SearchResultItemDto<SpecialityListItemDto>> {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'SpecialityList');
  }

  getFiltered(filter?: SpecialityListSearchFilter): Observable<SearchResultItemDto<SpecialityListItemDto>> {
    return this.http.get<SearchResultItemDto<SpecialityListItemDto>>(`${this.baseUrl}/search${this.composeQueryString(filter)}`);
  }

  getSpecialityList(filter?: BaseListFilter): Observable<SpecialityListDto> {
    return this.http.get<SpecialityListDto>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  getSpeciality(specialityId: number): Observable<SpecialityListItemDto> {
    return this.http.get<SpecialityListItemDto>(`${this.baseUrl}/${specialityId}`);
  }

  editSpeciality(speciality: SpecialityListItemDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, speciality);
  }

  addSpeciality(speciality: SpecialityListItemDto): Observable<SpecialityListItemDto> {
    return this.http.post<SpecialityListItemDto>(`${this.baseUrl}/AddItem`, speciality);
  }

  removeSpeciality(specialityDataId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/removeItem/${specialityDataId}`);
  }

  changeIsPublishedStatus(listId: number, isPublished: boolean): Observable<boolean> {
    return this.http.put<boolean>(`${this.baseUrl}/publish/${listId}`, isPublished);
  }
}
