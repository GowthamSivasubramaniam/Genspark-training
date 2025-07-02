import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Profile } from './profile';
import { UserService } from '../../Services/UserServices';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';

describe('Profile Component', () => {
  let component: Profile;
  let fixture: ComponentFixture<Profile>;
  let userServiceSpy: jasmine.SpyObj<UserService>;

beforeEach(() => {
  userServiceSpy = jasmine.createSpyObj('UserService', ['getProfile', 'updateProfile']);
  userServiceSpy.getProfile.and.returnValue(of({ name: 'Test User', phone: '1234567890' }));

  TestBed.configureTestingModule({
    imports: [Profile, FormsModule],
    providers: [{ provide: UserService, useValue: userServiceSpy }],
  }).compileComponents();

  fixture = TestBed.createComponent(Profile);
  component = fixture.componentInstance;

  spyOn(localStorage, 'getItem').and.callFake((key: string) => key === 'email' ? 'test@example.com' : null);
  fixture.detectChanges();
});

  it('should create the profile component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch profile data on ngOnInit success', fakeAsync(() => {
    const profileData = { name: 'John Doe', phone: '1234567890' };
    userServiceSpy.getProfile.and.returnValue(of(profileData));

    component.ngOnInit();
    tick();

    expect(userServiceSpy.getProfile).toHaveBeenCalledWith('test@example.com');
    expect(component.data).toEqual(profileData);
  }));

  it('should handle error on profile fetch', fakeAsync(() => {
    userServiceSpy.getProfile.and.returnValue(throwError(() => new Error('Error fetching')));

    spyOn(console, 'error');

    component.ngOnInit();
    tick();

    expect(userServiceSpy.getProfile).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching profile:', jasmine.any(Error));
  }));

  it('should enable edit mode on onEdit()', () => {
    component.isEdited = false;
    component.onEdit();
    expect(component.isEdited).toBeTrue();
  });

  it('should update profile successfully', fakeAsync(() => {
    component.data = { name: 'Jane', phone: '0987654321' };
    userServiceSpy.updateProfile.and.returnValue(of({}));

    component.updateProfile();
    tick();

    expect(userServiceSpy.updateProfile).toHaveBeenCalledWith(
      { name: 'Jane', phone: '0987654321' },
      'test@example.com'
    );
    expect(component.isEdited).toBeFalse();
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Updated successfully');
  }));

  it('should handle error on profile update', fakeAsync(() => {
    component.data = { name: 'Jane', phone: '0987654321' };
    userServiceSpy.updateProfile.and.returnValue(
      throwError(() => ({ error: { message: 'Update failed' } }))
    );

    spyOn(console, 'error');

    component.updateProfile();
    tick();

    expect(component.isEdited).toBeFalse();
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Update failed');
    expect(console.error).toHaveBeenCalledWith('Update failed', jasmine.any(Object));
  }));
});
