import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicationCommitDto } from '../models/application-commit.dto';
import { ApplicationModificationDto } from '../models/application-modification.dto';
import { ApplicationTerminationDto } from '../models/application-termination.dto';
import { ChangeStateDescription } from '../models/change-state.dto';

@Injectable()
export class ApplicationModificationResource extends BaseResource {

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'ApplicationModification');
  }

  startModification(lotId: number, changeStateDescription: ChangeStateDescription): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/startmodification`, changeStateDescription);
  }

  finishModification(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/finishmodification`, null);
  }

  cancelModification(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/cancelmodification`, null);
  }

  eraseApplication(lotId: number, changeStateDescription: ChangeStateDescription): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/erase`, changeStateDescription);
  }

  revertErasedApplication(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/revertErased`, null);
  }

  deleteLot(lotId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/lot/${lotId}`);
  }

  enterApplication(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/enter`, null);
  }

  startModificationEntered(lotId: number, changeStateDescription: ChangeStateDescription): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/startmodificationentered`, changeStateDescription);
  }

  finishEnteredModification(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/finishmodificationentered`, null);
  }

  changeEnteredContract(lotId: number, dto: ApplicationModificationDto): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/changecontract`, dto);
  }

  terminateContract(lotId: number, dto: ApplicationTerminationDto): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/terminate`, dto);
  }

  deleteDraft(lotId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/lot/${lotId}/deleteDraft`);
  }
}