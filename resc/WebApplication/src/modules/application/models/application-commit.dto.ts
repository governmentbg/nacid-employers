import { CommitState } from "src/infrastructure/enums/commit-state.enum";
import { PartDto } from "src/infrastructure/models/part.dto";
import { ActualEducationDto } from "./actual-education.dto";
import { ApplicationModificationDto } from "./application-modification.dto";
import { ApplicationTerminationDto } from "./application-termination.dto";
import { ContractDto } from "./contract.dto";
import { EmployerDto } from "./employer.dto";
import { StudentDto } from "./student.dto";
import { UniversityDto } from "./university.dto";

export class ApplicationCommitDto {
  id: number;
  lotId: number;
  state: CommitState;
  hasOtherCommits: boolean;

  studentPart: PartDto<StudentDto>;
  universityPart: PartDto<UniversityDto>;
  employerPart: PartDto<EmployerDto>;
  contractPart: PartDto<ContractDto>;
  actualEducationPart: PartDto<ActualEducationDto>;

  applicationModification: ApplicationModificationDto[] = [];
  applicationTermination: ApplicationTerminationDto;

  changeStateDescription: string;
}