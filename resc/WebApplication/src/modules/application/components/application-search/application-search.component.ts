import { Component } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { RoleService } from 'src/infrastructure/services/role.service';
import { CommitStateEnumLocalization } from 'src/modules/enum-localization.const';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ApplicationSearchResultDto } from '../../models/application-search-result.dto';
import { ApplicationSearchFilter } from '../../services/application-search.filter';
import { ApplicationResource } from '../../services/application.resource';

@Component({
  selector: 'app-application-search',
  templateUrl: './application-search.component.html',
  styleUrls: ['./application-search.component.css']
})
export class ApplicationSearchComponent extends BaseSearchComponent<ApplicationSearchResultDto> {
  isUniversityUser: boolean;
  universityUserInstitution: NomenclatureDto;
  canAddNewApplication: boolean = false;
  commitStateEnumLocalization = CommitStateEnumLocalization;
  commitStates = CommitState;

  constructor(
    public filter: ApplicationSearchFilter,
    protected resource: ApplicationResource,
    protected loadingIndicator: LoadingIndicatorService,
    private roleService: RoleService,
    private configuration: Configuration
  ) {
    super(filter, resource, loadingIndicator);
    this.filter = new ApplicationSearchFilter();
  }

  ngOnInit(): void {
    this.canAddNewApplication = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

    this.universityUserInstitution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));

    this.isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
    if (this.isUniversityUser) {
      this.filter.institution = this.universityUserInstitution.name;
      this.filter.institutionId = this.universityUserInstitution.id;
    } else {
      this.filter.institution = null;
      this.filter.institutionId = undefined;
    }

    this.search();
  }

  clearFilter(): void {
    if (this.isUniversityUser) {
      this.filter.studentName = null;
      this.filter.contractNumber = null;

      super.search();
    } else {
      super.clearFilter();
    }
  }
}
