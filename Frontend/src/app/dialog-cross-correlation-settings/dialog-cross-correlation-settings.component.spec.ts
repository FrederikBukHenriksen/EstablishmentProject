import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCrossCorrelationSettingsComponent } from './dialog-cross-correlation-settings.component';

describe('DialogCrossCorrelationSettingsComponent', () => {
  let component: DialogCrossCorrelationSettingsComponent;
  let fixture: ComponentFixture<DialogCrossCorrelationSettingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogCrossCorrelationSettingsComponent]
    });
    fixture = TestBed.createComponent(DialogCrossCorrelationSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
