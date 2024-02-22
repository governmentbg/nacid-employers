import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { EmployerListDto } from '../models/employers-list/employer-list.dto';
import { BaseListFilter } from '../services/base-list.filter';
import { EmployerListResource } from '../services/employer-list-resource';

@Injectable()
export class EmployerListResolver implements Resolve<EmployerListDto> {

  constructor(
    private resource: EmployerListResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(): EmployerListDto | Observable<EmployerListDto> | Promise<EmployerListDto> {
    const filter = new BaseListFilter();
    filter.isCurrent = true;

    this.loadingIndicator.show();
    return this.resource.getEmployerList(filter)
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }

}
