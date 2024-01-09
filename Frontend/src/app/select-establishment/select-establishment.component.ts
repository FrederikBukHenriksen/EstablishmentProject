import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Establishment, UserContextClient } from 'api';
import { SessionStorageService } from '../services/session-storage/session-storage.service';

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
  private router = inject(Router);
  private sessionStorageService = inject(SessionStorageService);

  protected accesibleEstablishments: TableOfAccesibleEstablishments[] = [];

  displayedColumns: string[] = ['name', 'actions'];
  protected dataSource: TableOfAccesibleEstablishments[] = [];

  constructor() {
    console.log('select-establishment');
    this.FetchAccesibleEstablishment();
  }

  private FetchAccesibleEstablishment() {
    this.userContextClient
      .getAccessibleEstablishments()
      .subscribe((x) => (this.dataSource = this.mapToTableObjects(x)));
  }

  private mapToTableObjects(
    input: Establishment[]
  ): TableOfAccesibleEstablishments[] {
    return input.map((x) => {
      return {
        id: x.id,
        name: x.name!,
      };
    });
  }

  protected onSelectEstablishment(establishmentId: string) {
    this.sessionStorageService.setActiveEstablishment(establishmentId);
    this.router.navigate(['/create-establishment']);
  }
}
