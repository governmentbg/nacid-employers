import { Component, Input } from '@angular/core';
import { toJSDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-calendar';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { ContactType } from 'src/modules/application/enums/contact-type.enum';
import { EducationType } from 'src/modules/application/enums/education-type.enum';
import { TaxType } from 'src/modules/application/enums/tax-type.enum';
import { ContactPerson } from 'src/modules/application/models/contact-person.dto';
import { ContractDto } from 'src/modules/application/models/contract.dto';
import { TaxTypeEnumLocalization } from 'src/modules/enum-localization.const';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html'
})

export class ContractFormComponent extends CommonFormComponent<ContractDto> {
  taxTypes = TaxType;
  taxLabel: string;
  type = ContactType;

  @Input() isApplicationEntered = false;
  @Input('educationType') set educationType(educationType: EducationType) {
    if (educationType == undefined) {
      return;
    } else if (educationType === EducationType.payment) {
      this.model.taxType = TaxType.partially;
    } else if (educationType === EducationType.standard) {
      this.model.taxType = TaxType.full;
    }
  }

  cyrillicRegExp = RegExps.CYRILLIC_NAMES_REGEX;
  mailRegex = RegExps.EMAIL_REGEX;
  phoneRegExp = RegExps.PHONE_NUMBER_REGEX;

  ngOnInit() {
    if (!this.model.contacts.length) {
      const institutionContact = new ContactPerson(ContactType.institution);
      this.model.contacts.push(institutionContact);

      const employerContact = new ContactPerson(ContactType.employer);
      this.model.contacts.push(employerContact);
    }
  }

  constructor(public sharedService: SharedService) {
    super();
  }

  endContractDate() {
    if (!this.model.signingDate || !this.model.term) {
      return;
    }
    else {
      let temp = new Date(this.model.signingDate);
      temp.setMonth(temp.getMonth() + +this.model.term);
      this.model.endDate = temp;
    }
  }
}