import { AbstractControl, FormGroup } from "@angular/forms";

export class ValidatorField {
  static Mustmatch(controlName: string, matchingControName: string): any{
    return (group: AbstractControl) =>{
      const formGroup = group as FormGroup;
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControName];

      if(matchingControl.errors && !matchingControl.errors.Mustmatch){
        return null;
      }

      if(control.value !== matchingControl.value){
        matchingControl.setErrors({Mustmatch: true});
      }
      else{
        matchingControl.setErrors(null);
      }

      return null;
    };
  }
}
