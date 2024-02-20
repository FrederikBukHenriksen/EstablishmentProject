import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { EstablishmentClient, EstablishmentDTO, UserContextClient } from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';
import { NGX_MAT_CALENDAR_RANGE_STRATEGY_PROVIDER_FACTORY } from '@angular-material-components/datetime-picker';
import { EstablishmentService } from '../services/API-implementations/establishment.service';
import { UserContextService } from '../services/API-implementations/usercontext.service';

export interface TableOfAccesibleEstablishments {
  name: string;
  id: string;
}

@Component({
  selector: 'app-select-establishment',
  templateUrl: './select-establishment.component.html',
})
export class SelectEstablishmentComponent {
  private userContextService = inject(UserContextService);
  private establishmentService = inject(EstablishmentService);

  private sessionStorageService = inject(SessionStorageService);
  protected accesibleEstablishments: TableOfAccesibleEstablishments[] = [];

  displayedColumns: string[] = ['name', 'actions'];
  protected dataSource: TableOfAccesibleEstablishments[] = [];

  accesibleEstablishmentsIds: string[] = [];

  constructor() {
    this.FetchAccesibleEstablishment();
  }

  private async FetchAccesibleEstablishment() {
    var accessibleEstablishmentsId =
      await this.userContextService.getAccessibleEstablishments();
    var accessibleEstablishmentDTOs =
      await this.establishmentService.getEstablishmentsDTO(
        accessibleEstablishmentsId
      );

    this.dataSource = this.mapToTableObjects(
      accessibleEstablishmentDTOs as EstablishmentDTO[]
    );
  }

  private mapToTableObjects(
    input: EstablishmentDTO[]
  ): TableOfAccesibleEstablishments[] {
    return input.map((x) => {
      return {
        id: x.id,
        name: x.name,
      };
    });
  }

  protected onSelectEstablishment(establishmentId: string) {
    this.sessionStorageService.setActiveEstablishment(establishmentId);
  }
}
