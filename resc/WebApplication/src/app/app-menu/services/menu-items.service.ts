import { Injectable } from '@angular/core';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { IMenuItem } from 'src/infrastructure/interfaces/IMenuItem';
import { RoleService } from 'src/infrastructure/services/role.service';

@Injectable({
  providedIn: 'root'
})
export class MenuItemsService {
  constructor(
    private roleService: RoleService
  ) { }

  getMainMenuItems(isLoggedInUser: boolean): IMenuItem[] {
    if (!isLoggedInUser) {
      return [];
    }

    const isAdministrator = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR);
    const isUniversityUser = this.roleService.hasRole(UserRoleAliases.UNIVERSITY_USER);

    return [
      {
        title: 'Договори', icon: 'file-earmark-fill', routerLink: '/application/search', isVisible: !isAdministrator
      },
      {
        title: 'Справки', icon: 'file-earmark-text-fill', routerLink: '/reports', isVisible: !isAdministrator
      },
      {
        title: 'Договори', icon: 'file-earmark-fill', isVisible: isAdministrator, children: [
          { title: 'Преглед договори', routerLink: '/application/search', isVisible: isAdministrator },
          { title: 'Справки по договори', routerLink: '/reports', isVisible: isAdministrator }
        ]
      },
      {
        title: 'Списъци', icon: 'file-earmark-text-fill', isVisible: !isUniversityUser, children: [
          { title: 'Специалности', routerLink: '/speciality', isVisible: !isUniversityUser },
          { title: 'Работодатели', routerLink: '/employer', isVisible: !isUniversityUser },
        ]
      },
      {
        title: 'Администрация', icon: 'wrench', isVisible: isAdministrator, children: [
          { title: 'Управление на потребители', routerLink: '/user/search', isVisible: isAdministrator },
          { title: 'Управление на номенклатури', routerLink: '/nomenclature', isVisible: isAdministrator },
        ]
      }
    ];
  }
}
