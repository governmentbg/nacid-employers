import { Directive, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ContentTypes } from 'src/infrastructure/constants/constants';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';

@Directive({})
export abstract class BaseSearchComponent<TDto> implements OnInit {

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  @Output() searchEvent = new EventEmitter<TDto[]>();

  model: TDto[] = [];
  canLoadMore: boolean;
  contentTypes = ContentTypes;
  modelCounts = 0;
  noResult: boolean = false;

  constructor(
    public filter: BaseSearchFilter,
    protected resource: IFilterable<BaseSearchFilter, TDto>,
    protected loadingIndicator: LoadingIndicatorService
  ) { }

  ngOnInit(): void {
    this.search();
  }

  search(loadMore?: boolean): Subscription {
    this.noResult = false;

    if (!loadMore) {
      this.filter.offset = 0;
    }

    this.loadingIndicator.show();
    return this.resource.getFiltered(this.filter)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((model: TDto[]) => {
        if (model.length < 1) {
          this.noResult = true;
        }

        if (!this.filter.offset) {
          this.modelCounts = model.length
          this.model = model;
        } else {
          this.modelCounts = this.modelCounts + model.length
          this.model = [...this.model, ...model];
        }

        this.canLoadMore = model.length === this.filter.limit;
        this.filter.offset = this.model.length;

        this.searchEvent.emit(this.model);
      });
  }

  loadMore(): void {
    this.filter.offset = this.model.length;
    this.search(true);
  }

  clearFilter(): void {
    this.filter.clear();
    this.modelCounts = 0;
    this.search();
  }
}
