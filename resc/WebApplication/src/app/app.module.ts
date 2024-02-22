import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { UserModule } from 'src/modules/user/user.module';
import { AppMenuComponent } from './app-menu/app-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppUserComponent } from './app-user/app-user.component';
import { AppComponent } from './app.component';
import { LoadingIndicatorComponent } from './loading-indicator/loading-indicator.component';
import { ScrollToTopBtnComponent } from './scroll-to-top-btn/scroll-to-top-btn.component';
import { ToastrModule } from 'ngx-toastr';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { LoadingIndicatorService } from './loading-indicator/services/loading-indicator.service';
import { Configuration, configurationFactory } from 'src/infrastructure/configuration/configuration';
import { InterceptorsModule } from 'src/infrastructure/interceptors/interceptors.module';
import { RoleService } from 'src/infrastructure/services/role.service';
import { NomenclatureModule } from 'src/modules/nomenclature/nomenclature.module';
import { ListModule } from 'src/modules/lists/list.module';
import { ApplicationModule } from 'src/modules/application/application.module';
import { SharedService } from 'src/infrastructure/services/shared.service';

@NgModule({
  declarations: [
    AppComponent,
    AppMenuComponent,
    AppUserComponent,
    ScrollToTopBtnComponent,
    LoadingIndicatorComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    CommonBootstrapModule,
    NomenclatureModule,
    UserModule,
    ListModule,
    AppRoutingModule,
    ApplicationModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    AuthGuard,
    LoadingIndicatorService,
    Configuration,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      deps: [Configuration],
      multi: true
    },
    InterceptorsModule,
    RoleService,
    SharedService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
