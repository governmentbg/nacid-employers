import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/services/loading-indicator.service';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { ContentTypes, UserRoleAliases } from 'src/infrastructure/constants/constants';
import { RoleService } from 'src/infrastructure/services/role.service';
import { ReportTypeEnumLocalization } from 'src/modules/enum-localization.const';
import { SchoolYearResource } from 'src/modules/lists/services/school-year.resource';
import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { ReportType } from '../../enums/report-type.enum';
import { ApplicationReportDto } from '../../models/reports/application-report.dto';
import { ApplicationReportSearchFilter } from '../../services/application-report-search.filter';
import { ApplicationReportResource } from '../../services/application-report.resource';

@Component({
  selector: 'app-application-report',
  templateUrl: './application-report.component.html',
  styleUrls: ['./application-report.component.css']
})
export class ApplicationReportComponent implements OnInit {
  report: ApplicationReportDto = new ApplicationReportDto();
  showInstitution: boolean = false;
  showResearchArea: boolean = false;
  showSpeciality: boolean = false;
  defaultReport: boolean = false;

  isUniversityUser: boolean = false;
  universityUserInstitution: NomenclatureDto;
  currentSchoolYear: NomenclatureDto = new NomenclatureDto();
  schoolYearName: string;
  reportType = ReportType;

  contentTypes = ContentTypes;
  enumLocalization = ReportTypeEnumLocalization;

  reportTypeName: string;
  researchAreaName: string;
  specialityName: string;
  institutionName: string;
  educationalQualificationName: string;
  reportDate: Date;

  constructor(
    private resource: ApplicationReportResource,
    public filter: ApplicationReportSearchFilter,
    private loadingIndicator: LoadingIndicatorService,
    private roleService: RoleService,
    private configuration: Configuration,
    private schoolYearResource: SchoolYearResource
  ) {
  }

  ngOnInit(): void {
    this.filter.clear();
    this.filter.reportType = ReportType.defaultReport;

    this.universityUserInstitution = JSON.parse(localStorage.getItem(this.configuration.institutionProperty));
    this.isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

    if (this.isUniversityUser) {
      this.filter.institution = this.universityUserInstitution;
      this.filter.institutionId = this.universityUserInstitution.id;
      this.filter.institutionName = this.universityUserInstitution.name;
    } else {
      this.filter.institution = null;
      this.filter.institutionId = undefined;
      this.filter.institutionName = null;
    }
    this.loadingIndicator.show();
    this.schoolYearResource.getCurrentSchoolYear()
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((schoolYear: NomenclatureDto) => {
        this.currentSchoolYear = schoolYear;
        this.filter.schoolYearId = schoolYear.id;
        this.filter.schoolYear = schoolYear;
        this.schoolYearName = schoolYear.name;
        this.search();
      })
  }

  search(): void {
    this.filter.schoolYearName = this.schoolYearName;

    this.loadingIndicator.show();
    this.resource.getFiltered(this.filter)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((report: ApplicationReportDto) => {
        if (report.reportType != ReportType.defaultReport) {
          if (report.reportType == ReportType.reportByInstitution) {
            this.showInstitution = true;
            this.showSpeciality = false;
            this.showResearchArea = false;
          }
          else if (report.reportType == ReportType.reportByResearchArea) {
            this.showResearchArea = true;
            this.showSpeciality = false;
            this.showInstitution = false;
          }
          else if (report.reportType == ReportType.reportBySpecialty) {
            this.showSpeciality = true;
            this.showInstitution = false;
            this.showResearchArea = false;
          }
          else if (report.reportType == ReportType.reportByResearchAreaAndSpecialty) {
            this.showSpeciality = true;
            this.showResearchArea = true;
            this.showInstitution = false;
          }
          else if (report.reportType == ReportType.reportByResearchAreaAndSpecialityAndInstitution) {
            this.showSpeciality = true;
            this.showResearchArea = true;
            this.showInstitution = true;
          }

          this.defaultReport = false;
        }
        else {
          this.defaultReport = true;
          this.showSpeciality = false;
          this.showResearchArea = false;
          this.showInstitution = false;
        }

        this.schoolYearName = this.filter.schoolYear?.name;
        this.reportTypeName = this.enumLocalization[this.filter.reportType];
        this.researchAreaName = this.filter.researchArea?.name;
        this.specialityName = this.filter.speciality?.name;
        this.institutionName = this.filter.institution?.name;
        this.educationalQualificationName = this.filter.educationalQualification?.name;
        this.reportDate = new Date();
        this.filter.createdReportDate = this.reportDate;
        this.report = report;
      })
  }

  clearFilter(): void {
    this.filter.clear();
    this.filter.schoolYearId = this.currentSchoolYear.id;
    this.filter.schoolYear = this.currentSchoolYear;

    if (this.isUniversityUser) {
      this.filter.institution = this.universityUserInstitution;
      this.filter.institutionId = this.universityUserInstitution.id;
    }

    this.filter.reportType = ReportType.defaultReport;
    this.search();
  }

  onYearChange(): void {
    this.search();
  }

  onReportTypeChange(): void {
    this.filter.clear();
    this.filter.schoolYearId = this.currentSchoolYear.id;
    this.filter.schoolYear = this.currentSchoolYear;

    if (this.isUniversityUser) {
      this.filter.institution = this.universityUserInstitution;
      this.filter.institutionId = this.universityUserInstitution.id;
    }
  }
}
