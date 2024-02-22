import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicationCommitHistoryItemDto } from '../models/application-commit-history-item.dto';
import { ApplicationDraftDto } from '../models/application-draft.dto';
import { ApplicationLotHistoryDto } from '../models/application-lot-history.dto';
import { ApplicationSearchResultDto } from '../models/application-search-result.dto';
import { ApplicationDto } from '../models/application.dto';
import { CommitInfoDto } from '../models/commit-info.dto';
import { ApplicationSearchFilter } from './application-search.filter';

@Injectable()
export class ApplicationResource extends BaseResource implements IFilterable<ApplicationSearchFilter, SearchResultItemDto<ApplicationSearchResultDto>> {

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'Application');
  }

  getFiltered(filter?: ApplicationSearchFilter): Observable<SearchResultItemDto<ApplicationSearchResultDto>> {
    return this.http.get<SearchResultItemDto<ApplicationSearchResultDto>>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  createApplication(model: ApplicationDto): Observable<CommitInfoDto> {
    return this.http.post<CommitInfoDto>(this.baseUrl, model);
  }

  getCommit(lotId: number, commitId: number): Observable<ApplicationDto> {
    return this.http.get<ApplicationDto>(`${this.baseUrl}/lot/${lotId}/commit/${commitId}`);
  }

  getHistory(lotId: number): Observable<ApplicationLotHistoryDto> {
    return this.http.get<ApplicationLotHistoryDto>(`${this.baseUrl}/lot/${lotId}/history`);
  }

  updateApplicationCommit(commitId: number, model: ApplicationDraftDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}?commitId=${commitId}`, model)
  }
}