import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Videostream } from './videostream';

describe('Videostream', () => {
  let component: Videostream;
  let fixture: ComponentFixture<Videostream>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Videostream]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Videostream);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
