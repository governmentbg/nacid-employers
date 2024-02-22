import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { PartState } from 'src/infrastructure/enums/part-state.enum';
import { PartDto } from 'src/infrastructure/models/part.dto';
import { PartResource } from 'src/infrastructure/services/part.resource';
import { RoleService } from 'src/infrastructure/services/role.service';
import { PartStateEnumLocalization } from 'src/modules/enum-localization.const';
import { ActionConfirmationModalComponent } from '../action-confirmation-modal/action-confirmation-modal.component';

@Component({
  selector: 'app-part-panel',
  templateUrl: './part-panel.component.html',
  styleUrls: ['./part-panel.component.css']
})
export class PartPanelComponent {
  private originalEntity: any;
  private isEditModeFromStartedModification = false;

  public isEditMode = false;
  public hasValidData: boolean;

  partStates = PartState;
  commitStates = CommitState;
  enumLocalization = PartStateEnumLocalization;

  canEdit: boolean = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.UNIVERSITY_USER);
  isUniversityUser: boolean = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

  @Input() header: string;

  @Input() partName: string;

  @Input() commitState: CommitState;

  @Input() model: PartDto<any>;
  @Output() modelChange: EventEmitter<PartDto<any>> = new EventEmitter();

  @Output() modelStateChange: EventEmitter<PartState> = new EventEmitter();

  constructor(
    private resource: PartResource,
    private loadingIndicator: LoadingIndicatorService,
    private modal: NgbModal,
    private roleService: RoleService
  ) { }

  edit(): void {
    if (this.isEditMode) {
      return;
    }

    this.originalEntity = JSON.parse(JSON.stringify(this.model.entity));
    this.isEditMode = true;
  }

  update(): void {
    this.loadingIndicator.show();
    this.resource.updatePartEntity(this.partName, this.model.id, this.model.entity)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe(() => {
        this.originalEntity = null;
        this.isEditMode = false;
        this.isEditModeFromStartedModification = false;

        this.modelChange.emit(this.model);
        this.modelStateChange.emit(this.model.state);
      });
  }

  cancelEdit(): void {
    if (!this.isEditMode) {
      return;
    }
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    const confirmationMessage = "Сигурни ли сте, че искате да откажете промените?";
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;

    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.model.entity = JSON.parse(JSON.stringify(this.originalEntity));
          this.originalEntity = null;

          if (this.isEditModeFromStartedModification) {
            this.cancelModification();
          } else {
            this.isEditMode = false;
            this.modelChange.emit(this.model);
          }
        }
      });
  }

  startModification(): void {
    this.loadingIndicator.show();
    this.resource.startPartModification(this.partName, this.model.id)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((result: PartDto<any>) => {
        this.model = result;

        this.edit();
        this.isEditModeFromStartedModification = true;

        this.modelChange.emit(this.model);
      });
  }

  cancelModification(): void {
    if (!this.isEditModeFromStartedModification) {
      const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
      const confirmationMessage = "Сигурни ли сте, че искате да откажете промените?";
      confirmationModal.componentInstance.confirmationMessage = confirmationMessage;

      confirmationModal.result
        .then((result: boolean) => {
          if (result) {
            this.loadingIndicator.show();
            this.resource.cancelPartModification(this.partName, this.model.id)
              .pipe(
                finalize(() => this.loadingIndicator.hide())
              )
              .subscribe((result: PartDto<any>) => {
                this.model = result;
                this.isEditMode = false;
                this.modelChange.emit(this.model);
                this.modelStateChange.emit(this.model.state);
              });
          }
        });
    }
    else {
      this.loadingIndicator.show();
      this.resource.cancelPartModification(this.partName, this.model.id)
        .pipe(
          finalize(() => this.loadingIndicator.hide())
        )
        .subscribe((result: PartDto<any>) => {
          this.model = result;
          this.isEditMode = false;
          this.modelChange.emit(this.model);
          this.modelStateChange.emit(this.model.state);
        });
    }
  }
}
