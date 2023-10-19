import { FormControl, FormGroup } from '@angular/forms';

export const loginFormDef = new FormGroup({
  name: new FormControl(''),
  password: new FormControl(''),
});
