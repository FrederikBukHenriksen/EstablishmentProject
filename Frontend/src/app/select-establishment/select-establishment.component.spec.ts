import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectEstablishmentComponent } from './select-establishment.component';

describe('SelectEstablishmentComponent', () => {
  let component: SelectEstablishmentComponent;
  let fixture: ComponentFixture<SelectEstablishmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectEstablishmentComponent]
    });
    fixture = TestBed.createComponent(SelectEstablishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
