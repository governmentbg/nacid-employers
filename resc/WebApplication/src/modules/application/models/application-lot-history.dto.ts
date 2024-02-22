import { ApplicationCommitHistoryItemDto } from './application-commit-history-item.dto';

export class ApplicationLotHistoryDto {
  commits: ApplicationCommitHistoryItemDto[];
  actualCommitId: number;
}
