import { Observable } from 'rxjs';

export interface IFilterable<TFilter, TDto> {
  getFiltered(filter?: TFilter): Observable<TDto[]>;
}
