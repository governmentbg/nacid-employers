import { CommitState } from '../infrastructure/enums/commit-state.enum';
import { PartState } from '../infrastructure/enums/part-state.enum';
import { ReportType } from './application/enums/report-type.enum';
import { TaxType } from './application/enums/tax-type.enum';
// import { ApplicationLotResultType } from './application/enums/application-lot-result-type.enum';
import { UserStatus } from './user/enums/user-status.enum';

export const PartStateEnumLocalization = {
  [PartState.unchanged]: 'Непроменено',
  [PartState.modified]: 'Променено',
  [PartState.erased]: 'Заличено'
};

export const CommitStateEnumLocalization = {
  [CommitState.initialDraft]: 'Чернова',
  [CommitState.modification]: 'Върнат',
  [CommitState.actual]: 'Изпратен за вписване',
  [CommitState.actualWithModification]: 'Изпратен за вписване',
  [CommitState.history]: 'Върнат за редакция',
  [CommitState.deleted]: 'Изтрит',
  [CommitState.commitReady]: 'Готов за вписване',
  [CommitState.entered]: 'Вписан',
  [CommitState.enteredWithModification]: 'Вписан',
  [CommitState.enteredModification]: 'В редакция',
  [CommitState.enteredWithChange]: 'Вписан с изменение',
  [CommitState.terminated]: 'Прекратен',
  [CommitState.expired]: 'Изтекъл'
};

export const ReportTypeEnumLocalization = {
  [ReportType.defaultReport]: "Обща бройка обучаеми",
  [ReportType.reportBySpecialty]: "По специалност",
  [ReportType.reportByResearchArea]: "По професионално направление",
  [ReportType.reportByInstitution]: "По висше училище",
  [ReportType.reportByResearchAreaAndSpecialty]: "По професионално направление и специалност",
  [ReportType.reportByResearchAreaAndSpecialityAndInstitution]: "По професионално направление, специалност и ВУ",
}

// export const ApplicationLotResultTypeEnumLocalization = {
//   [ApplicationLotResultType.certificate]: 'Издадено удостоверение',
//   [ApplicationLotResultType.rejection]: 'Издаден отказ',
//   [ApplicationLotResultType.actual]: 'Изпратено за преглед',
//   [ApplicationLotResultType.modification]: 'Върнато за редакция',
//   [ApplicationLotResultType.deleted]: 'Изтрито'
// };

export const UserStatusEnumLocalization = {
  [UserStatus.active]: 'Активен',
  [UserStatus.inactive]: 'Неактивиран',
  [UserStatus.deactivated]: 'Деактивиран',
};

export const TaxTypeEnumLocalization = {
  [TaxType.full]: 'Таксата е покрита изцяло',
  [TaxType.partially]: 'Таксата е покрита частично'
};
