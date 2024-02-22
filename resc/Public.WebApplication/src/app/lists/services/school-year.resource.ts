import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { SchoolYearDto } from '../models/school-year.dto';

@Injectable()
export class SchoolYearResource extends BaseResource {
  constructor(
    protected override http: HttpClient,
    protected override configuration: Configuration
  ) {
    super(http, configuration, 'SchoolYearPublic');
  }

  getCurrentYear(): Observable<SchoolYearDto> {
    return this.http.get<SchoolYearDto>(`${this.baseUrl}/current`);
  }
}
