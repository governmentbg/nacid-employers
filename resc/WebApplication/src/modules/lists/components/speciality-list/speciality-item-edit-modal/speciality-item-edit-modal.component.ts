import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { SpecialityListItemDto } from 'src/modules/lists/models/speciality-list/speciality-list-item.dto';
import { SpecialityMinisterDto } from 'src/modules/lists/models/speciality-list/speciality-minister.dto';
import { SpecialityListResource } from 'src/modules/lists/services/speciality-list.resource';

@Component({
  selector: 'app-speciality-item-edit-modal',
  templateUrl: './speciality-item-edit-modal.component.html'
})

export class SpecialityItemEditModal {
  model: SpecialityListItemDto;
  schoolYearId: number;

  constructor(
    public modal: NgbActiveModal,
    private resource: SpecialityListResource,
    public sharedService: SharedService) {
  }

  editItem(): void {
    this.resource.editSpeciality(this.model)
      .subscribe(() => this.modal.close(true));
  }

  onSpecialityChange(): void {
    this.model.studentsCount = null;
    this.model.specialityMinisters = [];
    const minister = new SpecialityMinisterDto();
    this.model.specialityMinisters.push(minister);
  }

  onFormChange(): void {
    this.onSpecialityChange();
    this.model.speciality = null;
  }

  onQualificationChange(): void {
    this.onFormChange();
    this.model.educationFormType = null;
  }

  onInstitutionChange(): void {
    this.onQualificationChange();
    this.model.educationalQualification = null;
  }

  addMinister(): void {
    if (!this.model.specialityMinisters) {
      this.model.specialityMinisters = [];
    }

    const minister = new SpecialityMinisterDto();
    this.model.specialityMinisters.push(minister);
  }

  removeMinister(index: number): void {
    if (this.model.specialityMinisters.length <= 1) {
      return;
    }

    this.model.specialityMinisters.splice(index, 1);
  }
}