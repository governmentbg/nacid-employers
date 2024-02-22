import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { Configuration, configurationFactory } from 'src/infrastructure/configuration/configuration';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmployerListComponent } from './lists/components/employer-list/employer-list.component';
import { SpecialityListComponent } from './lists/components/speciality-list/speciality-list.component';
import { BaseListFilter } from './lists/services/base-list.filter';
import { EmployerListSearchFilter } from './lists/services/employer-list-search.filter';
import { EmployerListResource } from './lists/services/employer-list.resource';
import { SpecialityListSearchFilter } from './lists/services/speciality-list-search.filter';
import { SpecialityListResource } from './lists/services/speciality-list.resource';
import { LoadingIndicatorComponent } from './loading-indicator/loading-indicator.component';
import { LoadingIndicatorService } from './loading-indicator/services/loading-indicator.service';
import { ScrollToTopBtnComponent } from './scroll-to-top-btn/scroll-to-top-btn.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule } from '@angular/material/tabs';
import { SchoolYearResource } from './lists/services/school-year.resource';
import { SchoolYearResolver } from './lists/resolvers/school-year.resolver';
import { FooterComponent } from './footer/footer.component';
import { RegulationComponent } from './regulations/regulation.component';

@NgModule({
  declarations: [
    AppComponent,
    ScrollToTopBtnComponent,
    LoadingIndicatorComponent,
    EmployerListComponent,
    SpecialityListComponent,
    FooterComponent,
    RegulationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonBootstrapModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatTabsModule
  ],
  providers: [
    Configuration,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      deps: [Configuration],
      multi: true
    },
    LoadingIndicatorService,
    BaseListFilter,
    EmployerListSearchFilter,
    SpecialityListSearchFilter,
    EmployerListResource,
    SpecialityListResource,
    SchoolYearResource,
    SchoolYearResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
