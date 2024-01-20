import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrossCorrelationComponent } from './cross-correlation.component';

describe('CrossCorrelationComponent', () => {
  let component: CrossCorrelationComponent;
  let fixture: ComponentFixture<CrossCorrelationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrossCorrelationComponent]
    });
    fixture = TestBed.createComponent(CrossCorrelationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
