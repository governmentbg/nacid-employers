import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { SchoolYearDto } from '../models/school-year.dto';
import { SchoolYearResource } from '../services/school-year.resource';

@Injectable()
export class SchoolYearResolver implements Resolve<SchoolYearDto> {

  constructor(
    private resource: SchoolYearResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(): SchoolYearDto | Observable<SchoolYearDto> | Promise<SchoolYearDto> {
    this.loadingIndicator.show();
    return this.resource.getCurrentYear()
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }
}