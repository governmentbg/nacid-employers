import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { NomenclatureResource } from './common/services/nomenclature.resource';
import { FileTemplateComponent } from './components/file-template/file-template.component';
import { MinisterComponent } from './components/minister.component';
import { NomenclatureHostComponent } from './components/nomenclature-host/nomenclature-host.component';
import { NomenclatureRoutingModule } from './nomenclature-routing.module';
import { FileTemplateResource } from './services/file-template.resource';

@NgModule({
  declarations: [
    NomenclatureHostComponent,
    MinisterComponent,
    FileTemplateComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    CommonBootstrapModule,
    RouterModule,
    NomenclatureRoutingModule,
    TranslateModule
  ],
  providers: [
    NomenclatureResource,
    FileTemplateResource
  ]
})
export class NomenclatureModule { }
