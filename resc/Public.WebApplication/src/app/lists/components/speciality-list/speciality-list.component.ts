import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { SchoolYearDto } from '../../models/school-year.dto';
import { SpecialityListItemDto } from '../../models/speciality-list/speciality-list-item.dto';
import { SpecialityListSearchFilter } from '../../services/speciality-list-search.filter';
import { SpecialityListResource } from '../../services/speciality-list.resource';

@Component({
  selector: 'app-speciality-list',
  templateUrl: './speciality-list.component.html',
  styleUrls: ['../list.component.css']
})
export class SpecialityListComponent extends BaseSearchComponent<SpecialityListItemDto> {
  schoolYear: SchoolYearDto;
  afterFirstSearch: boolean = false;

  constructor(
    protected override resource: SpecialityListResource,
    protected override loadingIndicator: LoadingIndicatorService,
    public override filter: SpecialityListSearchFilter,
    private activatedRoute: ActivatedRoute
  ) {
    super(filter, resource, loadingIndicator)
  }

  override ngOnInit() {
    this.activatedRoute.data
      .subscribe((data: { schoolYear: SchoolYearDto }) => this.schoolYear = data.schoolYear);
    this.filter.clear();
    this.filter.schoolYearId = this.schoolYear.id;
    super.search();
  }

  searchOverride() {
    super.search();
    // this.filter.clear();
    // this.filter.schoolYearId = this.schoolYear.id;
    this.afterFirstSearch = true;
  }

  onYearChange(event: any): void {
    this.filter.schoolYearId = event.id;
    super.search();
    this.afterFirstSearch = true;
  }
}
