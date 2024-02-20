import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogGraphSettingsComponent } from './dialog-graph-settings.component';

describe('DialogGraphSettingsComponent', () => {
  let component: DialogGraphSettingsComponent;
  let fixture: ComponentFixture<DialogGraphSettingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogGraphSettingsComponent]
    });
    fixture = TestBed.createComponent(DialogGraphSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
