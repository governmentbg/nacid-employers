import { Location } from '@angular/common';
import { Component, OnInit, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { PartPanelComponent } from '../../../../infrastructure/components/part-panel/part-panel.component';
import { CommitState } from '../../../../infrastructure/enums/commit-state.enum';
import { ApplicationCommitDto } from '../../models/application-commit.dto';
import { ChangeStateDescription } from '../../models/change-state.dto';
import { ApplicationModificationResource } from '../../services/application-modification.resource';
import { ChangeContractModalComponent } from '../application-modal/change-contract-modal.component';
import { TerminateContractModalComponent } from '../application-modal/terminate-contract-modal.component';
import { PartsEditWarningModalComponent } from '../parts-edit-warning-modal/parts-edit-warning-modal.component';

@Component({
  selector: 'app-application-commit',
  templateUrl: './application-commit.component.html'
})
export class ApplicationCommitComponent implements OnInit {
  @ViewChildren(PartPanelComponent) parts: PartPanelComponent[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private resource: ApplicationModificationResource,
    private router: Router,
    private location: Location,
    private loadingIndicator: LoadingIndicatorService,
    private modal: NgbModal,
    private roleService: RoleService,
    private toastrService: ToastrService
  ) { }

  model: ApplicationCommitDto;
  header: string;
  actions: { action: () => void, name: string, btnClass: string, isActive: boolean }[] = [];
  commitStates = CommitState;
  changeStateDescription: ChangeStateDescription = new ChangeStateDescription();
  canEnterApplication: boolean = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.CONTROL_USER);
  isUniversityUser: boolean = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);
  canSubmit: boolean = true;
  private forms: { [key: string]: boolean } = {};

  private commitStateConfig = {
    // initialDraft: {
    //   header: 'Чернова',
    //   actions: [
    //     {
    //       action: this.executeActionOnConfirmation.bind(
    //         this,
    //         this.finishModification.bind(this),
    //         'Сигурни ли сте, че искате да впишете първоначалната чернова в регистъра?',
    //         false
    //       ),
    //       name: 'Изпрати за вписване',
    //       isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER),
    //       style: 'btn-outline-success'
    //     },
    //     {
    //       action: this.executeActionOnConfirmation.bind(
    //         this,
    //         this.deleteDraft.bind(this),
    //         'Сигурни ли сте, че искате да изтриете записа?',
    //         false
    //       ),
    //       name: 'Изтрий',
    //       btnClass: 'btn-danger',
    //       isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER),
    //       style: 'btn-outline-danger'
    //     }
    //   ]
    // },
    actual: {
      header: 'Изпратен за вписване',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModification.bind(this),
            'Сигурни ли сте, че искате да върнете записа за редакция?',
            true
          ),
          name: 'Върни за редакция',
          isActive: this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.CONTROL_USER),
          style: 'btn-primary'
        },
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.erase.bind(this),
            'Сигурни ли сте, че искате да изтриете записа?',
            true
          ),
          name: 'Изтрий',
          btnClass: 'btn-danger',
          isActive: this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR),
          style: 'btn-danger'
        }
      ]
    },
    modification: {
      header: 'Върнат',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.erase.bind(this),
            'Сигурни ли сте, че искате да изтриете записа?',
            false
          ),
          name: 'Изтрий',
          btnClass: 'btn-danger',
          isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER),
          style: 'btn-danger'
        }
      ]
    },
    actualWithModification: {
      header: 'Актуално състояние в процес на промяна',
      actions: []
    },
    history: {
      header: 'Предишен запис',
      actions: []
    },
    deleted: {
      header: 'Изтрит',
      actions: [
        // {
        //   action: this.executeActionOnConfirmation.bind(
        //     this,
        //     this.revertErased.bind(this),
        //     'Сигурни ли сте, че искате да възстановите изтрития запис?',
        //     false
        //   ),
        //   name: 'Възстановяване на запис',
        //   isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER),
        //   style: 'btn-primary'
        // }
      ]
    },
    entered: {
      header: 'Вписан',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModificationEntered.bind(this),
            'Сигурни ли сте, че искате да редактирате записа?',
            true
          ),
          name: 'Редактирай',
          isActive: this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR),
          style: 'btn-primary'
        }
      ]
    },
    enteredModification: {
      header: 'В процес на редакция',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.finishEnteredModification.bind(this),
            'Сигурни ли сте, че искате да запишете промените?',
            false
          ),
          name: 'Запиши промените',
          isActive: this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER, UserRoleAliases.ADMINISTRATOR),
          style: 'btn-success'
        }
      ]
    },
    enteredWithModification: {
      header: 'Актуално състояние в процес на промяна',
      actions: []
    },
    enteredWithChange: {
      header: 'Вписан с изменение',
      actions: [
        {
          action: this.executeActionOnConfirmation.bind(
            this,
            this.startModificationEntered.bind(this),
            'Сигурни ли сте, че искате да редактирате записа?',
            true
          ),
          name: 'Редактирай',
          isActive: this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR),
          style: 'btn-primary'
        }
      ]
    },
    terminated: {
      header: 'Прекратен',
      actions: []
    },
    expired: {
      header: 'Изтекъл',
      actions: []
    }
  };

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { commit: ApplicationCommitDto }) => this.setData(data.commit));
  }

  startModification(): void {
    this.loadingIndicator.show();
    this.resource.startModification(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => {
        this.setData(data);
        this.router.navigate(['/application/search'])
      });
  }

  finishModification(): void {
    const partsInEditMode = this.getPartsInEditMode();
    if (partsInEditMode.length > 0) {
      const modalRef = this.modal.open(PartsEditWarningModalComponent, { backdrop: 'static' });
      modalRef.componentInstance.partsInEditMode = partsInEditMode;
      modalRef.result.then();
      return;
    }

    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да изпратите записа за вписване?";

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.loadingIndicator.show();
          this.resource.finishModification(this.model.lotId)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((data: ApplicationCommitDto) => {
              this.toastrService.success('Успешно изпратен запис');
              this.router.navigate(['/application', 'search']);
            });
        }
      })
  }

  erase(): void {
    this.loadingIndicator.show();
    this.resource.eraseApplication(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => {
        this.toastrService.success('Успешно изтрит запис');
        this.router.navigate(['/application', 'search']);
      });
  }

  cancelModification(): void {
    const partsInEditMode = this.getPartsInEditMode();
    if (partsInEditMode.length > 0) {
      const modalRef = this.modal.open(PartsEditWarningModalComponent, { backdrop: 'static' });
      modalRef.componentInstance.partsInEditMode = partsInEditMode;
      modalRef.result.then();
      return;
    }

    this.loadingIndicator.show();
    this.resource.cancelModification(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

  revertErased(): void {
    this.loadingIndicator.show();
    this.resource.revertErasedApplication(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => {
        this.router.navigate(['/application', 'search']);
      });
  }

  delete(): void {
    this.loadingIndicator.show();
    this.resource.deleteLot(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => this.router.navigate(['/application', 'search']));
  }

  enterApplication() {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.confirmationMessage = "Сигурни ли сте, че искате да впишете договора?";

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.resource.enterApplication(this.model.lotId)
            .pipe(
              finalize(() => this.loadingIndicator.hide())
            )
            .subscribe((data: ApplicationCommitDto) => {
              this.toastrService.success('Успешно вписан договор');
              this.router.navigate(['/application', 'search']);
            });
        }
      })
  }

  startModificationEntered(): void {
    this.loadingIndicator.show();
    this.resource.startModificationEntered(this.model.lotId, this.changeStateDescription)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

  finishEnteredModification(): void {
    const partsInEditMode = this.getPartsInEditMode();
    if (partsInEditMode.length > 0) {
      const modalRef = this.modal.open(PartsEditWarningModalComponent, { backdrop: 'static' });
      modalRef.componentInstance.partsInEditMode = partsInEditMode;
      modalRef.result.then();
      return;
    }

    this.loadingIndicator.show();
    this.resource.finishEnteredModification(this.model.lotId)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((data: ApplicationCommitDto) => this.setData(data));
  }

  changeContract() {
    const confirmationModal = this.modal.open(ChangeContractModalComponent, { size: 'lg' });
    confirmationModal.componentInstance.lotId = this.model.lotId;

    confirmationModal.result
      .then((result: ApplicationCommitDto) => {
        if (result) {
          this.setData(result);
        }
      })
  }

  terminateContract() {
    const confirmationModal = this.modal.open(TerminateContractModalComponent, { size: 'lg' });
    confirmationModal.componentInstance.lotId = this.model.lotId;

    confirmationModal.result
      .then((result: ApplicationCommitDto) => {
        if (result) {
          this.toastrService.success('Успешно прекратен договор');
          this.setData(result);
        }
      })
  }

  private executeActionOnConfirmation(action: () => void, confirmationMessage: string, showTextArea: boolean): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    confirmationModal.componentInstance.showTextArea = showTextArea;
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;
    confirmationModal.componentInstance.passDescription.subscribe((description: string) => {
      this.changeStateDescription.changeStateDescription = description;
    })
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          action();
        }
      });
  }

  private setData(data: ApplicationCommitDto): void {
    this.model = data;

    const commitConfig = this.commitStateConfig[CommitState[data.state]];
    this.header = commitConfig.header;
    this.actions = commitConfig.actions.filter(e => e.isActive);

    this.changeLocation(data.lotId, data.id);
  }

  private changeLocation(lotId: number, commitId: number): void {
    const url = this.router.createUrlTree(['/application', 'lot', lotId, 'commit', commitId]).toString();
    this.location.replaceState(url);
  }

  private getPartsInEditMode(): string[] {
    return this.parts
      .filter(e => e.isEditMode)
      .map(e => e.header);
  }

  changeFormValidStatus(form: string, isValid: boolean): void {
    this.forms[form] = isValid;
    this.canSubmit = Object.keys(this.forms).findIndex(e => !this.forms[e]) < 0;
  }

  backClicked() {
    this.router.navigate(['/application/', 'lot', this.model.lotId, 'history']);
  }

  goToSearch() {
    this.router.navigate(['/application', 'search']);
  }
}
