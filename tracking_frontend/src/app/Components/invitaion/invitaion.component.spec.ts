import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitaionComponent } from './invitaion.component';

describe('InvitaionComponent', () => {
  let component: InvitaionComponent;
  let fixture: ComponentFixture<InvitaionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvitaionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvitaionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
