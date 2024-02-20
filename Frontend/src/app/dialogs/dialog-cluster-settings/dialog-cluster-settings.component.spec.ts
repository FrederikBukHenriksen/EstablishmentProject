import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogClusterSettingsComponent } from './dialog-cluster-settings.component';

describe('DialogClusterSettingsComponent', () => {
  let component: DialogClusterSettingsComponent;
  let fixture: ComponentFixture<DialogClusterSettingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogClusterSettingsComponent]
    });
    fixture = TestBed.createComponent(DialogClusterSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
