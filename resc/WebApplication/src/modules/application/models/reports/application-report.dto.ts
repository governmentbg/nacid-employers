import { ReportType } from "../../enums/report-type.enum";
import { ApplicationReportItemDto } from "./application-report-item.dto";

export class ApplicationReportDto {
  totalCommits: number | null;
  totalEnteredCommits: number | null;
  totalChangedCommits: number | null;
  totalTerminatedCommits: number | null;
  totalExpiredCommits: number | null;
  reportType: ReportType;

  reports: ApplicationReportItemDto[] = [];
}