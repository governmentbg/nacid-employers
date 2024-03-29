import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-action-confirmation-modal',
  templateUrl: './action-confirmation-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ActionConfirmationModalComponent {
  description = "";

  @Input() showTextArea: boolean = false;

  @Input() confirmationMessage: string;
  @Input() confirmationButton: string;

  @Output() passDescription: EventEmitter<string> = new EventEmitter();

  constructor(public modal: NgbActiveModal) { }

  closeModal() {
    this.passDescription.emit(this.description);
    this.modal.close(true);
  }
}
