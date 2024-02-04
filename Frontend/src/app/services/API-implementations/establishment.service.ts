import { Injectable, inject } from '@angular/core';
import {
  EstablishmentClient,
  EstablishmentDTO,
  GetEstablishmentCommand,
  GetMultipleEstablishmentsCommand,
} from 'api';
import { lastValueFrom } from 'rxjs';
import { SessionStorageService } from '../session-storage/session-storage.service';

export interface IEstablishmentService {
  getEstablishment(establishmentId?: string): Promise<EstablishmentDTO>;
  getEstablishments(establishmentId: string[]): Promise<EstablishmentDTO[]>;
}

@Injectable({
  providedIn: 'root',
})
export class EstablishmentService implements IEstablishmentService {
  private readonly establishmentClient = inject(EstablishmentClient);
  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async getEstablishment(
    establishmentId?: string
  ): Promise<EstablishmentDTO> {
    establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    return (await this.getEstablishments([establishmentId]))[0];
  }

  public async getEstablishments(
    establishmentId: string[]
  ): Promise<EstablishmentDTO[]> {
    var command = new GetMultipleEstablishmentsCommand();
    command.establishmentIds = establishmentId;

    return (
      await lastValueFrom(this.establishmentClient.getEstablishments(command))
    ).establishmentDTOs;
  }
}
