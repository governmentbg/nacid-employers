import { Component, Input } from '@angular/core';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { EducationType } from 'src/modules/application/enums/education-type.enum';
import { StudentStatusType } from 'src/modules/application/enums/student-status-type.enum';
import { StudentDto } from 'src/modules/application/models/student.dto';

@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html'
})

export class StudentFormComponent extends CommonFormComponent<StudentDto> {
  educationTypes = EducationType;
  studentStatus = StudentStatusType;
  mailRegex = RegExps.EMAIL_REGEX;
  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;
  phoneRegExp = RegExps.PHONE_NUMBER_REGEX;
  uinRegExp = RegExps.UIN_REGEX;

  @Input() isApplicationEntered = false;

  invalidEgn = false;

  constructor(public sharedService: SharedService) {
    super();
  }

  validateEgn(egn: string) {
    if (egn.length == 10) {
      let monthNumbers = +egn.substring(2, 4);
      let dayNumbers = +egn.substring(4, 6);
      if (monthNumbers > 12 && monthNumbers <= 20 && monthNumbers > 32 && monthNumbers <= 40) {
        this.invalidEgn = true;
        return;
      }
      if (dayNumbers > 31) {
        this.invalidEgn = true;
        return;
      }

      const weights: number[] = [2, 4, 8, 5, 10, 9, 7, 3, 6];
      const mod = 11;

      let sum = 0;
      let checkSum = +egn.substring(9, 10);

      for (let i = 0; i < 9; i++) {
        sum += +egn.substring(i, i + 1) * weights[i];
      }

      var validCheckSum = sum % mod;

      if (validCheckSum >= 10) {
        validCheckSum = 0;
      }

      if (validCheckSum == checkSum) {
        this.invalidEgn = false;
      }
      else {
        this.invalidEgn = true;
      }
    }
  }
}