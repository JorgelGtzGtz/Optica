import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlmacenesComponent } from './almacenes.component';

describe('AlmacenesComponent', () => {
  let component: AlmacenesComponent;
  let fixture: ComponentFixture<AlmacenesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlmacenesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlmacenesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
