import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WidgetPhoneBookComponent } from './widget-phone-book.component';

describe('WidgetPhoneBookComponent', () => {
  let component: WidgetPhoneBookComponent;
  let fixture: ComponentFixture<WidgetPhoneBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WidgetPhoneBookComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WidgetPhoneBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
