import { Injectable } from '@angular/core';
import { BaseSearchFilter } from 'src/infrastructure/services/base-search.filter';
import { UserStatus } from '../enums/user-status.enum';
import { Role } from '../models/role.dto';

@Injectable({
  providedIn: 'root'
})
export class UserSearchFilter extends BaseSearchFilter {
  firstName: string;
  middleName: string;
  lastName: string;
  username: string;
  email: string;

  roleId: number | null;
  role: Role = new Role();
  status: UserStatus | null;

  institution: string;
  institutionId: number;

  constructor() {
    super(10);
    this.status = null;
  }
}
