import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { EmployerListItemDto } from '../models/employers-list/employer-list-item.dto';
import { EmployerListDto } from '../models/employers-list/employer-list.dto';
import { EmployerListSearchFilter } from './employer-list-search.filter';

@Injectable()
export class EmployerListResource extends BaseResource
  implements IFilterable<EmployerListSearchFilter, SearchResultItemDto<EmployerListItemDto>> {
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'EmployerList');
  }
  getFiltered(filter?: EmployerListSearchFilter): Observable<SearchResultItemDto<EmployerListItemDto>> {
    return this.http.get<SearchResultItemDto<EmployerListItemDto>>(`${this.baseUrl}/search${this.composeQueryString(filter)}`);
  }

  getEmployerList(filter?: any): Observable<EmployerListDto> {
    return this.http.get<EmployerListDto>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  getEmployer(employerId: number): Observable<EmployerListItemDto> {
    return this.http.get<EmployerListItemDto>(`${this.baseUrl}/${employerId}`);
  }

  editEmployer(employer: EmployerListItemDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}`, employer);
  }

  addEmployer(employer: EmployerListItemDto): Observable<EmployerListItemDto> {
    return this.http.post<EmployerListItemDto>(`${this.baseUrl}/AddItem`, employer);
  }

  removeEmployer(employerDataId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/removeItem/${employerDataId}`);
  }

  changeIsPublishedStatus(employerListId: number, isPublished: boolean): Observable<boolean> {
    return this.http.put<boolean>(`${this.baseUrl}/publish/${employerListId}`, isPublished);
  }
}
