import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogFilterSalesComponent } from './dialog-filter-sales.component';

describe('DialogFilterSalesComponent', () => {
  let component: DialogFilterSalesComponent;
  let fixture: ComponentFixture<DialogFilterSalesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogFilterSalesComponent]
    });
    fixture = TestBed.createComponent(DialogFilterSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
