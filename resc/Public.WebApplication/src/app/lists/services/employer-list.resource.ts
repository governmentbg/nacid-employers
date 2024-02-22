import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { EmployerListItemDto } from '../models/employer-list/employer-list-item.dto';
import { EmployerListDto } from '../models/employer-list/employer-list.dto';
import { EmployerListSearchFilter } from './employer-list-search.filter';

@Injectable()
export class EmployerListResource extends BaseResource
  implements IFilterable<EmployerListSearchFilter, EmployerListItemDto> {
  constructor(
    protected override http: HttpClient,
    protected override configuration: Configuration
  ) {
    super(http, configuration, 'EmployerPublicList');
  }
  getFiltered(filter?: any): Observable<EmployerListItemDto[]> {
    return this.http.get<EmployerListItemDto[]>(`${this.baseUrl}/search${this.composeQueryString(filter)}`);
  }

  getEmployerList(filter?: any): Observable<EmployerListDto> {
    return this.http.get<EmployerListDto>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }
}
