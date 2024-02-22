import { AttachedFile } from "src/infrastructure/models/attached-file.model";

export class ApplicationModificationDto {
  modificationDate: Date;
  reason: string;
  annexFile: AttachedFile;
}