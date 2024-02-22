import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";
import { EducationType } from "../enums/education-type.enum";
import { StudentStatusType } from "../enums/student-status-type.enum";

export class ActualEducationDto {
  educationalQualification: NomenclatureDto;
  status: StudentStatusType | null;
  courseYear: string;
  graduationDate: Date | null;
  educationType: EducationType | null;

  constructor() {
    this.status = StudentStatusType.current;
    this.courseYear = '1';
  }
}