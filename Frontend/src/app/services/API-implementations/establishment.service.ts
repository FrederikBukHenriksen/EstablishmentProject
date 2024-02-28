import { Injectable, inject } from '@angular/core';
import {
  EstablishmentClient,
  EstablishmentDTO,
  GetEstablishmentsCommand,
} from 'api';
import { lastValueFrom } from 'rxjs';
import { SessionStorageService } from '../session-storage/session-storage.service';

export interface IEstablishmentService {
  getEstablishment(establishmentId?: string): Promise<EstablishmentDTO>;
  getEstablishmentsDTO(establishmentId: string[]): Promise<EstablishmentDTO[]>;
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
    return (await this.getEstablishmentsDTO([establishmentId]))[0];
  }

  public async getEstablishmentsDTO(
    establishmentId: string[]
  ): Promise<EstablishmentDTO[]> {
    var command = new GetEstablishmentsCommand();
    command.establishmentIds = establishmentId;

    return (
      await lastValueFrom(
        this.establishmentClient.getEstablishmentsDTO(command)
      )
    ).dtos;
  }
}
