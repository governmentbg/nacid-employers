import { Component, Input } from '@angular/core';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { UniversityDto } from 'src/modules/application/models/university.dto';
import { SpecialityListItemDto } from 'src/modules/lists/models/speciality-list/speciality-list-item.dto';

@Component({
  selector: 'app-university-form',
  templateUrl: './university-form.component.html'
})

export class UniversityFormComponent extends CommonFormComponent<UniversityDto> {
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;
  @Input() isApplicationEntered = false;

  constructor(public sharedService: SharedService) {
    super();
  }

  setSpecialityItem(item: SpecialityListItemDto): void {
    this.model.specialityListItem = item;
  }
}