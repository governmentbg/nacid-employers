import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { ApplicationResource } from '../../services/application.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ToastrService } from 'ngx-toastr';
import { ApplicationCommitDto } from '../../models/application-commit.dto';
import { ApplicationDraftDto } from '../../models/application-draft.dto';
import { ApplicationModificationResource } from '../../services/application-modification.resource';

@Component({
  selector: 'app-application-draft',
  templateUrl: './application-draft.component.html'
})
export class ApplicationDraftComponent implements OnInit {
  model = new ApplicationDraftDto();
  canSubmit = false;
  institution: NomenclatureDto;

  private forms: { [key: string]: boolean } = {};

  commitStates = CommitState;

  constructor(
    private resource: ApplicationResource,
    private router: Router,
    private modal: NgbModal,
    private loadingIndicator: LoadingIndicatorService,
    private configuration: Configuration,
    private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute,
    private modificationResource: ApplicationModificationResource
  ) { }

  ngOnInit() {
    this.institution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));
    this.model.university.institution = this.institution;
    this.activatedRoute.data
      .subscribe((data: { commit: ApplicationCommitDto }) => {
        this.setData(data.commit);
      });
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }

  saveDraft(changeStatus: boolean): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });

    changeStatus == true
      ? confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изпратите данните за вписване?'
      : confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да запишете данните?';

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show()
          this.model.changeStatus = changeStatus;
          this.resource.updateApplicationCommit(this.model.id, this.model)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe(() => {
              if (changeStatus == true) {
                this.toastrService.success('Записа е изпратен за вписване');
              }
              else {
                this.toastrService.success('Данните са записани успешно');
              }
              this.router.navigate(['/application/search']);
            });
        }
      });
  }

  cancel() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да излезете от страницата?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.router.navigate(['/application', 'search']);
        }
      });
  }

  private setData(commit: ApplicationCommitDto): void {
    this.model.id = commit.id;
    this.model.lotId = commit.lotId;
    this.model.state = commit.state;
    this.model.university = commit.universityPart.entity;
    this.model.employer = commit.employerPart.entity;
    this.model.student = commit.studentPart.entity;
    this.model.contract = commit.contractPart.entity;
    this.model.actualEducation = commit.actualEducationPart.entity;
  }

  deleteDraft(): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да изтриете записа?";

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.modificationResource.deleteDraft(this.model.lotId)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe(() => {
              this.toastrService.success('Успешно изтрит запис');
              this.router.navigate(['/application/search']);
            });
        }
      });
  }
}