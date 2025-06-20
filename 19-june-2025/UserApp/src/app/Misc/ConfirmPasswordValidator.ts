import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function ConfirmPasswordvalidator():ValidatorFn {
    return (control:AbstractControl):ValidationErrors |null=>
    {
           const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password && confirmPassword && password !== confirmPassword) {
      return { MismatchError: 'Passwords do not match' };
    }

        return null;
    }
    
}