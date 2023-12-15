import { Component, OnInit } from '@angular/core';

export interface TableOfAccesibleEstablishments {
  name: string;
  id: string;
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  displayedColumns: string[] = ['name', 'actions'];

  public dataSource: TableOfAccesibleEstablishments[] = [];

  constructor() {}

  ngOnInit() {}

  protected onSelectEstablishment(establishmentId: string) {}
}
