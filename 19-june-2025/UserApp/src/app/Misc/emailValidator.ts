import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function emailValidator():ValidatorFn {
    return (control:AbstractControl):ValidationErrors |null=>
    {
        const val:string = control.value?.toLowerCase()
        if(val!=null && !val.includes('@gmail.com'))
        {
            return ({FormatError:'Should include @gmail.com'});
        }
        return null;
    }
    
}