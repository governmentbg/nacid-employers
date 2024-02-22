import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentFormComponent } from './components/common/student-form/student-form.component';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { UniversityFormComponent } from './components/common/university-form/university-form.component';
import { EmployerFormComponent } from './components/common/employer-form/employer-form.component';
import { ContractFormComponent } from './components/common/contract-form/contract-form.component';
import { ApplicationResource } from './services/application.resource';
import { ApplicationNewComponent } from './components/application-new/application-new.component';
import { TranslateModule } from '@ngx-translate/core';
import { ApplicationRoutingModule } from './application-routing.module';
import { PartModule } from 'src/infrastructure/part.module';
import { ApplicationCommitResolver } from './services/application-commit.resolver';
import { ApplicationCommitComponent } from './components/application-commit/application-commit.component';
import { ApplicationModificationResource } from './services/application-modification.resource';
import { ApplicationSearchComponent } from './components/application-search/application-search.component';
import { ApplicationSearchFilter } from './services/application-search.filter';
import { ActualEducationFormComponent } from './components/common/actual-education-form/actual-education-form.component';
import { ApplicationCommitHistoryComponent } from './components/application-commit-history/application-commit-history.component';
import { ApplicationCommitHistoryResolver } from './services/application-commit-history.resolver';
import { ChangeContractModalComponent } from './components/application-modal/change-contract-modal.component';
import { TerminateContractModalComponent } from './components/application-modal/terminate-contract-modal.component';
import { ApplicationDraftComponent } from './components/application-draft/application-draft.component';
import { ApplicationReportResource } from './services/application-report.resource';
import { ApplicationReportComponent } from './components/application-report/application-report.component';
import { ApplicationReportSearchFilter } from './services/application-report-search.filter';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    CommonBootstrapModule,
    TranslateModule,
    ApplicationRoutingModule,
    PartModule
  ],
  declarations: [
    StudentFormComponent,
    UniversityFormComponent,
    EmployerFormComponent,
    ContractFormComponent,
    ApplicationNewComponent,
    ApplicationCommitComponent,
    ApplicationSearchComponent,
    ActualEducationFormComponent,
    ApplicationCommitHistoryComponent,
    ChangeContractModalComponent,
    TerminateContractModalComponent,
    ApplicationDraftComponent,
    ApplicationReportComponent
  ],
  providers: [
    ApplicationResource,
    ApplicationModificationResource,
    ApplicationCommitResolver,
    ApplicationSearchFilter,
    ApplicationCommitHistoryResolver,
    ApplicationReportResource,
    ApplicationReportSearchFilter
  ],
})
export class ApplicationModule { }
