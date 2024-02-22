import { Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { ApplicationCommitDto } from '../../models/application-commit.dto';
import { ApplicationTerminationDto } from '../../models/application-termination.dto';
import { ApplicationModificationResource } from '../../services/application-modification.resource';

@Component({
  selector: 'app-terminate-contract-modal',
  templateUrl: './terminate-contract-modal.component.html'
})
export class TerminateContractModalComponent {
  model = new ApplicationTerminationDto();

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
    this.resource.terminateContract(this.lotId, this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((result: ApplicationCommitDto) => {
        this.modal.close(result);
      });
  }
}
