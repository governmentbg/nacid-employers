import { EducationType } from "../enums/education-type.enum";
import { StudentStatusType } from "../enums/student-status-type.enum";

export class StudentDto {
  firstName: string;
  middleName: string;
  lastName: string;
  uin: string;
  email: string;
  phoneNumber: string;

  educationType: EducationType | null;
  status: StudentStatusType | null;
  schoolYear: string;
  graduationDate: Date;

  constructor() {
    this.educationType = null;
    this.status = null;
  }
}