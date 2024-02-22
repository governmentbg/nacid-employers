import { Component, Input } from '@angular/core';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { EducationType } from 'src/modules/application/enums/education-type.enum';
import { StudentStatusType } from 'src/modules/application/enums/student-status-type.enum';
import { ActualEducationDto } from 'src/modules/application/models/actual-education.dto';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';

@Component({
  selector: 'app-actual-education-form',
  templateUrl: './actual-education-form.component.html'
})

export class ActualEducationFormComponent extends CommonFormComponent<ActualEducationDto> {
  studentStatus = StudentStatusType;
  studentEducationType = EducationType;

  @Input() isApplicationEntered = false;
  @Input('educationalQualification') set educationalQualification(educationalQualification: NomenclatureDto) {
    if (educationalQualification == undefined) {
      return;
    }

    this.model.educationalQualification = educationalQualification;
  }

  constructor() {
    super();
  }
}