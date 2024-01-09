import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  Establishment,
  EstablishmentClient,
  EstablishmentDTO,
  UserContextClient,
} from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { lastValueFrom } from 'rxjs';
import { NGX_MAT_CALENDAR_RANGE_STRATEGY_PROVIDER_FACTORY } from '@angular-material-components/datetime-picker';

export interface TableOfAccesibleEstablishments {
  name: string;
  id: string;
}

@Component({
  selector: 'app-select-establishment',
  templateUrl: './select-establishment.component.html',
  styleUrls: ['./select-establishment.component.scss'],
})
export class SelectEstablishmentComponent {
  private userContextClient = inject(UserContextClient);
  private establishmentClient = inject(EstablishmentClient);

  private router = inject(Router);
  private sessionStorageService = inject(SessionStorageService);

  protected accesibleEstablishments: TableOfAccesibleEstablishments[] = [];

  displayedColumns: string[] = ['name', 'actions'];
  protected dataSource: TableOfAccesibleEstablishments[] = [];

  accesibleEstablishmentsIds: string[] = [];

  constructor() {
    console.log('select-establishment');
    this.FetchAccesibleEstablishment();
    this.dataSource = this.accesibleEstablishments.map((x) => {
      return {
        id: x.id,
        name: 'hello',
      };
    });
  }

  private async FetchAccesibleEstablishment() {
    var accEstablishmentsId = await lastValueFrom(
      this.userContextClient.getAccessibleEstablishments()
    );
    console.log('ids', accEstablishmentsId);

    var establishments = await lastValueFrom(
      this.establishmentClient.getEstablishments(accEstablishmentsId)
    );
    console.log('establishments', establishments);

    this.dataSource = this.mapToTableObjects(establishments);
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
    this.router.navigate(['/create-establishment']);
  }
}
