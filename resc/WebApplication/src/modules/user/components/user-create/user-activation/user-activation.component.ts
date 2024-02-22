import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CommonFormComponent } from 'src/infrastructure/components/common-form.component';
import { RegExps } from 'src/infrastructure/constants/constants';
import { UserActivationDto } from 'src/modules/user/models/user-activation.dto';
import { UserActivationResource } from 'src/modules/user/resources/user-activation.resource';

@Component({
  selector: 'app-user-activation',
  templateUrl: './user-activation.component.html'
})
export class UserActivationComponent extends CommonFormComponent<UserActivationDto> implements OnInit {
  model: UserActivationDto = new UserActivationDto();

  passwordRegex = RegExps.PASSWORD_REGEX;

  constructor(
    private route: ActivatedRoute,
    private resource: UserActivationResource,
    private toastrService: ToastrService,
    private translateService: TranslateService,
    private router: Router
  ) {
    super();

    this.validationTexts = [...this.validationTexts, { key: 'pattern', value: this.translateService.instant('validations.password') }];
  }

  ngOnInit(): void {
    this.model.token = this.route.snapshot.queryParams.token;
  }

  activate(): void {
    if (this.model.password === this.model.passwordAgain) {
      this.resource.activateUser(this.model).subscribe(() => {
        this.toastrService.success(this.translateService.instant('toastr.userActivatePassword'));
        setTimeout(() => {
          this.router.navigate(['login']);
        }, 2000);
      });
    } else {
      this.toastrService.error(this.translateService.instant('toastr.userPasswordsDoesNotMatch'));
    }
  }
}
