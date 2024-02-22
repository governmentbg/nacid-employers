import { Injectable } from '@angular/core';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { EmployerListItemDto } from 'src/modules/lists/models/employers-list/employer-list-item.dto';

@Injectable()
export class ApplicationSearchFilter extends BaseSearchFilter {
  registerNumber: string;
  contractNumber: string;
  signingDateFrom: Date | null;
  signingDateTo: Date | null;

  institutionId: number;
  institution: string;

  specialityId: number;
  speciality: string;

  employerListItemId: number;
  employerListItem: string;
  bulstat: string;

  studentName: string;
  studentUIN: string;

  state: CommitState | null;

  constructor() {
    super(10);
    this.state = null;
  }
}
