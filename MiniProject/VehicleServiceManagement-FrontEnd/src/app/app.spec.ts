import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { App } from './app';

describe('App Component', () => {
  let component: App;
  let fixture: ComponentFixture<App>;
  let mockRouter: any;

  beforeEach(async () => {
    mockRouter = {
      url: ''
    };

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(App);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app component', () => {
    expect(component).toBeTruthy();
  });

  it('should return true for showLayout when URL is /login, /home or /register', () => {
    ['login', 'home', 'register'].forEach(path => {
      mockRouter.url = '/' + path;
      expect(component.showLayout).toBeTrue();
    });
  });

  it('should return false for showLayout when URL is different', () => {
    mockRouter.url = '/dashboard';
    expect(component.showLayout).toBeFalse();
  });

  it('should toggle showLogin and showRegister flags correctly', () => {
    component.showlogin();
    expect(component.showLogin).toBeTrue();

    component.showregister();
    expect(component.showRegister).toBeTrue();

    component.cancel();
    expect(component.showLogin).toBeFalse();
    expect(component.showRegister).toBeFalse();
  });
});
