import { Component, Input } from '@angular/core';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { EmployerDto } from 'src/modules/application/models/employer.dto';
import { EmployerListItemDto } from 'src/modules/lists/models/employers-list/employer-list-item.dto';

@Component({
  selector: 'app-employer-form',
  templateUrl: './employer-form.component.html'
})

export class EmployerFormComponent extends CommonFormComponent<EmployerDto> {
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;
  mailRegex = RegExps.EMAIL_REGEX;
  phoneRegExp = RegExps.PHONE_NUMBER_REGEX;

  @Input() speciality: string;

  @Input() isApplicationEntered = false;

  constructor(public sharedService: SharedService) {
    super();
  }

  setEmployerItem(item: EmployerListItemDto): void {
    this.model.employerListItem = item;
  }
}