import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TiposUsuarioComponent } from './tipos-usuario.component';

describe('TiposUsuarioComponent', () => {
  let component: TiposUsuarioComponent;
  let fixture: ComponentFixture<TiposUsuarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TiposUsuarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TiposUsuarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
