import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { SpecialityListDto } from '../models/speciality-list/speciality-list.dto';
import { BaseListFilter } from '../services/base-list.filter';
import { SpecialityListResource } from '../services/speciality-list.resource';

@Injectable()
export class SpecialityListResolver implements Resolve<SpecialityListDto> {

  constructor(
    private resource: SpecialityListResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(): SpecialityListDto | Observable<SpecialityListDto> | Promise<SpecialityListDto> {
    const filter = new BaseListFilter();
    filter.isCurrent = true;

    this.loadingIndicator.show();
    return this.resource.getSpecialityList(filter)
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }

}
