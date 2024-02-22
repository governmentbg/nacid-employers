import { AttachedFile } from "src/infrastructure/models/attached-file.model";
import { NomenclatureDto } from "src/modules/nomenclature/common/models/nomenclature-dto";

export class ApplicationTerminationDto {
  terminationDate: Date;
  terminationReason: NomenclatureDto;
  annexFile: AttachedFile;
}