import { AbstractControl, ValidationErrors } from '@angular/forms';

export function twoDecimalValidator(
  control: AbstractControl,
): ValidationErrors | null {
  const value = control.value;

  const regex = /^\d+(\.\d{1,2})?$/;

  if (value && !regex.test(value)) {
    return { invalidFormat: true };
  }
  return null;
}
