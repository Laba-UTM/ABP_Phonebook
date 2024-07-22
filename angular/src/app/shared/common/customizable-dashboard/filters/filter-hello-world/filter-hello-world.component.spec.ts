import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterHelloWorldComponent } from './filter-hello-world.component';

describe('FilterHelloWorldComponent', () => {
  let component: FilterHelloWorldComponent;
  let fixture: ComponentFixture<FilterHelloWorldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterHelloWorldComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterHelloWorldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
