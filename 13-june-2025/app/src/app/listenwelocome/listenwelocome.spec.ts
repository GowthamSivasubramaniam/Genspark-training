import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Listenwelocome } from './listenwelocome';

describe('Listenwelocome', () => {
  let component: Listenwelocome;
  let fixture: ComponentFixture<Listenwelocome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Listenwelocome]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Listenwelocome);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
