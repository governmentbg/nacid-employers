import { SchoolYearDto } from "../school-year.dto";
import { SpecialityListItemDto } from "./speciality-list-item.dto";

export class SpecialityListDto {
  id: number;
  schoolYear: SchoolYearDto = new SchoolYearDto();
  isPublished: boolean;
  items: SpecialityListItemDto[] = [];
}