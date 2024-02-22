import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployerListItemDto } from 'src/modules/lists/models/employers-list/employer-list-item.dto';
import { EmployerSpecialityDto } from 'src/modules/lists/models/employers-list/employer-speciality-dto';
import { EmployerListResource } from 'src/modules/lists/services/employer-list-resource';

@Component({
  selector: 'app-employer-item-edit-modal',
  templateUrl: './employer-item-edit-modal.component.html'
})

export class EmployerItemEditModal {
  model: EmployerListItemDto;
  schoolYearId: number;

  constructor(
    public modal: NgbActiveModal,
    private resource: EmployerListResource) {
  }

  editItem(): void {
    this.resource.editEmployer(this.model)
      .subscribe(() => this.modal.close(true));
  }

  addSpeciality(): void {
    if (!this.model.specialities) {
      this.model.specialities = [];
    }

    const newSpeciality = new EmployerSpecialityDto();
    this.model.specialities.push(newSpeciality);
  }

  removeSpeciality(index: number): void {
    if (this.model.specialities.length <= 1) {
      return;
    }

    this.model.specialities.splice(index, 1);
  }

  trackByFn(item: any): void {
    return item.name;
  }
}