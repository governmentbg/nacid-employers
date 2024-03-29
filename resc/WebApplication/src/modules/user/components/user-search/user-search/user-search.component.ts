import { Component } from '@angular/core';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { BaseSearchComponent } from 'src/infrastructure/components/search-component/base-search.component';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { UserStatusEnumLocalization } from 'src/modules/enum-localization.const';
import { UserStatus } from 'src/modules/user/enums/user-status.enum';
import { UserSearchResultDto } from 'src/modules/user/models/user-search-result.dto';
import { UserResource } from 'src/modules/user/resources/user.resource';
import { UserSearchFilter } from 'src/modules/user/services/user-search-filter.service';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css']
})
export class UserSearchComponent extends BaseSearchComponent<UserSearchResultDto> {
  userStatusLocalization = UserStatusEnumLocalization;
  userStatus = UserStatus;

  constructor(
    public filter: UserSearchFilter,
    protected userResource: UserResource,
    protected loadingIndicator: LoadingIndicatorService,
    public sharedService: SharedService
  ) {
    super(filter, userResource, loadingIndicator);
    this.filter = new UserSearchFilter();
  }
}
