import { Directive, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ContentTypes } from 'src/infrastructure/constants/constants';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
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
  totalCounts = 0;

  constructor(
    public filter: BaseSearchFilter,
    protected resource: IFilterable<BaseSearchFilter, SearchResultItemDto<TDto>>,
    protected loadingIndicator: LoadingIndicatorService
  ) { }

  ngOnInit(): void {
    this.search();
  }

  search(loadMore?: boolean): Subscription {
    if (!loadMore) {
      this.filter.offset = 0;
    }

    this.loadingIndicator.show();
    return this.resource.getFiltered(this.filter)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((model: SearchResultItemDto<TDto>) => {
        this.totalCounts = model.totalCount;

        if (!this.filter.offset) {
          this.modelCounts = model.items.length
          this.model = model.items;
        } else {
          this.modelCounts = this.modelCounts + model.items.length
          this.model = [...this.model, ...model.items];
        }

        this.canLoadMore = model.items.length === this.filter.limit;
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
