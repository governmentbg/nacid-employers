import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { FileTemplateComponent } from './components/file-template/file-template.component';
import { MinisterComponent } from './components/minister.component';
import { NomenclatureHostComponent } from './components/nomenclature-host/nomenclature-host.component';

const routes: Routes = [
  {
    path: 'nomenclature',
    component: NomenclatureHostComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'minister',
        pathMatch: 'full'
      },
      {
        path: 'minister',
        component: MinisterComponent
      },
      {
        path: 'template',
        component: FileTemplateComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NomenclatureRoutingModule { }
