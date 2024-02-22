import { AfterViewInit, Directive, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Directive({})
export abstract class CommonFormComponent<TModel> implements AfterViewInit {
  @Input() model: TModel;
  @Output() modelChange: EventEmitter<TModel> = new EventEmitter();

  @Input() isEditMode: boolean;

  @Output() isValidForm: EventEmitter<boolean> = new EventEmitter();

  @ViewChild(NgForm) form: NgForm;

  public validationTexts: { key: string, value: string }[] = [
    { key: 'required', value: 'Полето е задължително' }
  ];

  ngAfterViewInit(): void {
    if (this.form) {
      this.form.statusChanges
        .subscribe(() => {
          if (this.form && this.form.valid != undefined) {
            this.isValidForm.emit(this.form.valid);
          }
        });
    }
  }
}
