import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { ApplicationDto } from '../../models/application.dto';
import { CommitInfoDto } from '../../models/commit-info.dto';
import { ApplicationResource } from '../../services/application.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-application-new',
  templateUrl: './application-new.component.html'
})
export class ApplicationNewComponent implements OnInit {
  model = new ApplicationDto();
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
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.institution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));
    this.model.university.institution = this.institution;
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }

  save(): void {
    if (!this.canSubmit) {
      return;
    }

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да изпратите данните за вписване?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.model.isDraft = false;
          this.loadingIndicator.show()
          this.resource.createApplication(this.model)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((model: CommitInfoDto) => {
              this.toastrService.success('Записа е изпратен за вписване');
              this.router.navigate(['/application/search']);
            });
        }
      });
  }

  saveDraft() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = 'Сигурни ли сте, че искате да запазите записа в чернова?';
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.model.isDraft = true;
          this.loadingIndicator.show()
          this.resource.createApplication(this.model)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((model: CommitInfoDto) => {
              this.toastrService.success('Данните са записани успешно');
              this.router.navigate(['/application/draft', 'lot', model.lotId, 'commit', model.commitId]);
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
}