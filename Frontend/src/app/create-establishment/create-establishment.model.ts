import { FormControl, FormGroup } from '@angular/forms';

export class CreateEstablishmentModel {
  applyForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });
}
