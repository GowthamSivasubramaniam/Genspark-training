import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function UserNameValidator():ValidatorFn
{
    return (control:AbstractControl):ValidationErrors|null =>
    {
        const value: string = control.value?.toLowerCase();
        if(value !=null && (value.includes('admin') || value.includes('root')))
        {
            return {ValueError:"Username cannot contain words like admin,root"}
        }
        return null;

    }
}