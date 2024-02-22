import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { EmployerListItemDto } from '../../models/employer-list/employer-list-item.dto';
import { SchoolYearDto } from '../../models/school-year.dto';
import { EmployerListSearchFilter } from '../../services/employer-list-search.filter';
import { EmployerListResource } from '../../services/employer-list.resource';

@Component({
  selector: 'app-employer-list',
  templateUrl: './employer-list.component.html',
  styleUrls: ['../list.component.css']
})
export class EmployerListComponent extends BaseSearchComponent<EmployerListItemDto> {
  schoolYear: SchoolYearDto;
  afterFirstSearch: boolean = false;

  constructor(
    protected override resource: EmployerListResource,
    public override filter: EmployerListSearchFilter,
    protected override loadingIndicator: LoadingIndicatorService,
    private activatedRoute: ActivatedRoute
  ) {
    super(filter, resource, loadingIndicator);
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
