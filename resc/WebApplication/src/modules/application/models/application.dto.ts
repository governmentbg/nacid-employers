import { ActualEducationDto } from "./actual-education.dto";
import { ContractDto } from "./contract.dto";
import { EmployerDto } from "./employer.dto";
import { StudentDto } from "./student.dto";
import { UniversityDto } from "./university.dto";

export class ApplicationDto {
  student: StudentDto;
  university: UniversityDto;
  employer: EmployerDto;
  contract: ContractDto;
  actualEducation: ActualEducationDto;
  isDraft: boolean;

  constructor() {
    this.student = new StudentDto();
    this.university = new UniversityDto();
    this.employer = new EmployerDto();
    this.contract = new ContractDto();
    this.actualEducation = new ActualEducationDto();
  }
}