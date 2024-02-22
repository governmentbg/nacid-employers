import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployerListComponent } from './lists/components/employer-list/employer-list.component';
import { SpecialityListComponent } from './lists/components/speciality-list/speciality-list.component';
import { SchoolYearResolver } from './lists/resolvers/school-year.resolver';
import { RegulationComponent } from './regulations/regulation.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'specialityList',
    pathMatch: 'full',
    resolve: {
      schoolYear: SchoolYearResolver
    }
  },
  {
    path: 'specialityList',
    component: SpecialityListComponent,
    resolve: {
      schoolYear: SchoolYearResolver
    }
  },
  {
    path: 'employerList',
    component: EmployerListComponent,
    resolve: {
      schoolYear: SchoolYearResolver
    }
  },
  {
    path: 'regulations',
    component: RegulationComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
