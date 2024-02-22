import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { ListRoutingModule } from './list-routing.module';
import { SpecialityListComponent } from './components/speciality-list/speciality-list.component';
import { SpecialityListResolver } from './resolvers/speciality-list.resolver';
import { EmployerListComponent } from './components/employers-list/employer-list.component';
import { EmployerListResource } from './services/employer-list-resource';
import { SpecialityListSearchComponent } from './components/speciality-list/speciality-list-search/speciality-list-search.component';
import { SpecialityListSearchFilter } from './services/speciality-list-search.filter';
import { SpecialityListResource } from './services/speciality-list.resource';
import { EmployerListResolver } from './resolvers/employer-list.resolver';
import { EmployerListSearchComponent } from './components/employers-list/employer-list-search/employer-list-search.component';
import { EmployerListSearchFilter } from './services/employer-list-search.filter';
import { SchoolYearResource } from './services/school-year.resource';
import { EmployerItemEditModal } from './components/employers-list/employer-item-edit-modal/employer-item-edit-modal.component';
import { SpecialityItemEditModal } from './components/speciality-list/speciality-item-edit-modal/speciality-item-edit-modal.component';

@NgModule({
  declarations: [
    SpecialityListComponent,
    SpecialityListSearchComponent,
    SpecialityItemEditModal,
    EmployerListComponent,
    EmployerListSearchComponent,
    EmployerItemEditModal
  ],
  imports: [
    CommonModule,
    FormsModule,
    CommonBootstrapModule,
    TranslateModule,
    ListRoutingModule
  ],
  providers: [
    SpecialityListResource,
    SpecialityListResolver,
    SpecialityListSearchFilter,
    EmployerListResource,
    EmployerListResolver,
    EmployerListSearchFilter,
    SchoolYearResource
  ]
})
export class ListModule { }
