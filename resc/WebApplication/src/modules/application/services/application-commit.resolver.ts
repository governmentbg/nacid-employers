import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ApplicationDto } from '../models/application.dto';
import { ApplicationResource } from './application.resource';

@Injectable()
export class ApplicationCommitResolver implements Resolve<ApplicationDto> {

  constructor(
    private resource: ApplicationResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  resolve(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): ApplicationDto | Observable<ApplicationDto> | Promise<ApplicationDto> {
    const lotId = +route.params.lotId;
    const commitId = +route.params.commitId;
    if (isNaN(lotId) || isNaN(commitId)) {
      return EMPTY;
    }

    this.loadingIndicator.show();
    return this.resource.getCommit(lotId, commitId)
      .pipe(
        catchError(() => {
          return EMPTY;
        }),
        finalize(() => this.loadingIndicator.hide())
      );
  }

}
