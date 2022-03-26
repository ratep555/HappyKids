import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrls: ['./add-manager.component.scss']
})
export class AddManagerComponent implements OnInit {
  managerForms: FormArray = this.fb.array([]);

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private router: Router) { }

  ngOnInit(): void {
    this.createManagerForm();
  }

  createManagerForm() {
    this.managerForms.push(this.fb.group({
      displayName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(20)]],
      email: [null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
      ],
      password: ['', [Validators.required,
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.compareValues('password')]]
    }));
  }

  private compareValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isEqual: true};
    };
  }

  recordSubmit(fg: FormGroup) {
    this.accountService.addManager(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('userslist');
      }, error => {
          console.log(error);
        });
      }
}
