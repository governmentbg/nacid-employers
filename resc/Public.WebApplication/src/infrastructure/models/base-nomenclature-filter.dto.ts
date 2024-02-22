export class BaseNomenclatureFilterDto {
  limit: number = 10;
  offset: number | null;

  textFilter: string | null;
  entityId: number | null;
  researchAreaId: number | null;
  educationalQualificationId: number | null;
  educationFormId: number | null;

  includeInactive: boolean | null;
}
