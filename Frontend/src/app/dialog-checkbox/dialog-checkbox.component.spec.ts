/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DialogBase } from './dialog-checkbox.component';

describe('DialogCheckboxComponent', () => {
  let component: DialogBase;
  let fixture: ComponentFixture<DialogBase>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DialogBase],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogBase);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
