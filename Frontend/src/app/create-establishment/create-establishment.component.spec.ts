import { ComponentFixture, TestBed } from '@angular/core/testing';

import CreateEstablishmentComponent from './create-establishment.component';

describe('CreateEstablishmentComponent', () => {
  let component: CreateEstablishmentComponent;
  let fixture: ComponentFixture<CreateEstablishmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateEstablishmentComponent],
    });
    fixture = TestBed.createComponent(CreateEstablishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
