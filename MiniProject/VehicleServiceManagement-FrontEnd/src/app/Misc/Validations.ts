import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";


export function nameValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const val = control.value;
    if (val && !/^[A-Za-z\s]{3,}$/.test(val)) {
      return { Error: 'Name must be at least 3 letters and alphabets only' };
    }
    return null;
  };
}


export function emailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const val: string = control.value?.toLowerCase();
    if (val && !val.includes('@gmail.com')) {
      return { Error: 'Email should include @gmail.com' };
    }
    return null;
  };
}


export function phoneValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const val = control.value;
    if (val && !/^\d{10}$/.test(val)) {
      return { Error: 'Phone number must be 10 digits' };
    }
    return null;
  };
}


export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const val = control.value;
    if (
      val &&
      !/(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}/.test(val)
    ) {
      return { Error: 'Password must be at least 6 chars and include uppercase, lowercase, and number' };
    }
    return null;
  };
}


export function vehicleNoValidator(): ValidatorFn {
 


  return (control: AbstractControl): ValidationErrors | null => {
    const val: string = control.value;
    const pattern = /^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{4}$/;

    if (val && !pattern.test(val)) {
      return { Error: 'Invalid vehicle registration number format' };
    }
    return null;
  };
}



export function nonEmptyValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const val = control.value;
    if (!val || val.trim().length === 0) {
      return { Error: 'This field is required' };
    }
    return null;
  };
}
export function priceValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (value === null || value === undefined || value === '') {
      return { Error: 'Price is required' };
    }

    const num = parseFloat(value);

    if (isNaN(num) || num <= 0) {
      return { Error: 'Price must be a positive number' };
    }

    if (!/^\d+(\.\d{1,2})?$/.test(value)) {
      return { Error: 'Price can have up to 2 decimal places' };
    }

    return null;
  };
}






export function manufacturerValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;
    const regex = /^[A-Za-z\s]{3,20}$/;
    return regex.test(value) ? null : { invalidManufacturer: true };
  };
}

export function modelYearValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = Number(control.value);
    const currentYear = new Date().getFullYear();
    if (!value || isNaN(value)) return { invalidModel: true };
    return (value >= 1980 && value <= currentYear) ? null : { invalidModel: true };
  };
}
