import {
  AfterViewChecked,
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

export interface TableModel {
  columns: string[];
  elements: TableEntry[];
}

export interface TableCol {
  id: string;
  title: string;
}

export interface TableEntry {
  id: any;
  elements: TableElement[];
}

export interface TableElement {
  id: string;
}

export class TableButton implements TableElement {
  id: string;
  text: string;
  onClick!: () => void;

  constructor(id: string, text: string, onClick: () => void) {
    this.id = id;
    this.text = text;
    this.onClick = onClick;
  }
}

export class TableCheckBox implements TableElement {
  id: string;
  value: boolean;
  action!: (value: boolean) => void;

  constructor(id: string, value: boolean, action: (value: boolean) => void) {
    this.id = id;
    this.value = value;
    this.action = action;
  }
}

export class TableMenu implements TableElement {
  id: string;
  text: string;
  options: TableMenuElement[];

  constructor(id: string, text: string, options: TableMenuElement[]) {
    this.id = id;
    this.text = text;
    this.options = options;
  }
}

export class TableMenuElement {
  text: string;
  action!: () => void;

  constructor(text: string, action: () => void) {
    this.text = text;
    this.action = action;
  }
}

export class TableString implements TableElement {
  id: string;
  text: string;

  constructor(id: string, value: string) {
    this.id = id;
    this.text = value;
  }
}

export class TableInput implements TableElement {
  id: string;

  constructor(id: string) {
    this.id = id;
  }
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush, // Make sure you really need this
})
export class TableComponent implements AfterViewInit {
  @Input() public tableModel!: TableModel;
  @Input() public addPaginator: boolean = false;

  constructor(private cdr: ChangeDetectorRef) {}

  dataSource!: MatTableDataSource<TableEntry>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    // Initialize MatTableDataSource with elements array
    this.dataSource = new MatTableDataSource(this.tableModel.elements);

    // If you want to add pagination
    if (this.addPaginator) {
      this.dataSource.paginator = this.paginator;
    }
    console.log('table, ngAfterViewInit', this.dataSource);
    console.log('table, ngAfterViewInit', this.dataSource.data);
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    if (changes['tableModel']) {
      this.tableModel = changes['tableModel'].currentValue;
      this.tableModel.columns = [...this.tableModel.columns];
      this.tableModel.elements = [...this.tableModel.elements];
      this.dataSource.data = this.tableModel.elements;
      console.log('table, ngOnChanges', this.tableModel);
      this.cdr.detectChanges();
    }
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

  public CastToTableString(tableElement: TableElement): TableString {
    return tableElement as TableString;
  }

  public CastToTableButton(tableElement: TableElement): TableButton {
    return tableElement as TableButton;
  }

  // protected getTypeOfTableElement(element: TableElement): string {
  //   return element.constructor.name;
  // }

  protected getTypeOfTableElement(element: TableElement): string {
    return element
      ? element.constructor
        ? element.constructor.name
        : 'UndefinedType'
      : 'UndefinedType';
  }

  protected GetColumns() {
    return this.tableModel.columns;
  }
  public CastToTableCheckbox(tableElement: TableElement): TableCheckBox {
    return tableElement as TableCheckBox;
  }

  //Table Menu
  public CastToTableMenu(tableElement: TableElement): TableMenu {
    return tableElement as TableMenu;
  }

  public GetTableMenuElements(tableMenu: TableMenu): TableMenuElement[] {
    return tableMenu.options;
  }
}
