import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { EmployerListComponent } from './components/employers-list/employer-list.component';
import { SpecialityListComponent } from './components/speciality-list/speciality-list.component';
import { EmployerListResolver } from './resolvers/employer-list.resolver';
import { SpecialityListResolver } from './resolvers/speciality-list.resolver';

const routes: Routes = [
  {
    path: 'speciality',
    component: SpecialityListComponent,
    resolve: {
      specialityList: SpecialityListResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'employer',
    component: EmployerListComponent,
    resolve: {
      employerList: EmployerListResolver
    },
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ListRoutingModule { }
