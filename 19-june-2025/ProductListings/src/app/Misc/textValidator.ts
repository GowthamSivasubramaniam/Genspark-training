import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function textValidator():ValidatorFn
{
    return (control:AbstractControl):ValidationErrors|null =>
    {
        const value = control.value;
        if(value?.length<3)
        {
            return {lenError:"Text must be of length 4 or greater"}
        }
        return null;

    }
}