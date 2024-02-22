import { Component, Input } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { SpecialityListItemDto } from 'src/modules/lists/models/speciality-list/speciality-list-item.dto';
import { SpecialityListSearchFilter } from 'src/modules/lists/services/speciality-list-search.filter';
import { SpecialityListResource } from 'src/modules/lists/services/speciality-list.resource';

@Component({
  selector: 'app-speciality-list-search',
  templateUrl: 'speciality-list-search.component.html'
})

export class SpecialityListSearchComponent extends BaseSearchComponent<SpecialityListItemDto> {
  @Input() schoolYearId: number;

  constructor(
    public filter: SpecialityListSearchFilter,
    protected resource: SpecialityListResource,
    protected loadingIndicator: LoadingIndicatorService,
  ) {
    super(filter, resource, loadingIndicator);
    this.filter = new SpecialityListSearchFilter();
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