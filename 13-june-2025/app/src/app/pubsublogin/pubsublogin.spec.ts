import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Pubsublogin } from './pubsublogin';

describe('Pubsublogin', () => {
  let component: Pubsublogin;
  let fixture: ComponentFixture<Pubsublogin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Pubsublogin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Pubsublogin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
