import { Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ApplicationCommitDto } from '../../models/application-commit.dto';
import { ApplicationModificationDto } from '../../models/application-modification.dto';
import { ApplicationModificationResource } from '../../services/application-modification.resource';

@Component({
  selector: 'app-change-contract-modal',
  templateUrl: './change-contract-modal.component.html'
})
export class ChangeContractModalComponent {
  model = new ApplicationModificationDto();

  @Input() lotId: number;

  @ViewChild('resultForm') resultForm: NgForm;

  constructor(
    public modal: NgbActiveModal,
    private loadingIndicator: LoadingIndicatorService,
    private resource: ApplicationModificationResource,
  ) { }

  save(): void {
    if (!this.resultForm.form.valid) {
      return;
    }

    this.loadingIndicator.show();
    this.resource.changeEnteredContract(this.lotId, this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((result: ApplicationCommitDto) => {
        this.modal.close(result);
      });
  }
}
