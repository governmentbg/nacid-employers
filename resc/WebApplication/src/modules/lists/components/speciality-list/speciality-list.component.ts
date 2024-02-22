import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { SharedService } from 'src/infrastructure/services/shared.service';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { SpecialityListDto } from 'src/modules/lists/models/speciality-list/speciality-list.dto';
import { SpecialityListItemDto } from '../../models/speciality-list/speciality-list-item.dto';
import { SpecialityMinisterDto } from '../../models/speciality-list/speciality-minister.dto';
import { BaseListFilter } from '../../services/base-list.filter';
import { SchoolYearResource } from '../../services/school-year.resource';
import { SpecialityListResource } from '../../services/speciality-list.resource';
import { SpecialityItemEditModal } from './speciality-item-edit-modal/speciality-item-edit-modal.component';

@Component({
  selector: 'app-speciality-list',
  templateUrl: './speciality-list.component.html'
})
export class SpecialityListComponent extends CommonFormComponent<SpecialityListItemDto> implements OnInit {
  model: SpecialityListItemDto = new SpecialityListItemDto();
  specialityList: SpecialityListDto = new SpecialityListDto();

  isPublished = false;

  constructor(
    private resource: SpecialityListResource,
    private schoolYearResource: SchoolYearResource,
    private loadingIndicator: LoadingIndicatorService,
    private toastrService: ToastrService,
    private modal: NgbModal,
    private activatedRoute: ActivatedRoute,
    public sharedService: SharedService) {
    super();
  }

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { specialityList: SpecialityListDto }) => this.specialityList = data.specialityList);
  }

  addSpeciality(): void {
    this.model.specialityListId = this.specialityList.id;

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да добавите нов запис в списъка?';

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.addSpeciality(this.model)
            .pipe(finalize(() => this.loadingIndicator.hide()))
            .subscribe((item: SpecialityListItemDto) => {
              this.specialityList.items.unshift(item);
              this.model = new SpecialityListItemDto();
            },
              (err) => handleDomainError(
                err,
                [{ code: 'Item_Already_Exists', text: 'Вече има създаден запис за тази специалност' }],
                this.toastrService
              ));
        }
      });
  }

  removeSpeciality(speciality: SpecialityListItemDto): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изтрите записа?'

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show()
          this.resource.removeSpeciality(speciality.id)
            .pipe(finalize(() => this.loadingIndicator.hide()))
            .subscribe(() => {
              const index = this.specialityList.items.indexOf(speciality);
              this.specialityList.items.splice(index, 1);
            },
              (err) => handleDomainError(
                err,
                [
                  { code: 'Item_Not_Found', text: 'Тази специалност не може да бъде намерена, моля свържете се с администратор' },
                  { code: 'Item_In_Use', text: 'Тази специалност не може да бъде изтрита, защото е записана в договор' }
                ],
                this.toastrService
              ));
        }
      });
  }

  changeIsPublishedStatus(isPublished: boolean): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = isPublished ?
      "Сигурни ли сте, че искате да публикувате списъка?" : `Сигурни ли сте, че искате да свалите от публикация списъка?`;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.resource.changeIsPublishedStatus(this.specialityList.id, isPublished).subscribe((isPublished: boolean) => {
            this.specialityList.isPublished = isPublished
          },
            (err) => handleDomainError(
              err,
              [{ code: 'List_Not_Found', text: 'Листът със специалности не съществува, моля свържете се с администратор' }],
              this.toastrService
            ));
        }
      });
  }

  editSpeciality(speciality: SpecialityListItemDto, index: number): void {
    this.resource.getSpeciality(speciality.id)
      .subscribe((item: SpecialityListItemDto) => {
        const editModal = this.modal.open(SpecialityItemEditModal, { backdrop: 'static', size: 'lg' });
        editModal.componentInstance.schoolYearId = this.specialityList.schoolYear.id;
        editModal.componentInstance.model = item;

        editModal.result.then((result: boolean) => {
          if (result) {
            this.specialityList.items[index] = editModal.componentInstance.model;
          }
        });
      });
  }

  loadList(schoolYearId: number): void {
    const filter = new BaseListFilter();
    filter.schoolYearId = schoolYearId;

    this.loadingIndicator.show();
    this.resource.getSpecialityList(filter)
      .pipe(finalize(() => this.loadingIndicator.hide()))
      .subscribe((specialityList: SpecialityListDto) => {
        if (specialityList) {
          this.specialityList = specialityList;
        }
      })
  }

  createYear(schoolYearId: number): void {
    const yearString = `(${this.specialityList.schoolYear.primaryYear + 1}/${this.specialityList.schoolYear.primaryYear + 2})`

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = `Сигурни ли сте, че искате да създадете нова ${yearString} учебна година?`;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.schoolYearResource.createYear(schoolYearId)
            .pipe(finalize(() => this.loadingIndicator.hide()))
            .subscribe((specialityList: SpecialityListDto) => {
              if (specialityList) {
                this.specialityList = specialityList;
              }
            });
        }
      });
  }

  searchEventHandler(items: SpecialityListItemDto[]): void {
    this.specialityList.items = items;
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

  onResearchAreaChange(): void {
    this.onInstitutionChange();
    this.model.institution = null;
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
