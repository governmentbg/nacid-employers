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
import { EmployerListItemDto } from '../../models/employers-list/employer-list-item.dto';
import { EmployerListDto } from '../../models/employers-list/employer-list.dto';
import { EmployerSpecialityDto } from '../../models/employers-list/employer-speciality-dto';
import { BaseListFilter } from '../../services/base-list.filter';
import { EmployerListResource } from '../../services/employer-list-resource';
import { EmployerItemEditModal } from './employer-item-edit-modal/employer-item-edit-modal.component';

@Component({
  selector: 'app-employer-list',
  templateUrl: './employer-list.component.html'
})
export class EmployerListComponent extends CommonFormComponent<EmployerListItemDto> implements OnInit {
  model: EmployerListItemDto = new EmployerListItemDto();
  employerList: EmployerListDto = new EmployerListDto();

  isPublished = false;

  constructor(
    private resource: EmployerListResource,
    private loadingIndicator: LoadingIndicatorService,
    private toastrService: ToastrService,
    private modal: NgbModal,
    private activatedRoute: ActivatedRoute,
    public sharedService: SharedService) {
    super();
  }

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { employerList: EmployerListDto }) => this.employerList = data.employerList);
  }

  addEmployer(): void {
    this.model.employerListId = this.employerList.id;

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да добавите нов запис в списъка?';

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.addEmployer(this.model)
            .pipe(finalize(() => this.loadingIndicator.hide()))
            .subscribe((item: EmployerListItemDto) => {
              this.employerList.items.unshift(item);
              this.model = new EmployerListItemDto();
            },
              (err) => handleDomainError(
                err,
                [{ code: 'Item_Already_Exists', text: 'Вече има създаден запис за този ранотодател' }],
                this.toastrService
              ));
        }
      });
  }

  editEmployer(employer: EmployerListItemDto, index: number): void {
    this.resource.getEmployer(employer.id)
      .subscribe((item: EmployerListItemDto) => {
        const editModal = this.modal.open(EmployerItemEditModal, { backdrop: 'static', size: 'lg' });
        editModal.componentInstance.schoolYearId = this.employerList.schoolYear.id;
        editModal.componentInstance.model = item;

        editModal.result.then((result: boolean) => {
          if (result) {
            this.employerList.items[index] = editModal.componentInstance.model;
          }
        });
      });
  }

  removeEmployer(employer: EmployerListItemDto): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изтрите записа?'

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show()
          this.resource.removeEmployer(employer.id)
            .pipe(finalize(() => this.loadingIndicator.hide()))
            .subscribe(() => {
              const index = this.employerList.items.indexOf(employer);
              this.employerList.items.splice(index, 1);
            },
              (err) => handleDomainError(
                err,
                [
                  { code: 'Item_Not_Found', text: 'Този работодател не може да бъде намерен, моля свържете се с администратор' },
                  { code: 'Item_In_Use', text: 'Този работодател не може да бъде изтрит, защото е записан в договор' }
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

          this.resource.changeIsPublishedStatus(this.employerList.id, isPublished).subscribe((isPublished: boolean) => {
            this.employerList.isPublished = isPublished
          },
            (err) => handleDomainError(
              err,
              [{ code: 'List_Not_Found', text: 'Листът със специалности не съществува, моля свържете се с администратор' }],
              this.toastrService
            ));
        }
      });
  }

  loadList(schoolYearId: number): void {
    const filter = new BaseListFilter();
    filter.schoolYearId = schoolYearId;

    this.loadingIndicator.show();
    this.resource.getEmployerList(filter)
      .pipe(finalize(() => this.loadingIndicator.hide()))
      .subscribe((employerList: EmployerListDto) => {
        if (employerList) {
          this.employerList = employerList;
        }
      })
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

  searchEventHandler(items: EmployerListItemDto[]): void {
    this.employerList.items = items;
  }
}
