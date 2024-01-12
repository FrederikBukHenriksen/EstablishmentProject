import { Component, Input, OnInit } from '@angular/core';

export interface TableModel {
  columns: string[];
  elements: TableEntry[];
}

export interface TableCol {
  id: string;
  title: string;
}

export interface TableEntry {
  id: string;
  elements: TableElement[];
}

export interface TableElement {
  id: string;
}

export class TableButton implements TableElement {
  id: string;
  onClick!: () => void;

  constructor(id: string, onClick: () => void) {
    this.id = id;
    this.onClick = onClick;
  }
}

export class TableString implements TableElement {
  id: string;
  value: string;

  constructor(id: string, value: string) {
    this.id = id;
    this.value = value;
  }
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  @Input() public tableModel!: TableModel;
  // displayedColumns: string[] = ['button1', 'button2'];

  // public tableEntries: TableEntry[] = [
  //   {
  //     id: 'Hello',
  //     elements: [
  //       new TableButton('button1', () => console.log('knap')),
  //       new TableString('button2', 'gutentag'),
  //     ],
  //   },
  // ];

  handleButtonClick() {
    console.log('Button clicked!');
    // Add your logic here
  }
  constructor() {}

  ngOnInit() {
    console.log('tablemodel', this.tableModel);
  }

  protected GetClassName(input: any) {
    return input.constructor.name;
  }

  protected GetValue(col: string, element: TableElement) {
    if (element instanceof TableButton) {
      return element;
    } else {
      return element.id;
    }
  }

  protected GetRowElement(tableEntry: TableEntry, col: string): TableElement {
    return tableEntry.elements.find(
      (element) => element.id === col
    ) as TableElement;
  }

  public GetTableString(tableElement: TableElement): TableString {
    return tableElement as TableString;
  }

  public GetTableButton(tableElement: TableElement): TableButton {
    return tableElement as TableButton;
  }

  public GetAction(tableElement: TableElement): void {
    if (tableElement instanceof TableButton) {
      var casted = tableElement as TableButton;
      console.log('action', casted);
      casted.onClick;
    }
  }

  protected getTypeOfTableElement(element: TableElement): string {
    return element.constructor.name;
  }

  protected GetColumns() {
    return this.tableModel.columns;
  }
}
