import { Component, Input } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { EmployerListItemDto } from 'src/modules/lists/models/employers-list/employer-list-item.dto';
import { EmployerListResource } from 'src/modules/lists/services/employer-list-resource';
import { EmployerListSearchFilter } from 'src/modules/lists/services/employer-list-search.filter';

@Component({
  selector: 'app-employer-list-search',
  templateUrl: 'employer-list-search.component.html'
})

export class EmployerListSearchComponent extends BaseSearchComponent<EmployerListItemDto> {
  @Input() schoolYearId: number;

  constructor(
    public filter: EmployerListSearchFilter,
    protected resource: EmployerListResource,
    protected loadingIndicator: LoadingIndicatorService,
  ) {
    super(filter, resource, loadingIndicator);
    this.filter = new EmployerListSearchFilter();
  }

  ngOnInit() {
    this.filter.schoolYearId = this.schoolYearId;
  }

  clearFilter(): void {
    this.filter.clear();
    this.filter.schoolYearId = this.schoolYearId;
    super.search();
  }
}