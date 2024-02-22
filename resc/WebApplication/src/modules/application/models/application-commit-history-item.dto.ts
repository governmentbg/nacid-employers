import { CommitState } from '../../../infrastructure/enums/commit-state.enum';

export class ApplicationCommitHistoryItemDto {
  commitId: number;
  lotId: number;

  state: CommitState;
  changeStateDescription: string;

  number: number;
  createDate: Date;

  contractNumber: string;
  creatorUser: string;
}
