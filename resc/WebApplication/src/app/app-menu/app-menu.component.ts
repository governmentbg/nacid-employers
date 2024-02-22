import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { IMenuItem } from 'src/infrastructure/interfaces/IMenuItem';
import { RoleService } from 'src/infrastructure/services/role.service';

@Component({
  selector: 'app-menu',
  templateUrl: './app-menu.component.html',
  styleUrls: ['./app-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppMenuComponent implements OnInit {
  @Input() menuItems: IMenuItem[] = [];

  filePath: string;
  isCollapsed = false;

  constructor(
    private roleService: RoleService
  ) { }

  ngOnInit() {
    if (this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR)) {
      this.filePath = 'assets/documents/RegEmployers Admin Help.pdf'
    } else if (this.roleService.hasRole(UserRoleAliases.CONTROL_USER)) {
      this.filePath = 'assets/documents/RegEmployers MON Help.pdf'
    } else if (this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER)) {
      this.filePath = 'assets/documents/RegEmployers HS Help.pdf'
    } else {
      this.filePath = 'assets/documents/RegEmployers HS Help.pdf'
    }
  }
}
